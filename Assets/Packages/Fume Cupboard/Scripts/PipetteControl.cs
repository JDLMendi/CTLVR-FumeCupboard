using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Devices;
using UltimateXR.Avatar;
using UltimateXR.Core;
using UltimateXR.UI;
using UltimateXR.Manipulation;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PipetteControl : MonoBehaviour
{
    public static bool releasePipette = false;
    GameObject pipetteLiquid;

    void Start() {
        pipetteLiquid = GameObject.Find("PipetteLiquid");
    }

    void Update() {
        if(UxrGrabManager.Instance.GetObjectBeingGrabbed(UxrAvatar.LocalAvatar, UxrHandSide.Left, out UxrGrabbableObject grabbableObject)) {
            if(UxrAvatar.LocalAvatarInput.GetButtonsPress(UxrHandSide.Left, UxrInputButtons.Trigger)) {
                pipetteLiquid.GetComponent<PipetteFill>().pourOut();
            }
        }

        if (UxrGrabManager.Instance.GetObjectBeingGrabbed(UxrAvatar.LocalAvatar, UxrHandSide.Right, out UxrGrabbableObject grabbableObject2))
        {
            if (UxrAvatar.LocalAvatarInput.GetButtonsPress(UxrHandSide.Right, UxrInputButtons.Trigger))
            {
                pipetteLiquid.GetComponent<PipetteFill>().pourOut();
            }
        }
    }
}
