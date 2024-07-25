using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Devices;
using UltimateXR.Avatar;
using UltimateXR.Core;
using UltimateXR.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject pauseMenu;
    public EventSystem eventSystem;
    public UxrLaserPointer laserHand;

    private void Update() {
        if (UxrAvatar.LocalAvatarInput.GetButtonsPressDown(UxrHandSide.Left, UxrInputButtons.Button2)) {
            if (IsPaused) {
                Debug.Log("Resuming Scene");
                Resume();
            } else {
                Debug.Log("Pausing Scene");
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        IsPaused = false;
        // Deselect any selected UI element
        eventSystem.SetSelectedGameObject(null);
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        IsPaused = true;
    }

    public void Restart() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Return() {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        
        #else
        Application.Quit();
        #endif
    }
}
