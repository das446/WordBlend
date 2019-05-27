using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubmitMode : MonoBehaviour, InputMode {

    List<Tile> selectedTiles = new List<Tile>();

    [SerializeField] GameObject submitButton;

    List<GameObject> ringPool = new List<GameObject>();
    [SerializeField] GameObject ring;

    public static event Action<int> Submit;
    [SerializeField] Board board;
    [SerializeField] GameManager gameManager;
    [SerializeField] float scaleSpeed;
    [SerializeField] float rotSpeed;
    bool horizontal = true;

    public GameObject horizontalArrows;
    public GameObject verticalArrows;

    public static bool canExit = true;

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
        List<Tile> validWordsTiles = new List<Tile>();
        Tile sw = t;
        Tile nw = board.Get(t.pos.x, t.pos.y + 1);
        Tile ne = board.Get(t.pos.x + 1, t.pos.y + 1);
        Tile se = board.Get(t.pos.x + 1, t.pos.y);
        List<Tile> checkTiles = new List<Tile>() { sw, nw, ne, se };
        for (int i = 0; i < 4; i++) {
            Word word = GetLongestWordHorizontal(checkTiles[i]);
            if (word != null) {
                validWordsTiles.AddRange(word.letters);
            }
            word = GetLongestWordVertical(checkTiles[i]);
            if (word != null) {
                validWordsTiles.AddRange(word.letters);
            }
        }
        validWordsTiles = validWordsTiles.Distinct().ToList();
        if (validWordsTiles.Count > 0) {
            SubmitTiles(validWordsTiles);
        }

    }

    Word GetLongestWord(Tile t, bool horizontal) {
        if (horizontal) {
            return GetLongestWordHorizontal(t);
        } else {
            return GetLongestWordVertical(t);
        }
    }

    private Word GetLongestWordHorizontal(Tile t) {
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

    private Word GetLongestWordVertical(Tile t) {
        bool valid = false;

        int y = t.pos.y;
        List<Tile> tiles = new List<Tile>();

        while (y > 0) {
            tiles.Add(board.Get(t.pos.x, y));
            y--;
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

    public void SubmitTiles(List<Tile> tiles) {
        int amnt = tiles.Count;
        canExit = false;
        foreach (Tile tile in tiles) {
            tile.MakeParticles();
            tile.powerUp?.Submit(tile);
            Letter l = board.RandomLetterWeighted();
            if (board.CurrentVowels() < 4) {
                l = board.RandomVowel();
            }
            StartCoroutine(tile.ChangeLetter(l, scaleSpeed, rotSpeed));
        }
        Audio.PlaySound("success");
        Submit(amnt);
        if (board.AmountLocked() == 0) {
            board.LockRandomTile();
        }
    }

    public bool CanExit() {
        return canExit;
    }

    public void OnHover(Tile t) {
        Word word = GetLongestWord(t, horizontal);
        ClearRings();
        if (word != null) {
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

    public void Change() {
        horizontal = !horizontal;
        if (horizontal) {
            horizontalArrows.SetActive(true);
            verticalArrows.SetActive(false);
        } else {
            horizontalArrows.SetActive(false);
            verticalArrows.SetActive(true);
        }
    }
}