using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UltimateXR;
using UltimateXR.Manipulation;

public class BulbPipette : MonoBehaviour
{
    // Determine which of the three grab point is being held.

    private bool _holdingAir; // 1
    private bool _holdingEmpty; // 3 
    private bool _holdingSuction;// 2
    private bool _noAir = false;
    public UxrGrabber grabberLeft;
    public UxrGrabber grabberRight;

    private UxrGrabber[] _grabbers;

    private void Start() {
        GameObject grabberLeft = GameObject.Find("GrabberLeft");
        GameObject grabbetRight = GameObject.Find("GrabberRight");

        if (grabberLeft != null && grabberRight != null) {
            _grabbers = new UxrGrabber[2];
            _grabbers[0] = grabberLeft.GetComponent<UxrGrabber>();
            _grabbers[1] = grabberRight.GetComponent<UxrGrabber>();
            Debug.Log($"There are {_grabbers.Length} grabbers in the array");
        } else {
            Debug.LogWarning("Missing Grabbers!");
        }
    }

    
    private void Update() {

        foreach (UxrGrabber grabber in _grabbers) {
            Debug.Log(UxrGrabManager.Instance.GetGrabbedPoint(grabber)); // GetGrabbedPoint() <- Gets the active grab point

            /** 
                Grab Point Index
                0 - Shaft
                1 - Bulb/Air
                2 - Suction
                3 - Exit
            **/
        }
        
    }
}
