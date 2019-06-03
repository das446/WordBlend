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

    float submitPreviewTimer = 1;
    bool submitPreview = false;

    void Start() {
        drag.TransformStarted += OnStartDrag;
        drag.Transformed += OnDrag;
        drag.TransformCompleted += OnStopDrag;

        singleTap.Tapped += OnSingleTap;
        doubleTap.Tapped += OnDoubleTap;
        reverse.Tapped += ReverseDirection;

        RotateMode.FinishRotate += ShowSubmitPreview;
        SubmitMode.Submit += ClearSubmitPreview;

        GameManager.DeregisterEvents += DeregisterEvents;
    }

    void DeregisterEvents() {
        drag.TransformStarted -= OnStartDrag;
        drag.Transformed -= OnDrag;
        drag.TransformCompleted -= OnStopDrag;

        singleTap.Tapped -= OnSingleTap;
        doubleTap.Tapped -= OnDoubleTap;
        reverse.Tapped -= ReverseDirection;

        RotateMode.FinishRotate -= ShowSubmitPreview;
        //SubmitMode.Drop -= ShowSubmitPreview;

        GameManager.DeregisterEvents -= DeregisterEvents;

    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(WordChecker.CheckWord("penv"));
        }
    }

    private void ReverseDirection(object sender, EventArgs e) {
        Debug.Log("Reverse");
        rotateMode.Change();
    }

    private void OnSingleTap(object sender, EventArgs e) {
        submitMode.ClearRings();
        rotateMode.OnClick(board);
    }

    private void OnDoubleTap(object sender, EventArgs e) {
        ClearSubmitPreview();
        submitMode.OnClick(OriginTile());

    }

    private void ClearSubmitPreview() {
        submitPreview = false;
        submitPreviewTimer = 1;
        submitMode.ClearRings();
    }

    private void ClearSubmitPreview(int i) {
        ClearSubmitPreview();
    }

    private void OnStartDrag(object sender, EventArgs e) {

        ClearSubmitPreview();
    }

    private void OnDrag(object sender, EventArgs e) {

        rotateMode.MoveCircle();
    }

    private void OnStopDrag(object sender, EventArgs e) {
        submitMode.PreviewSubmit(OriginTile());
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

    public void ShowSubmitPreview() {
        submitMode.PreviewSubmit(rotateMode.OriginTile());
    }

}