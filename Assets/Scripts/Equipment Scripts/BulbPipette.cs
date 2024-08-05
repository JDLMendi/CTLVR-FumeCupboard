using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UltimateXR;
using UltimateXR.Manipulation;

public class BulbPipette : MonoBehaviour
{
    // Determine which of the three grab point is being held.

    [SerializeField] bool _holdingAir; // 1
    [SerializeField] bool _holdingEmpty; // 3 
    [SerializeField] bool _holdingSuction;// 2
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

        bool anyGrabberActive = false;

        // Iterate over each grabber in the array
        foreach (UxrGrabber grabber in _grabbers)
        {
            int grabIndex = UxrGrabManager.Instance.GetGrabbedPoint(grabber);
            // Debug.Log(grabIndex); // GetGrabbedPoint() <- Gets the active grab point

            /** 
                Grab Point Index
                0 - Shaft
                1 - Bulb/Air
                2 - Suction
                3 - Exit
                -1 - Nothing
            **/

            switch (grabIndex)
            {
                case 1:
                    _holdingAir = true;
                    Debug.Log("Holding Air");
                    break;
                case 2:
                    _holdingSuction = true;
                    Debug.Log("Holding Suction");
                    break;
                case 3:
                    _holdingEmpty = true;
                    Debug.Log("Holding Empty");
                    break;
                default:
                    // If the grabIndex is not -1 or 0, mark anyGrabberActive as true
                    if (grabIndex != -1 && grabIndex != 0)
                    {
                        anyGrabberActive = true;
                    }
                    break;
            }
        }

        // If no grabber has an active grab point, set all holding parameters to false
        if (!anyGrabberActive)
        {
            _holdingAir = _holdingSuction = _holdingEmpty = false;
        }
    }
}
        

