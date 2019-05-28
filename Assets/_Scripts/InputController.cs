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

        GameManager.DeregisterEvents += DeregisterEvents;
    }

    void DeregisterEvents() {
        drag.TransformStarted -= OnDrag;
        drag.Transformed -= OnDrag;
        drag.TransformCompleted -= OnStopDrag;

        singleTap.Tapped -= OnSingleTap;
        doubleTap.Tapped -= OnDoubleTap;
        reverse.Tapped -= ReverseDirection;
        GameManager.DeregisterEvents += DeregisterEvents;

    }

    void Update() {
        if (submitPreview) {
            submitPreviewTimer -= Time.deltaTime;
            if (submitPreviewTimer <= 0) {
                submitMode.PreviewSubmit(OriginTile());
            }

        }
    }

    private void ReverseDirection(object sender, EventArgs e) {
        rotateMode.Change();
    }

    private void OnSingleTap(object sender, EventArgs e) {
        submitMode.ClearRings();
        rotateMode.OnClick(board);
    }

    private void OnDoubleTap(object sender, EventArgs e) {

        submitPreview = false;
        submitPreviewTimer = 1;
        submitMode.ClearRings();
        submitMode.OnClick(OriginTile());

    }

    private void OnStartDrag(object sender, EventArgs e) {

        submitPreview = false;
        submitPreviewTimer = 1;
        submitMode.ClearRings();
    }

    private void OnDrag(object sender, EventArgs e) {

        rotateMode.MoveCircle();
    }

    private void OnStopDrag(object sender, EventArgs e) {
        submitPreview = true;
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

    void ShowSubmitPreview() {

    }

}