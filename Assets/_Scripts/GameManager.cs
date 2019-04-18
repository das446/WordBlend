using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float maxTime;
    public GameObject TimeBar;
    private float currTime;

    int points;
    [SerializeField] int[] pointVals;
    [SerializeField] Text text;

    private void GetPoints(Word word) {
        points += pointVals[word.Length()];
        text.text = "Score: " + points;
        currTime += (word.Length() * 4);

    }

    void Start() {
        currTime = maxTime;
        SubmitMode.Submit += GetPoints;
    }

    void Update() {
        if (currTime > 0f)
            currTime -= Time.deltaTime;
        else {
            currTime = 0f;
        }
        float scale = currTime / maxTime;
        scale = scale > 1 ? 1 : scale;
        TimeBar.transform.localScale = new Vector3(1f, currTime / maxTime, 1f);
    }
}