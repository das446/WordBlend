﻿using System;
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
    void Start() {
        LoadWordsFromFile(Application.streamingAssetsPath + "/" + file);
        SetLetterFrequencies();
    }

    private void LoadWordsFromFile(string file) {
        string[] w = System.IO.File.ReadAllLines(file);
        words = w.ToList();
        words = words.Where(x => x.Length >= min && x.Length <= max).ToList();

    }

    public static bool CheckWord(Word word) {
        return words.Contains(word.ToString());
    }

    static void SetLetterFrequencies() {
        Debug.Log("Set frequencies");
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

        for (int i = 0; i < letters.Count; i++) {
            Debug.Log(letters[i] + "=" + letterFrequency[letters[i]]);
        }

    }

    private static int compare(string letter1, string letter2 ){
        return letterFrequency[letter1] > letterFrequency[letter2] ? 1 : 0;
    }
}