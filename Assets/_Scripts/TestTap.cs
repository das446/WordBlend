using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class TestTap : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GetComponent<TransformGesture>().Transformed += tappedHandler;
    }

    private void tappedHandler(object sender, EventArgs e) {
        float x = Mathf.Round(transform.position.x);
        float y = Mathf.Round(transform.position.y);
        transform.position = new Vector2(x, y);

    }

    // Update is called once per frame
    void Update() {

    }
}