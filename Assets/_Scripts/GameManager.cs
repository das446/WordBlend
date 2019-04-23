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
    private float currFreezeTime;
    private Image timeBarImage;
    int points;
    [SerializeField] int[] pointVals;
    [SerializeField] Text text;

    private void GetPoints(Word word) {
        points += pointVals[word.Length()];
        text.text = "Score: " + points;

        if (currTime + word.Length() * 4 > maxTime)
            currTime = maxTime;
        else
            currTime += (word.Length() * 4);
    }

    void Start() {
        currTime = maxTime;
        currFreezeTime = 0f;
        SubmitMode.Submit += GetPoints;
        timeBarImage = TimeBar.GetComponent<Image>();
    }

    void Update() {

        if (currFreezeTime > 0f) {
            currFreezeTime -= Time.deltaTime;
        }
        else {
            if (timeBarImage.color == Color.cyan)
                timeBarImage.color = Color.red;
            currFreezeTime = 0f;

            if (currTime > 0f)
                currTime -= Time.deltaTime;
            else {
                currTime = 0f;
            }
        }
        float scale = currTime / maxTime;
        scale = scale > 1 ? 1 : scale;
        TimeBar.transform.localScale = new Vector3(1f, currTime / maxTime, 1f);
    }

    public void freezeTime() {
        timeBarImage.color = Color.cyan;
        currFreezeTime = maxFreezeTime;
    }
}