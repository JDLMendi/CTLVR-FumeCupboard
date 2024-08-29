using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Mechanics;
using UltimateXR.Manipulation;
using UltimateXR.Avatar;
using UltimateXR.Haptics;

public class SeparatoryFunnel : MonoBehaviour
{
    public LayeredLiquidController liquidController;
    public GameObject stopcock;
    public ParticleSystem liquidEmitter;
    public AudioSource soundFx;
    public Renderer mixedLiquidRenderer;
    private Transform _stopcockTransform;
    private float _stopcockRotation;
    private bool  _executeOncePerShake;
    private float _actionCooldown = 0.5f;
    private bool _isSeperated = false;
    private float _lastActionTime = -Mathf.Infinity;
    private bool _isPlaced;
    private bool _isStopperOn;
    [SerializeField] private UxrShakeDetector _shakeDetector;

    private void Start() {
        _stopcockTransform = stopcock.GetComponent<Transform>();
        _shakeDetector = GetComponent<UxrShakeDetector>();
        soundFx = GetComponent<AudioSource>();
        if (_shakeDetector == null) Debug.Log("ShakeDetector not found!");
        _executeOncePerShake = true;
    }
    
    private void OnEnable() {
        _shakeDetector.OnShake += OnShake;
        UxrGrabManager.Instance.ObjectPlaced += OnPlaced;
        UxrGrabManager.Instance.ObjectRemoved += OnRemoved;
    }

    private void OnDisable() {
         _shakeDetector.OnShake -= OnShake;
        UxrGrabManager.Instance.ObjectPlaced -= OnPlaced;
        UxrGrabManager.Instance.ObjectRemoved -= OnRemoved;
    }

    private void Update() {
        // Print the local x rotation of the Stopcock
        float xLocalRotation = _stopcockTransform.localRotation.eulerAngles.x;
        // Debug.Log(xLocalRotation);

        if (!_isStopperOn && _isSeperated && ((xLocalRotation >= 70f && xLocalRotation <= 110f) || (xLocalRotation >= 250f && xLocalRotation <= 290f)))
        {
           
            // Activate the particle system if the rotation is within the specified ranges
            if (!liquidEmitter.isPlaying)
            {
                
                liquidEmitter.Play();
            }

            liquidController.DecreaseFirstLiquid(0.00002f);
            if (liquidController.firstLiquidScale <= 0) liquidEmitter.Stop();
        }
        else
        {   
            // Deactivate the particle system if the rotation is outside the specified ranges
            if (liquidEmitter.isPlaying)
            {
                liquidEmitter.Stop();
            }
        }
    }


    private void OnPlaced(object sender, UxrManipulationEventArgs e) {
        if (e.GrabbableObject.name == "Stopper") {
            Debug.Log("Stopper is placed");
            _isStopperOn = true;
            return;
        }

        _isPlaced = true;
        if (_isSeperated) liquidController.enabled = true;
    }

    private void OnRemoved(object sender, UxrManipulationEventArgs e) {
        if (e.GrabbableObject.name == "Stopper") {
            Debug.Log("Stopper is removed");
            _isStopperOn = false;
            return;
        }

        _isPlaced = false;
        liquidController.enabled = false;
    }
    private void OnShake()
    {
        if (!_isStopperOn) return; //Stopper should be on before shaking
        
        Debug.Log("OnShake has been invoked!");
            // A check for whether we want to only execute this method once in a specific timeframe.
        if (_executeOncePerShake)
        {
                // Check whether the cooldown period has gone since the last action. Return if it isn't.
            if (!(Time.time >= _lastActionTime + _actionCooldown)) return;
        }

        // Execute the functionality.
        if (!_isSeperated) {
            UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback(this.gameObject.GetComponent<UxrGrabbableObject>(), UxrHapticClipType.RumbleFreqLow);
            soundFx.Play();
        }
        _lastActionTime = Time.time;
        Color color = mixedLiquidRenderer.material.color;
        color.a = Mathf.Clamp01(color.a - 0.03f);
        mixedLiquidRenderer.material.color = color;

        if (color.a <= 0.3) _isSeperated = true;
    }

}
