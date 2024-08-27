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
    void Update() {
        if(UxrGrabManager.Instance.GetObjectBeingGrabbed(UxrAvatar.LocalAvatar, UxrHandSide.Left, out UxrGrabbableObject grabbableObject)) {
            if(UxrAvatar.LocalAvatarInput.GetButtonsPress(UxrHandSide.Left, UxrInputButtons.Trigger)) {
                grabbableObject.transform.Find("PipetteLiquid").gameObject.GetComponent<PipetteFill>().pourOut();
            }
        }

        if (UxrGrabManager.Instance.GetObjectBeingGrabbed(UxrAvatar.LocalAvatar, UxrHandSide.Right, out UxrGrabbableObject grabbableObject2)) {
            if (UxrAvatar.LocalAvatarInput.GetButtonsPress(UxrHandSide.Right, UxrInputButtons.Trigger)) {
                grabbableObject2.transform.Find("PipetteLiquid").gameObject.GetComponent<PipetteFill>().pourOut();
            }
        }
    }
}
