using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using UltimateXR.Core;
using UltimateXR.Manipulation;
using UltimateXR.Devices;
using UltimateXR.Haptics;
using UltimateXR.Avatar;

public enum BulbHolding {
    Air,
    Empty,
    Suction,
    Shaft,
    Nothing
}
public class BulbPipette : MonoBehaviour
{

    public UxrGrabbableObject grabbableObject;
    public GameObject baseBulb;
    public GameObject liquidChild;
    public Color liquidColour;
    public Renderer liquidRenderer;
    public ParticleSystem particleSystem;
    
    // Holds what hand is holding
    private BulbHolding _leftHolding;
    private BulbHolding _rightHolding;
    private Collider _collider;
    private bool _noAir = false; // Used in conjunction with Suction and Air
    private bool _beingHeld = false;
    private bool _isFilled = false;
    private bool _collidedWithLiquid = false;
    private GameObject _collidedLiquid;
    private Color _collidedColour = Color.white;
    private Coroutine _fillCoroutine;
    private Coroutine _emptyCoroutine;
    private void Start() {
        _leftHolding = _rightHolding = BulbHolding.Nothing;
        grabbableObject = GetComponent<UxrGrabbableObject>();
        _collider = GetComponent<Collider>();

        Transform liquidTransform = transform.Find("Liquid");
        if (liquidTransform != null) {
            Debug.LogWarning("Liquid Child Object found!");
            liquidChild = liquidTransform.gameObject;
            liquidRenderer = liquidChild.GetComponent<Renderer>();
            if (liquidRenderer) liquidColour = liquidRenderer.material.color;
            else Debug.Log("Renderer not found!");
        } else {
            Debug.LogWarning("Liquid Child Object not found!");
        }
    }
    private void OnEnable() {

        // Holding Grab Points
        UxrGrabManager.Instance.ObjectGrabbed += GrabHold;
        UxrGrabManager.Instance.ObjectReleasing += GrabRelease;

        // Button interaction
        UxrControllerInput.GlobalButtonStateChanged += TriggerPressed;
    }

    private void OnDisable() {
        UxrGrabManager.Instance.ObjectGrabbed -= GrabHold;
        UxrGrabManager.Instance.ObjectReleasing -= GrabRelease;
        UxrControllerInput.GlobalButtonStateChanged -= TriggerPressed;
    }

    private void GrabHold(object sender, UxrManipulationEventArgs e) {
        // Debug.Log($"Grab Index: {e.GrabPointIndex}");

        if (e.GrabbableObject != grabbableObject) return;
        _collider.isTrigger = true;

        Debug.Log($"Hand side: {e.Grabber.Side}");
        BulbHolding bulbHolding = BulbHolding.Nothing;
        switch (e.GrabPointIndex) {
            case 0: // Shaft
                bulbHolding = BulbHolding.Shaft;
                break;
            case 1: // Air
                bulbHolding = BulbHolding.Air;
                break;
            case 2: // Suction
                bulbHolding = BulbHolding.Suction;
                break;
            case 3: // Empty
                bulbHolding = BulbHolding.Empty;
                break; 
            default:
                break;
        }

        switch (e.Grabber.Side) {
            case UxrHandSide.Left:
                _leftHolding = bulbHolding;
                Debug.Log($"Holding on the Left Hand: {bulbHolding}" );
                break;
            case UxrHandSide.Right:
                _rightHolding = bulbHolding;
                Debug.Log($"Holding on the Right Hand: {bulbHolding}" );
                break;
        }
    }

    private void GrabRelease(object sender, UxrManipulationEventArgs e) {
        if (e.GrabbableObject != grabbableObject) return;

        switch (e.Grabber.Side) {
            case UxrHandSide.Left:
                _leftHolding = BulbHolding.Nothing;
                Debug.Log($"Holding nothing on the Left");
                break;
            case UxrHandSide.Right:
                _rightHolding = BulbHolding.Nothing;
                Debug.Log($"Holding nothing on the Right");
                break;
        }

        if (!e.IsMultiHands) {
            _collider.isTrigger = false;
        }
    }

    // Collision when the pipette enters a gameobject with a 'Liquid' Tag
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Liquid")) {
            _collidedWithLiquid = true;
            _collidedLiquid = other.gameObject;

            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null) {
                _collidedColour = renderer.material.color;
                // Debug.Log("Collided with liquid. Color: " + _collidedColour);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Liquid")) {
            _collidedWithLiquid = false;
            _collidedLiquid = null;
            Debug.Log("Trigger has left the liquid!");
        }
    }

    // When a button is pressed, this should determine if the button is a Trigger (front button of controller).
    // 1. Check what hand the trigger is being pulled and if it is holding something
    
    private void TriggerPressed(object sender, UxrInputButtonEventArgs e) {

        // Checks if the button being held is the trigger.
        if (e.Button != UxrInputButtons.Trigger || e.ButtonEventType != UxrButtonEventType.Pressing) return;

        UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback (grabbableObject, UxrHapticClipType.Click);

        // Determine which hand is being pressed with
        bool isHoldingImportant;
        BulbHolding holding;
        switch (e.HandSide) {
            case UxrHandSide.Left:
                holding = _leftHolding;
                break;
            case UxrHandSide.Right:
                holding = _rightHolding;
                break;
            default:
                return;
        }
        
        isHoldingImportant = holding != BulbHolding.Nothing && holding != BulbHolding.Shaft;

        // Call corresponding function depending on which value is being held down.
        if (isHoldingImportant) {
            switch (holding) {
                case BulbHolding.Air:
                    PressingAir();
                    break;
                case BulbHolding.Suction:
                    PressingSuction();
                    break;
                case BulbHolding.Empty:
                    PressingEmpty();
                    break;
            }
        }
    }
    private void PressingAir() {
        _noAir = true;
        baseBulb.SetActive(false);
    }

    private void PressingSuction() {
        if (_noAir) {
            FillPipette();
            // Check if collider is colliding with a liquid gameobject and get the colour, empty it out and fill the pipette with the colour
        }
    }
    private void PressingEmpty() {
        _noAir = false;
        EmptyPipette();

        // Show Base Bulb
        // Check if it collides with a colourless liquid gameobject and cause it to change the opacity and colour when it is.
        // Otherwise, if it is not colliding with anything then could possibly not do anything or do a PS and empty the liquid from the pipette.
    }

    private void FillPipette()
    {
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
        }
        _fillCoroutine = StartCoroutine(LerpFillColor(_collidedColour, 0.6f, 1f)); // Duration of 1 second
    }

    private void EmptyPipette()
    {
        if (_collidedLiquid != null && _isFilled)
        {
            // Get the parent GameObject of the collided liquid
            GameObject parentObject = _collidedLiquid.transform.parent.gameObject;
            // Try to get the LiquidDiluting component from the parent
            LiquidDiluting liquidDiluting = parentObject.GetComponent<LiquidDiluting>();
            if (liquidDiluting != null)
            {
                Debug.Log("LiquidDiluting script found on the parent!");
                liquidDiluting.MixLiquid(liquidRenderer.material.color, 0.6f);    
            }
            else
            {
                Debug.Log("LiquidDiluting script not found on the parent!");
            }
        }
        else if (_collidedLiquid == null && _isFilled)
        {
            particleSystem.startColor = liquidColour;
            particleSystem.Play();
        }

        baseBulb.SetActive(true);
        
         // If a coroutine is already running, stop it
        if (_emptyCoroutine != null)
        {
            StopCoroutine(_emptyCoroutine);
        }

        // Start the coroutine to lerp the color back to white
        _emptyCoroutine = StartCoroutine(LerpEmptyColor(Color.white, 0.1f, 1f)); // Duration of 1 second
    }

    

    private IEnumerator LerpEmptyColor(Color targetColor, float targetAlpha, float duration)
    {
        Color startColor = liquidRenderer.material.color;
        targetColor.a = targetAlpha;
        float time = 0f;

        while (time < duration)
        {
            liquidRenderer.material.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set
        liquidRenderer.material.color = targetColor;
        _isFilled = false;
    }


    private IEnumerator LerpFillColor(Color targetColor, float targetAlpha, float duration)
    {
        _isFilled = true;
        Color startColor = liquidRenderer.material.color;
        targetColor.a = targetAlpha;
        float time = 0f;

        while (time < duration)
        {
            liquidRenderer.material.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set
        liquidRenderer.material.color = targetColor;
    }

}


