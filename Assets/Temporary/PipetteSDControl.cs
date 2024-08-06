using System.Collections;
using UnityEngine;

using UltimateXR.Core;
using UltimateXR.Manipulation;
using UltimateXR.Devices;
using UltimateXR.Avatar;

public class PipetteSDControl : MonoBehaviour
{
    public GameObject liquidChild;
    public Color liquidColour;
    public Renderer liquidRenderer;
    public UxrGrabbableObject grabbableObject;

    private Collider collider;

    private bool _filled = false;
    private bool _collidedWithLiquid = false;

    private GameObject _collidedLiquid;
    private Color _collidedColour = Color.white;

    private void Start() {
        grabbableObject = GetComponent<UxrGrabbableObject>();
        collider = GetComponent<Collider>();

        // Finding the Liquid Child and set the renderer
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

    private void Update() {
        if (grabbableObject.IsBeingGrabbed) {
            collider.isTrigger = true; // Sets the collider as a trigger so it doesn't cause interacting objects to be affected by physics

            bool leftTriggered = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Left, UxrInputButtons.Trigger, UxrButtonEventType.PressDown);
            bool rightTriggered = UxrAvatar.LocalAvatarInput.GetButtonsEvent(UxrHandSide.Right, UxrInputButtons.Trigger, UxrButtonEventType.PressDown);

            bool triggered = leftTriggered || rightTriggered;

            if (triggered) {
                if (_collidedWithLiquid) {
                    if (!_filled) {
                        FillPipette();
                        Debug.Log("Filling Pipette");
                    } else {
                        EmptyPipette();
                        Debug.Log("Emptying Pipette");
                    }
                } else {
                    Debug.Log("Not touching a liquid GameObject");
                }
            }
        } else {
            collider.isTrigger = false;
        }
    }

    private void FillPipette() {
        _filled = true;
        liquidColour = _collidedColour;
        liquidColour.a = 0.8f;
        liquidRenderer.material.color = liquidColour;
    }

    private void EmptyPipette() {
    _filled = false;
    if (_collidedLiquid != null) {
        // Get the parent GameObject of the collided liquid
        GameObject parentObject = _collidedLiquid.transform.parent.gameObject;

        // Try to get the LiquidDiluting component from the parent
        LiquidDiluting liquidDiluting = parentObject.GetComponent<LiquidDiluting>();
        if (liquidDiluting != null) {
            Debug.Log("LiquidDiluting script found on the parent!");
            liquidDiluting.MixLiquid(liquidColour, 2.0f);

            // Reset the colour of the liquid inside the pipette
            liquidColour = Color.white;
            liquidColour.a = 0.1f;
            liquidRenderer.material.color = liquidColour;
        } else {
            Debug.Log("LiquidDiluting script not found on the parent!");
        }
    } else {
        Debug.Log("No collided liquid to interact with!");
    }
}


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
}
