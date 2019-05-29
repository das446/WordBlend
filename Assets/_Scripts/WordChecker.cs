using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordChecker : MonoBehaviour {
    static List<string> words = new List<string>();
    [SerializeField] string file;
    [SerializeField] int min, max;
    public static Dictionary<string, int> letterFrequency = new Dictionary<string, int>();
    public static int freq;
    static Node WordTrie;

    struct Node {
        public string letter;
        public Dictionary<string, Node> children;
    }

    void Start() {
        LoadWordsFromFile(Application.streamingAssetsPath + "/" + file);
        SetLetterFrequencies();
    }

    private void LoadWordsFromFile(string file) {
        string[] w = System.IO.File.ReadAllLines(file);

        words = w.ToList();
        words = words.Select(s => s.ToLower()).ToList();
        words = words.Where(x => x.Length >= min && x.Length <= max).ToList();
        //remove weird words that don't have vowels
        words = words.Where(x => x.Contains('a') || x.Contains('e') || x.Contains('i') || x.Contains('o') || x.Contains('u') || x.Contains('y')).ToList();
        WordTrie = MakeTrie(words.ToArray());

    }

    Node MakeTrie(string[] words) {
        Node head = new Node();
        head.letter = "";
        head.children = new Dictionary<string, Node>();
        Node cur = head;
        for (int i = 0; i < words.Count(); i++) {
            string word = words[i];
            word += ".";
            cur = head;
            foreach (char letter in word) {
                string l = letter.ToString();
                if (!cur.children.ContainsKey(l)) {
                    Node newNode = new Node();
                    newNode.letter = l;
                    newNode.children = new Dictionary<string, Node>();
                    cur.children.Add(l, newNode);
                } else {
                    cur = cur.children[l];
                }
            }
        }
        return head;
    }

    public static bool CheckWord(Word word) {
        string s = "";
        foreach (Tile t in word.letters) {
            s += t.letter.name;
        }
        return CheckWord(s);

    }

    public static bool CheckWord(string word) {
        word = word.ToLower();
        word += ".";
        Node cur = WordTrie;
        foreach (char letter in word) {
            string l = letter.ToString();
            //Debug.Log(l);
            if (cur.children.ContainsKey(l)) {
                cur = cur.children[l];
            } else {
                return false;
            }
        }
        return true;
    }

    public static bool CheckPattern(Func<string, bool> p) {
        return words.Where(p).Count() > 0;
    }

    static void SetLetterFrequencies() {
        letterFrequency = new Dictionary<string, int>();
        freq = 0;
        List<string> letters = new List<string>();
        foreach (string word in words) {
            foreach (char letter in word) {
                string l = letter + "";
                if (letterFrequency.ContainsKey(l)) {
                    letterFrequency[l]++;
                } else {
                    letterFrequency.Add(l, 1);
                    letters.Add(l);
                }
                freq++;
            }

        }

        letters.Sort(compare);

    }

    private static int compare(string letter1, string letter2) {
        return letterFrequency[letter1] > letterFrequency[letter2] ? 1 : 0;
    }
}