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
    public GameObject water;
    public float waterMoveTime;
    private bool isWaterMoving;
    private Vector3 newWaterPos, oldWaterPos;
    private float waterLerpStart;
    private float currTime;
    private float curFreezeTime;

    private Image timeBarImage;
    [SerializeField] SpriteRenderer timerKnob;

    int points;
    [SerializeField] int[] pointVals;
    [SerializeField] Text text;
    Color defaultGrey;
    float waterAnimSpeed;

    public static Action DeregisterEvents;

    private void GetPoints(int amnt) {
        points += pointVals[amnt];
        float pointsToAdd = pointVals[amnt];
        text.text = "Score: " + points;
        currTime += amnt * 4;
        if (currTime > maxTime) {
            currTime = maxTime;
        }

        float waterDisplace = (pointsToAdd / 1000.0f);
        // Start Water Lerp 
        if (water.transform.position.y < 2.5) {
            oldWaterPos = water.transform.position;
            newWaterPos = new Vector3(water.transform.position.x, water.transform.position.y + waterDisplace, water.transform.position.z);
            waterLerpStart = Time.time;
            isWaterMoving = true;
        }
    }

    void Start() {
        isWaterMoving = false;
        currTime = maxTime;
        curFreezeTime = 0f;
        SubmitMode.Submit += GetPoints;
        timeBarImage = TimeBar.GetComponent<Image>();
        FreezePowerUp.FreezeTimer += FreezeTimer;
        defaultGrey = timerKnob.color;
        waterAnimSpeed = water.gameObject.GetComponent<Animator>().speed;
    }

    void Update() {

        if (curFreezeTime > 0f) {
            curFreezeTime -= Time.deltaTime;
        } else {
            if (timerKnob.color == Color.cyan) {
                timerKnob.color = defaultGrey;
                Color c = Color.red;
                c.a = 0.5f;
                TimeBar.color = c;
                water.gameObject.GetComponent<Animator>().speed = waterAnimSpeed;
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

        if (isWaterMoving) {
            float timeSinceStart = Time.time - waterLerpStart;
            float lerpPercentTemp = timeSinceStart / waterMoveTime;
            water.transform.position = Vector3.Lerp(oldWaterPos, newWaterPos, lerpPercentTemp);
            if (lerpPercentTemp >= 1.0f)
                isWaterMoving = false;
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
        Color c = Color.cyan;
        c.a = 0.5f;
        TimeBar.color = c;
        curFreezeTime = maxFreezeTime;
        water.gameObject.GetComponent<Animator>().speed = 0;
    }
}