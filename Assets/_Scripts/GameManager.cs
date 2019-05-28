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
    [SerializeField] SpriteRenderer timerKnob;

    int points;
    [SerializeField] int[] pointVals;
    [SerializeField] Text text;
    Color defaultGrey;

    public static Action DeregisterEvents;

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
        defaultGrey = timerKnob.color;
    }

    void Update() {

        if (curFreezeTime > 0f) {
            curFreezeTime -= Time.deltaTime;
        } else {
            if (timerKnob.color == Color.cyan) {
                timerKnob.color = defaultGrey;
            }
            curFreezeTime = 0f;

            if (currTime > 0f)
                currTime -= Time.deltaTime;
            else {
                currTime = 0f;
            }
        }
        SetTimerBar();
        SetTimerKnob();

        if (Input.GetKey("escape")) { Application.Quit(); }
        if (currTime <= 0) {
            Lose();
        }

    }

    void SetTimerKnob() {
        float t = currTime / maxTime;
        float r = 180 - (180 * t);
        Vector3 v = new Vector3(0, 0, r);
        timerKnob.transform.eulerAngles = v;
    }

    private void SetTimerBar() {
        float fill = currTime / maxTime;
        TimeBar.fillAmount = fill;
    }

    private void Lose() {
        PlayerPrefs.SetInt("Score", points);
        SubmitMode.Submit -= GetPoints;
        FreezePowerUp.FreezeTimer -= FreezeTimer;
        Tile.objectPool = new List<Tile>();
        DeregisterEvents();
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void FreezeTimer(int amnt) {
        timerKnob.color = Color.cyan;
        curFreezeTime = maxFreezeTime;
    }
}