using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Vector2Int pos;
    public Letter letter;
    [SerializeField] SpriteRenderer image;
    [SerializeField] TMP_Text letterDisplay;
    [SerializeField] bool moveable;
    static List<Tile> objectPool = new List<Tile>();

    public static event Action<Tile> OnClick;

    public void SetLetter(Letter l) {
        letter = l;
        image.sprite = l.image;
        letterDisplay.text = letter.name;
    }

    public void SetPos(Vector2Int p) {
        pos = p;
    }
    public void Offset(Vector2Int p) {
        pos = pos + p;
    }
    public void Offset(int x, int y) {
        Offset(new Vector2Int(x, y));
    }

    public void OnMouseDown() {
        if (OnClick != null) {
            OnClick(this);
        }
    }

    public Tile Create(Vector2Int pos, Letter letter) {
        Tile t = objectPool.FirstOrDefault(x => x.gameObject.activeSelf == false);
        Vector3 v = new Vector3(pos.x, pos.y, 0);
        if (t == null) {
            t = (Tile) Instantiate(this);
            objectPool.Add(t);
            t.gameObject.SetActive(true);
        }
        t.gameObject.SetActive(true);
        t.SetPos(pos);
        t.SetLetter(letter);
        return t;

    }

    public void Clear() {
        gameObject.SetActive(false);
    }

}