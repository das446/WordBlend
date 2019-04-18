using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    [SerializeField] InputMode curMode;

    [SerializeField] RotateMode rotateMode;
    [SerializeField] SubmitMode submitMode;
    [SerializeField] Board board;

    void Start() {
        curMode = rotateMode;
        Tile.OnClick += OnTileClick;
        Tile.OnHover +=OnTileHover;

    }

    private void OnTileHover(Tile tile)
    {
        curMode.OnHover(tile);
    }

    private void OnTileClick(Tile tile) {
        curMode.OnClick(tile);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = GetMousePos();
            curMode.OnClick(mousePos);

        } else if (Input.GetKeyDown(KeyCode.Space) && curMode.CanExit()) {
            SwitchMode();
        }
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