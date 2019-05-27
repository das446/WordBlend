using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class InputController : MonoBehaviour {

    [SerializeField] InputMode curMode;

    [SerializeField] RotateMode rotateMode;
    [SerializeField] SubmitMode submitMode;
    [SerializeField] Board board;

    [SerializeField] TapGesture singleTap;
    [SerializeField] TapGesture doubleTap;
    [SerializeField] TransformGesture drag;

    [SerializeField] TapGesture reverse;

    void Start() {
        drag.TransformStarted += OnDrag;
        drag.Transformed += OnDrag;
        drag.TransformCompleted += OnDrag;

        singleTap.Tapped += OnSingleTap;
        doubleTap.Tapped += OnDoubleTap;
        reverse.Tapped += ReverseDirection;
    }

    void OnDisable() {
        drag.TransformStarted -= OnDrag;
        drag.Transformed -= OnDrag;
        drag.TransformCompleted -= OnDrag;

        singleTap.Tapped -= OnSingleTap;
        doubleTap.Tapped -= OnDoubleTap;
        reverse.Tapped -= ReverseDirection;
    }

    private void ReverseDirection(object sender, EventArgs e) {
        rotateMode.Change();
    }

    private void OnSingleTap(object sender, EventArgs e) {
        rotateMode.OnClick(board);
    }

    private void OnDoubleTap(object sender, EventArgs e) {

        submitMode.OnClick(OriginTile());
    }

    private void OnDrag(object sender, EventArgs e) {
        rotateMode.MoveCircle();

    }

    private void OnTileHover(Tile tile) {
        curMode.OnHover(tile);
    }

    private void OnTileClick(Tile tile) {
        curMode.OnClick(tile);
    }

    Tile OriginTile() {
        return rotateMode.OriginTile();
    }

    private void SwitchMode() {
        curMode.Exit();
        if (curMode == rotateMode) {
            curMode = submitMode;
        } else {
            curMode = rotateMode;
        }
        curMode.Enter();
    }

    public static Vector2 GetMousePos() {
        Vector2 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        return mouse;
    }

}