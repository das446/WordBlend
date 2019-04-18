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

    void Start() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Letter l = RandomLetter();
                Tile t = baseTile.Create(new Vector2Int(x, y), l);
                tiles.Add(t);
                t.transform.parent = transform;
            }
        }
    }

    public Tile Get(Vector2Int pos) {
        CheckBounds(pos);
        Tile tile = tiles.First(t => t.pos == pos && t.gameObject.activeSelf);
        if (tile == null) {
            throw new System.NullReferenceException("No tile at pos=" + pos);
        }
        return tile;
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

    public void RotateClockwise(Vector2Int pivot) {
        Tile sw = Get(pivot);
        Tile nw = Get(pivot, 0, 1);
        Tile ne = Get(pivot, 1, 1);
        Tile se = Get(pivot, 1, 0);

        sw.Offset(0, 1);
        nw.Offset(1, 0);
        ne.Offset(0, -1);
        se.Offset(-1, 0);

        if (BoardChange != null) {
            BoardChange(this);
        }
    }

    public void RotateCounterClockwise(Vector2Int pivot) {
        Tile sw = Get(pivot);
        Tile nw = Get(pivot, 0, 1);
        Tile ne = Get(pivot, 1, 1);
        Tile se = Get(pivot, 1, 0);

        sw.Offset(1, 0);
        nw.Offset(0, -1);
        ne.Offset(-1, 0);
        se.Offset(0, 1);

        if (BoardChange != null) {
            BoardChange(this);
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

    public Letter RandomLetter() {
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

}