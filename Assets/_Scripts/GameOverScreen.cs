using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    [SerializeField] Text scoreText;
    void Start() {
        int score = PlayerPrefs.GetInt("Score");
        scoreText.text = "Game Over.\nYour Score: " + score;
    }

    public void PlayAgain() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void ToMain() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}