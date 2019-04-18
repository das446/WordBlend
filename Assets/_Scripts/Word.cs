using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour {
    public List<Tile> letters;

    public Word(List<Tile> letters) {
        this.letters = new List<Tile>(letters);
    }

    public int Length(){
        return letters.Count;
    }

    public Word RemoveLast() {
        List<Tile> l = new List<Tile>(letters);
        l.RemoveAt(l.Count - 1);
        return new Word(l);
    }

    public new string ToString() {
        string s = "";
        for (int i = 0; i < letters.Count; i++) {
            s = s + letters[i].letter.name;
        }
        return s.ToLower();
    }

    public Tile this [int i] {
        get { return letters[i]; }
    }
}