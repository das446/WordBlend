using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField] List<Tile> tiles;
    public int width, height;

    public static event Action<Board> BoardChange;

    [SerializeField] Tile baseTile;
    [SerializeField] List<Letter> letters;
    [SerializeField] int lockedAmnt;
    public int minVowels;
    [SerializeField] bool weightedRandom;
    public static Tile outOfBounds;
    public static Tile empty;

    void Start() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Letter l = GetRandomLetter();
                CreateTile(new Vector2Int(x, y), l);
            }
        }

        List<Tile> toLock = tiles.RandomItems(3);
        foreach (Tile item in toLock) {
            item.Lock();
        }

        empty = Instantiate(baseTile);
        outOfBounds = Instantiate(baseTile);
    }

    public Tile CreateTile(Vector2Int vector2Int) {
        Letter l = RandomLetterWeighted();
        if (CurrentVowels() < minVowels) {
            l = RandomVowel();
        }
        return CreateTile(vector2Int, l);
    }

    public Tile CreateTile(Vector2Int vector2Int, Letter l) {
        Tile t = baseTile.Create(vector2Int, l);
        t.board = this;
        tiles.Add(t);
        t.transform.parent = transform;
        return t;
    }

    public void RemoveTile(Tile t) {
        tiles.Remove(t);
        t.DestroyTile();
    }

    public Tile Get(Vector2Int pos) {

        if (pos.x >= width) {
            return outOfBounds;
        } else if (pos.x < 0) {
            return outOfBounds;
        } else if (pos.y >= height) {
            return outOfBounds;
        } else if (pos.y < 0) {
            return outOfBounds;
        }

        try {
            Tile tile = tiles.First(t => t.pos == pos && t.gameObject.activeSelf);
            return tile;
        } catch {
            return empty;
        }

    }

    /// <summary>
    /// Returns a tile at pos + offset x and y
    /// </summary>
    public Tile Get(Vector2Int pos, int x, int y) {
        return Get(pos + new Vector2Int(x, y));
    }

    public Tile Get(int x, int y) {
        return Get(new Vector2Int(x, y));
    }

    public Letter GetRandomLetter() {
        if (weightedRandom) {
            return RandomLetterWeighted();
        } else {
            return RandomLetter();
        }
    }

    public IEnumerator RotateAnimation(Vector2Int pivot) {
        yield return null;
    }

    void CheckBounds(Vector2Int pos) {
        if (pos.x >= width) {
            throw new System.IndexOutOfRangeException("pos.x=" + pos.x + " greater than width=" + width);
        } else if (pos.x < 0) {
            throw new System.IndexOutOfRangeException("pos.x=" + pos.x + " less than 0");
        } else if (pos.y >= height) {
            throw new System.IndexOutOfRangeException("pos.y=" + pos.y + "greater than height=" + height);
        } else if (pos.y < 0) {
            throw new System.IndexOutOfRangeException("pos.y=" + pos.y + "less than 0");
        }
    }

    public Letter RandomLetterWeighted() {
        int max = WordChecker.freq;
        int r = UnityEngine.Random.Range(0, max);
        int i = 0;

        foreach (string l in WordChecker.letterFrequency.Keys) {
            if (i >= r) {
                return letters.Where(x => x.name.ToLower() == l).First();
            } else {
                i += WordChecker.letterFrequency[l];

            }
        }
        return letters.Last();

    }

    public Letter RandomLetter() {
        return letters.RandomItem();
    }

    public Letter RandomVowel() {
        return letters.RandomItem(IsVowel);
    }

    private static bool IsVowel(Letter x) {
        return x.name == "A" || x.name == "E" || x.name == "I" || x.name == "O" || x.name == "U";
    }

    public int CurrentVowels() {
        return tiles.Where(x => IsVowel(x.letter)).Count();
    }

    public int AmountLocked() {
        return tiles.Where(x => !x.moveable).Count();
    }

    public void LockRandomTile() {
        tiles.RandomItem(x => x.moveable).Lock();
    }

}