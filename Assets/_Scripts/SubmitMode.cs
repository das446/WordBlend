﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubmitMode : MonoBehaviour, InputMode {

    List<Tile> selectedTiles = new List<Tile>();

    [SerializeField] GameObject submitButton;

    List<GameObject> ringPool = new List<GameObject>();
    [SerializeField] GameObject ring;

    public static event Action<Word> Submit;
    [SerializeField] Board board;
    [SerializeField] float scaleSpeed;
    [SerializeField] float rotSpeed;

    public void Enter() {
        //submitButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Exit() {
        ClearRings();
        selectedTiles = new List<Tile>();
        //submitButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnClick() {

    }

    public void OnClick(Vector2 v) {

    }

    public void OnClick(Tile t) {
        Word word = GetLongestWord(t);

        if (word != null) {
            SubmitWord(word);
        }
    }

    private Word GetLongestWord(Tile t) {
        bool valid = false;

        int x = t.pos.x;
        List<Tile> tiles = new List<Tile>();

        while (x < board.width) {
            tiles.Add(board.Get(x, t.pos.y));
            x++;
        }

        Word word = new Word(tiles);

        while (word.Length() > 2 && !valid) {
            valid = WordChecker.CheckWord(word);
            if (!valid) {
                word = word.RemoveLast();
            }
        }

        if (valid) {
            return word;
        } else {
            return null;
        }
    }

    private GameObject MakeRing(Vector3 pos) {
        GameObject r = ringPool.FirstOrDefault(x => x.gameObject.activeSelf == false);
        Vector3 v = new Vector3(pos.x, pos.y, 0);
        if (r == null) {
            r = Instantiate(ring);
            ringPool.Add(r);
            r.SetActive(true);
        }
        r.gameObject.SetActive(true);
        r.transform.position = pos;
        return r;

    }

    public string CurWord() {
        string s = "";
        for (int i = 0; i < selectedTiles.Count; i++) {
            s = s + selectedTiles[i].letter.ToString.ToLower();
        }
        return s;
    }

    public void SubmitWord(Word word) {
        foreach (Tile tile in word.letters) {
            Letter l = board.RandomLetter();
            if (board.CurrentVowels() < 4) {
                l = board.RandomVowel();
            }
            StartCoroutine(tile.ChangeLetter(l, scaleSpeed, rotSpeed));
        }
        Submit(word);
    }

    public bool CanExit() {
        return true;
    }

    public void OnHover(Tile t) {
        Word word = GetLongestWord(t);
        ClearRings();
        if (word != null && t.moveable) {
            foreach (Tile tile in word.letters) {
                MakeRing(tile.transform.position);
            }
        }
    }

    private void ClearRings() {
        foreach (GameObject g in ringPool) {
            g.SetActive(false);
        }
    }
}