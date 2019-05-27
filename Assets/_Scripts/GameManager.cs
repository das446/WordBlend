using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float maxTime;
    public Image TimeBar;
    public float maxFreezeTime;
    public int lockedTileCount;
    private float currTime;
    private float curFreezeTime;
    private Image timeBarImage;
    int points;
    [SerializeField] int[] pointVals;
    [SerializeField] Text text;

    private void GetPoints(int amnt) {
        points += pointVals[amnt];
        text.text = "Score: " + points;
        currTime += amnt * 4;
        if (currTime > maxTime) {
            currTime = maxTime;
        }
    }

    void Start() {
        currTime = maxTime;
        curFreezeTime = 0f;
        SubmitMode.Submit += GetPoints;
        timeBarImage = TimeBar.GetComponent<Image>();
        FreezePowerUp.FreezeTimer += FreezeTimer;
    }

    void Update() {

        if (curFreezeTime > 0f) {
            curFreezeTime -= Time.deltaTime;
        } else {
            if (timeBarImage.color == Color.cyan) {
                timeBarImage.color = Color.red;
            }
            curFreezeTime = 0f;

            if (currTime > 0f)
                currTime -= Time.deltaTime;
            else {
                currTime = 0f;
            }
        }
        float fill = currTime / maxTime;
        TimeBar.fillAmount = fill;

        if (Input.GetKey("escape")) { Application.Quit(); }
        if (currTime <= 0) {
            Lose();
        }

    }

    private void Lose() {
        PlayerPrefs.SetInt("Score", points);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void FreezeTimer(int amnt) {
        timeBarImage.color = Color.cyan;
        curFreezeTime = maxFreezeTime;
    }
}