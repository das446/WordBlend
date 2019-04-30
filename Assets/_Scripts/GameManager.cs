using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float maxTime;
    public GameObject TimeBar;
    public float maxFreezeTime;
    public int lockedTileCount;
    private float currTime;
    private float curFreezeTime;
    private Image timeBarImage;
    int points;
    [SerializeField] int[] pointVals;
    [SerializeField] Text text;

    private void GetPoints(Word word) {
        points += pointVals[word.Length()];
        text.text = "Score: " + points;
        currTime += word.Length() * 4;
        if (currTime > maxTime) {
            currTime = maxTime;
        }
    }

    void Start() {
        currTime = maxTime;
        curFreezeTime = 0f;
        SubmitMode.Submit += GetPoints;
        timeBarImage = TimeBar.GetComponent<Image>();
        FreezePowerUp.FreezeTimer+=FreezeTimer;
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
        float scale = currTime / maxTime;
        scale = scale > 1 ? 1 : scale;
        TimeBar.transform.localScale = new Vector3(1f, currTime / maxTime, 1f);

        if (Input.GetKey("escape"))
            Application.Quit(); 
    }

    public void FreezeTimer(int amnt) {
        timeBarImage.color = Color.cyan;
        curFreezeTime = maxFreezeTime;
    }
}