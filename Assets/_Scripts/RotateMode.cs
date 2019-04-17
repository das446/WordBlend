using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMode : MonoBehaviour, InputMode
{
    [SerializeField] GameObject ring;
    [SerializeField] Board board;
    [SerializeField] bool active;
    [SerializeField] float rotSpeed;
    void Start() { }

    void Update()
    {
        if (!active) { return; }

        MoveCircle();

    }

    private void MoveCircle()
    {
        Vector2 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        float x = Mathf.Round(mouse.x) + 0.5f;
        float y = Mathf.Round(mouse.y) + 0.5f;

        if (x > board.width - 1.5f)
        {
            x = board.width - 1.5f;
        }
        else if (x < 0.5f)
        {
            x = 0.5f;
        }

        if (y > board.height - 1.5f)
        {
            y = board.height - 1.5f;
        }
        else if (y < 0.5f)
        {
            y = 0.5f;
        }

        Vector2 mouseRounded = new Vector2(x, y);

        ring.transform.position = mouseRounded;
    }

    public void OnClick()
    {

    }

    public void OnClick(Vector2 v)
    {

    }

    public void OnClick(Tile t)
    {
        if (t.pos.x >= board.width - 1)
        {
            t = board.Get(t.pos, -1, 0);
            Debug.Log("Changed x");
        }
        if (t.pos.y >= board.height - 1)
        {
            t = board.Get(t.pos, 0, -1);
            Debug.Log("Changed y");
        }

        Tile nw = board.Get(t.pos, 0, 1);
        Tile ne = board.Get(t.pos, 1, 1);
        Tile se = board.Get(t.pos, 1, 0);



        StartCoroutine(RotatePieces(t, nw, ne, se));
        
    }

    public IEnumerator RotatePieces(Tile sw, Tile nw, Tile ne, Tile se)
    {

        Vector2 swTarget = nw.transform.position;
        Vector2 nwTarget = ne.transform.position;
        Vector2 neTarget = se.transform.position;
        Vector2 seTarget = sw.transform.position;
        StartCoroutine(MovePiece(sw, swTarget));
        StartCoroutine(MovePiece(nw, nwTarget));
        StartCoroutine(MovePiece(ne, neTarget));
        StartCoroutine(MovePiece(se, seTarget));
        

        yield return new WaitForSeconds(1);
        board.RotateClockwise(sw.transform.position.ToVector2Int());

    }

    public IEnumerator MovePiece(Tile t, Vector2 target)
    {
        while (Vector3.Distance(t.transform.position, target) > 0.0001f)
        {
            t.transform.position = Vector3.Lerp(t.transform.position, target, Time.deltaTime * rotSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Enter()
    {
        active = true;
        ring.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        active = false;
        ring.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnClick(Tile t, Board board)
    {
        throw new System.NotImplementedException();
    }
}