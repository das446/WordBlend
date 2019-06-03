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

    public static event Action FinishRotate;

    void Update() {
        if (!active) { return; }

        RotateGrad();

        //MoveCircle();

    }

    private void RotateGrad() {
        int c = clockWise? - 1 : 1;
        gradient.transform.Rotate(0, 0, gradRotSpeed * c * Time.deltaTime);
    }

    public void MoveCircle() {
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
        float vx = ring.transform.position.x;
        float vy = ring.transform.position.y;
        if (vx != x || vy != y) {
            Audio.PlaySound("Move", volume : 0.3f);
        }
        ring.transform.position = mouseRounded;

    }

    public void OnClick(Board b) {
        Tile t = OriginTile();
        OnClick(t);
    }

    public Tile OriginTile() {
        int x = (int) (transform.position.x - 0.5f);
        int y = (int) (transform.position.y - 0.5f);
        Tile t = board.Get(x, y);
        return t;
    }

    public void OnClick() {

    }

    public void OnClick(Vector2 v) {

    }

    public void OnClick(Tile t) {

        if (!canRotate) { return; }

        if (t.pos.x >= board.width - 1) {
            t = board.Get(t.pos, -1, 0);
        }
        if (t.pos.y >= board.height - 1) {
            t = board.Get(t.pos, 0, -1);
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
            Audio.PlaySound("rotate", volume : 1);
            RotatePieces(t, nw, ne, se);
        } else {
            Audio.PlaySound("locked");
        }
    }

    private void RotatePieces(Tile sw, Tile nw, Tile ne, Tile se) {
        if (clockWise) {
            StartCoroutine(RotatePiecesClockwise(sw, nw, ne, se));
        } else {
            StartCoroutine(RotatePiecesCounterClockwise(sw, nw, ne, se));
        }
    }

    public IEnumerator RotatePiecesClockwise(Tile sw, Tile nw, Tile ne, Tile se) {

        canRotate = false;

        Vector2Int swTarget = nw.pos;
        Vector2Int nwTarget = ne.pos;
        Vector2Int neTarget = se.pos;
        Vector2Int seTarget = sw.pos;
        StartCoroutine(sw.Move(swTarget, tileRotSpeed));
        StartCoroutine(nw.Move(nwTarget, tileRotSpeed));
        StartCoroutine(ne.Move(neTarget, tileRotSpeed));
        yield return (StartCoroutine(se.Move(seTarget, tileRotSpeed)));

        canRotate = true;
        FinishRotate();

    }

    public IEnumerator RotatePiecesCounterClockwise(Tile sw, Tile nw, Tile ne, Tile se) {

        canRotate = false;

        Vector2Int swTarget = se.pos;
        Vector2Int nwTarget = sw.pos;
        Vector2Int neTarget = nw.pos;
        Vector2Int seTarget = ne.pos;
        StartCoroutine(sw.Move(swTarget, tileRotSpeed));
        StartCoroutine(nw.Move(nwTarget, tileRotSpeed));
        StartCoroutine(ne.Move(neTarget, tileRotSpeed));
        yield return (StartCoroutine(se.Move(seTarget, tileRotSpeed)));

        canRotate = true;

        FinishRotate();

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

    public void Change() {
        clockWise = !clockWise;
    }
}