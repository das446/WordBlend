using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Vector2Int pos;
    public Letter letter;
    public SpriteRenderer image;
    [SerializeField] SpriteRenderer background;
    [SerializeField] TextMeshPro tmp;
    [SerializeField] public bool moveable;
    public PowerUp powerUp;
    static List<Tile> objectPool = new List<Tile>();
    [SerializeField] float min;
    [SerializeField] List<PowerUp> possiblePowerups;
    [SerializeField] int powerupChance;

    public static event Action<Tile> OnClick;
    public static event Action<Tile> OnHover;

    public void SetLetter(Letter l) {
        letter = l;
        image.sprite = l.image;
        //letterDisplay.text = letter.name;
    }

    public void SetPos(Vector2Int p) {
        pos = p;
        transform.position = new Vector2(p.x, p.y);
    }
    public void Offset(Vector2Int p) {
        pos = pos + p;
        //transform.position = new Vector2(pos.x, pos.y);
    }
    public void Offset(int x, int y) {
        Offset(new Vector2Int(x, y));
    }

    public void OnMouseDown() {
        if (OnClick != null) {
            OnClick(this);
        }
    }

    public void OnMouseOver() {
        if (OnHover != null) { OnHover(this); }
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
        t.powerUp = null;
        return t;

    }

    public void Clear() {
        gameObject.SetActive(false);
    }

    public IEnumerator ChangeLetter(Letter l, float scaleSpeed, float rotSpeed) {
        float s = transform.localScale.x;
        float x = s;
        while (s > min) {
            s -= Time.deltaTime * scaleSpeed;
            transform.localScale = new Vector3(s, s, 1);
            transform.Rotate(0, 0, -s * rotSpeed);
            yield return new WaitForEndOfFrame();
        }
        SetLetter(l);
        if (moveable) {
            RandomAttribute();
        } else {
            Unlock();
        }
        while (s < x) {
            s += Time.deltaTime * scaleSpeed;
            transform.localScale = new Vector3(s, s, 1);
            transform.Rotate(0, 0, -s * rotSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(x, x, 1);
        SubmitMode.canExit = true;
    }

    public void RandomAttribute() {
        if (UnityEngine.Random.Range(0, 10) == 0) {
            Lock();
        } else if (UnityEngine.Random.Range(0, 100) <= powerupChance) {
            powerUp = possiblePowerups.RandomItem();
            powerUp.Init(this);
            Debug.Log(powerUp);
        } else {
            Unlock();
        }
    }

    public new string ToString() {
        return letter.ToString();
    }

    public void Lock() {
        moveable = false;
        background.color = Color.black;
        background.enabled = true;
        powerUp = null;
    }

    public void FreezeTile() {
        moveable = true;
        background.color = Color.cyan;
        background.enabled = true;
        powerUp = possiblePowerups.Where(x => x.name == "freeze").First();
    }

    public void Unlock() {
        moveable = true;
        background.color = Color.white;
        background.enabled = false;
    }

}