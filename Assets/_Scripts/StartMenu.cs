using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {
    public void StartGame(string mode) {
        PlayerPrefs.SetString("Mode", mode);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

     public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GoToHelp() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Help");
    }

    public void GoToMain() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Exit() {
        Application.Quit();
    }

}