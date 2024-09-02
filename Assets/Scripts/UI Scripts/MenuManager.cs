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

    private void OnEnable() {
        // Button interaction
        UxrControllerInput.GlobalButtonStateChanged += MenuButtonPressed;
    }

    private void OnDisable() {
        UxrControllerInput.GlobalButtonStateChanged -= MenuButtonPressed;
    }

    private void MenuButtonPressed(object sender, UxrInputButtonEventArgs e) {
        if (e.Button != UxrInputButtons.Button1 || e.ButtonEventType != UxrButtonEventType.PressDown || e.HandSide != UxrHandSide.Left) return;
        
        switch (IsPaused) {
            case true:
                Debug.Log("Resuming Scene");
                Resume();
                break;
            case false:
                Debug.Log("Pausing Scene");
                Pause();
                break;
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
