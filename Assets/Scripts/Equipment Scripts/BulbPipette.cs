using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UltimateXR.Core;
using UltimateXR.Manipulation;

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

    private void Start() {
        _leftHolding = _rightHolding = BulbHolding.Nothing;
        grabbableObject = GetComponent<UxrGrabbableObject>();
        _collider = GetComponent<Collider>();
    }
    private void OnEnable() {
        UxrGrabManager.Instance.ObjectGrabbing += GrabHold;
        UxrGrabManager.Instance.ObjectReleasing += GrabRelease;
    }

    private void OnDisable() {
        UxrGrabManager.Instance.ObjectGrabbing -= GrabHold;
        UxrGrabManager.Instance.ObjectReleasing -= GrabRelease;
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

        if (!e.IsSwitchHands) {
            _collider.isTrigger = false;
        } 
        _collider.isTrigger = false;
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
    }
}
        

