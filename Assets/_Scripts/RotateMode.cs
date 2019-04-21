using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMode : MonoBehaviour, InputMode {
    [SerializeField] GameObject ring;
    [SerializeField] Board board;
    [SerializeField] bool active;
    [SerializeField] float tileRotSpeed;
    [SerializeField] float gradRotSpeed;
    [SerializeField] GameObject gradient;
    bool clockWise = true;

    bool canRotate = true;

    void Update() {
        if (!active) { return; }
        if (Input.GetKeyDown(KeyCode.R)) {
            clockWise = !clockWise;
        }

        RotateGrad();

        MoveCircle();

    }

    private void RotateGrad() {
        int c = clockWise? - 1 : 1;
        gradient.transform.Rotate(0, 0, gradRotSpeed * c * Time.deltaTime);
    }

    private void MoveCircle() {
        Vector2 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        float x = Mathf.Round(mouse.x) + 0.5f;
        float y = Mathf.Round(mouse.y) + 0.5f;

        if (x > board.width - 1.5f) {
            x = board.width - 1.5f;
        } else if (x < 0.5f) {
            x = 0.5f;
        }

        if (y > board.height - 1.5f) {
            y = board.height - 1.5f;
        } else if (y < 0.5f) {
            y = 0.5f;
        }

        Vector2 mouseRounded = new Vector2(x, y);

        ring.transform.position = mouseRounded;
    }

    public void OnClick() {

    }

    public void OnClick(Vector2 v) {

    }

    public void OnClick(Tile t) {

        if (!canRotate) { return; }

        if (t.pos.x >= board.width - 1) {
            t = board.Get(t.pos, -1, 0);
            Debug.Log("Changed x");
        }
        if (t.pos.y >= board.height - 1) {
            t = board.Get(t.pos, 0, -1);
            Debug.Log("Changed y");
        }

        Rotate(t);

    }

    private void RotateCounterClockwise(Tile t) {
        throw new NotImplementedException();
    }

    private void Rotate(Tile t) {
        Tile nw = board.Get(t.pos, 0, 1);
        Tile ne = board.Get(t.pos, 1, 1);
        Tile se = board.Get(t.pos, 1, 0);
        if (t.moveable && nw.moveable && ne.moveable && se.moveable) {
            RotatePieces(t, nw, ne, se);
        }
    }

    private void RotatePieces(Tile sw, Tile nw, Tile ne, Tile se) {
        if (clockWise) {
            RotatePiecesClockwise(sw, nw, ne, se);
        } else {
            RotatePiecesCounterClockwise(sw, nw, ne, se);
        }
    }

    public void RotatePiecesClockwise(Tile sw, Tile nw, Tile ne, Tile se) {

        canRotate = false;

        Vector2 swTarget = nw.transform.position;
        Vector2 nwTarget = ne.transform.position;
        Vector2 neTarget = se.transform.position;
        Vector2 seTarget = sw.transform.position;
        board.Rotate(sw.pos, clockWise);
        StartCoroutine(MovePiece(sw, swTarget));
        StartCoroutine(MovePiece(nw, nwTarget));
        StartCoroutine(MovePiece(ne, neTarget));
        StartCoroutine(MovePiece(se, seTarget));

        //yield return new WaitUntil(() => canRotate);

    }

    public void RotatePiecesCounterClockwise(Tile sw, Tile nw, Tile ne, Tile se) {

        canRotate = false;

        Vector2 swTarget = se.transform.position;
        Vector2 nwTarget = sw.transform.position;
        Vector2 neTarget = nw.transform.position;
        Vector2 seTarget = ne.transform.position;
        board.Rotate(sw.pos, clockWise);
        StartCoroutine(MovePiece(sw, swTarget));
        StartCoroutine(MovePiece(nw, nwTarget));
        StartCoroutine(MovePiece(ne, neTarget));
        StartCoroutine(MovePiece(se, seTarget));

        //yield return new WaitUntil(() => canRotate);

    }

    public IEnumerator MovePiece(Tile t, Vector2 target) {
        while (Vector3.Distance(t.transform.position, target) > 0.001f) {
            t.transform.position = Vector3.Lerp(t.transform.position, target, Time.deltaTime * tileRotSpeed);
            yield return new WaitForEndOfFrame();
        }
        t.image.sortingOrder = board.height - (int) target.y;
        t.transform.position = target;
        canRotate = true;
    }

    public void Enter() {
        active = true;
        ring.gameObject.SetActive(true);
        gameObject.SetActive(true);
        canRotate = true;
    }

    public void Exit() {
        active = false;
        ring.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool CanExit() {
        return canRotate;
    }

    public void OnHover(Tile t) { }
}