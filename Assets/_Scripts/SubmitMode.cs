using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubmitMode : MonoBehaviour, InputMode {

    List<Tile> selectedTiles=new List<Tile>();

    [SerializeField] GameObject submitButton;

    List<GameObject> ringPool = new List<GameObject>();
    [SerializeField] GameObject ring;

    public void Enter() {
        submitButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Exit() {
        foreach (GameObject g in ringPool) {
            g.SetActive(false);
        }
        selectedTiles = new List<Tile>();
        submitButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnClick() {

    }

    public void OnClick(Vector2 v) {
        
    }

    public void OnClick(Tile t) {
        bool valid = false;

        if (selectedTiles.Count == 0) {
            selectedTiles.Add(t);
            valid = true;
        } else {
            Vector3 expected = selectedTiles[selectedTiles.Count-1].transform.position + new Vector3(1, 0, 0);
            if (t.transform.position == expected) {
                selectedTiles.Add(t);
                valid = true;
            }
        }

        if (valid) {
            MakeRing(t.transform.position);
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
            s = s + selectedTiles[i].letter.name.ToLower();
        }
        return s;
    }

    public void SubmitWord() {
        Debug.Log(CurWord());
        bool valid = WordChecker.CheckWord(CurWord());
        if (valid) {
            Debug.Log("Valid");
        } else {
            Debug.Log("Invalid");
        }
    }
}