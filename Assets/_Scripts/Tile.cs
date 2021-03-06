﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Vector2Int pos;
    public Letter letter;
    public SpriteRenderer image;
    [SerializeField] SpriteRenderer lockBackground, iceBackground;
    [SerializeField] TextMeshPro tmp;
    [SerializeField] public bool moveable;
    public PowerUp powerUp;
    public static List<Tile> objectPool = new List<Tile>();
    [SerializeField] float min;
    [SerializeField] List<PowerUp> possiblePowerups;
    [SerializeField] int powerupChance;
    [SerializeField] GameObject particles;
    [SerializeField] float lockChance;

    public Board board;

    public static event Action<Tile> OnClick;
    public static event Action<Tile> OnHover;

    public void SetLetter(Letter l) {
        letter = l;
        image.sprite = l.image;
        //letterDisplay.text = letter.name;
    }

    public void MakeParticles() {
        Destroy(Instantiate(particles, transform.position, transform.rotation), 5);
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
        if (objectPool == null) { objectPool = new List<Tile>(); }
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

        t.transform.rotation = Quaternion.identity;
        t.transform.localScale = new Vector3(1, 1, 1);

        t.RandomAttribute();

        return t;
    }

    public IEnumerator Move(Vector2Int target, float speed) {
        pos = target;
        Vector3 targetPos = new Vector3(target.x, target.y, 0);
        while (Vector3.Distance(transform.position, targetPos) > 0.001f) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        //t.image.sortingOrder = (int) target.y;
        transform.position = targetPos;

    }

    public void DestroyTile() {
        objectPool.Remove(this);
        Destroy(gameObject);
    }

    public void Clear() {
        gameObject.SetActive(false);
    }

    public IEnumerator ReplaceLetter(Letter l, float scaleSpeed, float rotSpeed) {

        float s = transform.localScale.x;
        yield return StartCoroutine(SpinIn(scaleSpeed, rotSpeed));
        SetLetter(l);
        s = 0;

        if (moveable) {
            RandomAttribute();
        } else {
            Unlock();
        }

        StartCoroutine(SpinOut(scaleSpeed, rotSpeed));
    }

    public IEnumerator SpinIn(float scaleSpeed, float rotSpeed) {
        float s = transform.localScale.x;
        while (s > min) {
            s -= Time.deltaTime * scaleSpeed;
            transform.localScale = new Vector3(s, s, 1);
            transform.Rotate(0, 0, -s * rotSpeed);
            yield return new WaitForEndOfFrame();
        }

    }

    public IEnumerator SpinOut(float scaleSpeed, float rotSpeed) {
        float s = 0;
        while (s < 1) {
            s += Time.deltaTime * scaleSpeed;
            transform.localScale = new Vector3(s, s, 1);
            transform.Rotate(0, 0, -s * rotSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1, 1, 1);
        SubmitMode.canExit = true;
    }

    public void RandomAttribute() {
        if (UnityEngine.Random.Range(0, 100) <= lockChance) {
            Lock();
        } else if (UnityEngine.Random.Range(0, 100) <= powerupChance) {
            powerUp = possiblePowerups.RandomItem();
            powerUp.Init(this);
        } else {
            Unlock();
        }
    }

    public new string ToString() {
        return letter.ToString();
    }

    public void Lock() {
        if (!CanLock(board)) { return; }
        moveable = false;
        //background.color = Color.black;
        lockBackground.enabled = true;
        //powerUp = null;
    }

    private bool CanLock(Board board) {
        return true;
        int x = pos.x;
        int y = pos.y;
        if (x == 0 || y == 0) {
            if (!WordChecker.CheckPattern(s =>
                    s[0].ToString() == letter.name
                )) { return false; }
        }
        if (x == board.width - 1 || y == board.height - 1) {
            if (!WordChecker.CheckPattern(s =>
                    s.Last().ToString() == letter.name
                )) { return false; }
        }

        return true;
    }

    public void FreezeTile() {
        moveable = true;
        iceBackground.enabled = true;
        powerUp = possiblePowerups.Where(x => x.name == "freeze").First();
    }

    public void Unlock() {
        moveable = true;
        lockBackground.enabled = false;
        iceBackground.enabled = false;
    }

}