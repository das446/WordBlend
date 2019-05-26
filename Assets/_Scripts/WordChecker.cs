using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class WordChecker : MonoBehaviour {
    static List<string> words = new List<string>();
    [SerializeField] string file;
    [SerializeField] int min, max;
    public static Dictionary<string, int> letterFrequency = new Dictionary<string, int>();
    public static int freq;
    void Start() {
        string f = Application.streamingAssetsPath + "/" + file;
        if (Application.platform == RuntimePlatform.Android) {
            GetAndroidPath();
        } else {
            string[] w = System.IO.File.ReadAllLines(file);
            LoadWordsFromFile(w);
            SetLetterFrequencies();

        }
    }

    private IEnumerator GetAndroidPath() {
        string filePath = "jar:file://" + Application.dataPath + "!/assets/" + file;
        UnityWebRequest www = new UnityWebRequest(filePath);
        yield return www.SendWebRequest();
        //www.downloadHandler.text;

    }

    private void LoadWordsFromFile(string[] w) {
        
        words = w.ToList();
        words = words.Where(x => x.Length >= min && x.Length <= max).ToList();
        //remove weird words that don't have vowels
        words = words.Where(x => x.Contains('a') || x.Contains('e') || x.Contains('i') || x.Contains('o') || x.Contains('u') || x.Contains('y')).ToList();

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

    private static int compare(string letter1, string letter2) {
        return letterFrequency[letter1] > letterFrequency[letter2] ? 1 : 0;
    }
}