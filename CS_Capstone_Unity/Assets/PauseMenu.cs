using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenu;
    public GameObject playerHud;
    // Update is called once per frame

    void Start() {
        pauseMenu.SetActive(false);
        playerHud.SetActive(true);
    }


    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start)) {
            if(isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    void Pause() {
        pauseMenu.SetActive(true);
        playerHud.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        playerHud.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Resume button pressed");
    }

    public void Settings() {
        Debug.Log("Settings button pressed");
    }

    public void QuitGame() {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
