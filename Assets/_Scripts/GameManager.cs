using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxTime;
    public GameObject TimeBar;
    private float currTime;

    // Start is called before the first frame update
    void Start()
    {
        currTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime > 0f)
            currTime -= Time.deltaTime;
        else
        {
            currTime = 0f;
        }

        TimeBar.transform.localScale = new Vector3(1f, currTime/maxTime, 1f);
    }
}
