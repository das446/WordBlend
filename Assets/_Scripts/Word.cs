using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour {
    List<Tile> letters;

    public Word(List<Tile> letters) {
        this.letters = new List<Tile>(letters);
    }

    public Word RemoveLast() {
        List<Tile> l = new List<Tile>(letters);
        l.RemoveAt(l.Count - 1);
        return new Word(l);
    }

    public new string ToString() {
        string s = "";
        for (int i = 0; i < letters.Count; i++) {
            s = s + letters[i].ToString();
        }
        return s;
    }

    public Tile this [int i] {
        get { return letters[i]; }
    }
}