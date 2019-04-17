using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMode : MonoBehaviour, InputMode {
    [SerializeField] GameObject ring;
    [SerializeField] Board board;
    [SerializeField] bool active;

    void Start() { }

    void Update() {
        if (!active) { return; }

        MoveCircle();

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

    public void OnClick(Vector2 v) { }

    public void OnClick(Tile t, Board board) {
        int originx = (int) (transform.position.x - 0.5f);
        int originy = (int) (transform.position.y - 0.5f);

        board.RotateClockwise(new Vector2Int(originx, originy));
    }

    public void Enter() {
        active = true;
        ring.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Exit() {
        active = false;
        ring.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}