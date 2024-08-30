using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Avatar;
using UltimateXR.Haptics;
using UltimateXR.Manipulation;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ThermometerControl : MonoBehaviour
{
    private ThermometerFill thermometerLiquid;

    void Start()
    {
        thermometerLiquid = transform.Find("ThermometerInner").gameObject.GetComponent<ThermometerFill>();
    }

    void Update()
    {
        /*
        if (UxrGrabManager.Instance.GetObjectBeingGrabbed(UxrAvatar.LocalAvatar, UxrHandSide.Left, out UxrGrabbableObject grabbableObject))
        {
            if (UxrAvatar.LocalAvatarInput.GetButtonsPress(UxrHandSide.Left, UxrInputButtons.Trigger))
            {
                grabbableObject.transform.Find("ThermometerInner").gameObject.GetComponent<ThermometerFill>().increaseTemp();
            }
        }

        if (UxrGrabManager.Instance.GetObjectBeingGrabbed(UxrAvatar.LocalAvatar, UxrHandSide.Right, out UxrGrabbableObject grabbableObject2))
        {
            if (UxrAvatar.LocalAvatarInput.GetButtonsPress(UxrHandSide.Right, UxrInputButtons.Trigger))
            {
                grabbableObject2.transform.Find("ThermometerInner").gameObject.GetComponent<ThermometerFill>().decreaseTemp();
            }
        }
        */
    }

    void OnTriggerEnter(Collider other) {
        thermometerLiquid.SetInsideLiquid(true);
    }

    void OnTriggerExit(Collider other) {
        thermometerLiquid.SetInsideLiquid(false);
    }
}
