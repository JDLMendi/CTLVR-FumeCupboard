using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartBtn() {
        SceneManager.LoadScene("Main Scene");
    }

    public void StartScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitBtn() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
