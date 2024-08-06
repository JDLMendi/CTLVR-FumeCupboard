using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UltimateXR.Core;
using UltimateXR.Manipulation;
using UltimateXR.Devices;
using Unity.Mathematics;

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
    
    // Holds what hand is holding
    private BulbHolding _leftHolding;
    private BulbHolding _rightHolding;
    private Collider _collider;
    private bool _noAir = false; // Used in conjunction with Suction and Air
    private bool beingHeld = false;

    private void Start() {
        _leftHolding = _rightHolding = BulbHolding.Nothing;
        grabbableObject = GetComponent<UxrGrabbableObject>();
        _collider = GetComponent<Collider>();
    }
    private void OnEnable() {

        // Holding Grab Points
        UxrGrabManager.Instance.ObjectGrabbing += GrabHold;
        UxrGrabManager.Instance.ObjectReleasing += GrabRelease;

        // Button interaction
        UxrControllerInput.GlobalButtonStateChanged += TriggerPressed;
    }

    private void OnDisable() {
        UxrGrabManager.Instance.ObjectGrabbing -= GrabHold;
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

    // When a button is pressed, this should determine if the button is a Trigger (front button of controller).
    // 1. Check what hand the trigger is being pulled and if it is holding something
    
    private void TriggerPressed(object sender, UxrInputButtonEventArgs e) {

        // Checks if the button being held is the trigger.
        if (e.Button != UxrInputButtons.Trigger || e.ButtonEventType != UxrButtonEventType.Pressing) return;

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

        // Hide Base Bulb
    }

    private void PressingSuction() {
        if (_noAir) {
            // Check if collider is colliding with a liquid gameobject and get the colour, empty it out and fill the pipette with the colour
        }
    }
    private void PressingEmpty() {
        _noAir = false;

        // Show Base Bulb

        // Check if it collides with a colourless liquid gameobject and cause it to change the opacity and colour when it is.
        // Otherwise, if it is not colliding with anything then could possibly not do anything or do a PS and empty the liquid from the pipette.
    }
 }


