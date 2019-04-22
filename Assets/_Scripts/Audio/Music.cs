using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//using System.Diagnostics;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour {


    public List<Sound> songs;

    public Sound CurrentSong;
    private static AudioSource _source1;
    string currentSongName;

    private static Music instance = null;

    public float DefaultVolume = 0;

    public static AudioSource Source {
        get {
            if (_source1 == null) { Source = instance.GetComponent<AudioSource>(); }
            return _source1;
        }

        set {
            _source1 = value;
        }
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // SceneManager.activeSceneChanged += SceneManager_activeSceneChanged1; ;
    }

    void OnEnable() {

        //SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        //SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        ChangeSong(scene.buildIndex);
    }

    void Start() {
        Source.Stop();
        Source = GetComponent<AudioSource>();
        Source.volume = DefaultVolume;
        //CurrentSong = Songs[SceneManager.GetActiveScene().buildIndex];
        Source.loop = true;
        Source.clip = CurrentSong.sound;
        Source.Play();

    }

    // Update is called once per frame

    public static void PlaySound(Sound sound, float defaultVolume) {
        Source.PlayOneShot(sound.sound, defaultVolume);
    }

    public static void PlaySound(AudioClip sound, AudioSource music, float defaultVolume) {
        Debug.Log("Sound");
        music.PlayOneShot(sound, defaultVolume);
    }

    public static void Stop() {
        Source.Stop();
    }

    public static void Play() {
        Source.Play();
    }

    public static void ChangeSong(string SongName) {

        try {
            instance.CurrentSong = instance.songs.Where(x => x.name == SongName).First();
        } catch {
            Debug.LogWarning("No music named " + SongName);
            return;
        }
        Source.Stop();
        Source.loop = true;
        Source.clip = instance.CurrentSong.sound;
        Source.Play();
        instance.currentSongName = SongName;
    }

    public static void ChangeSong(int SongIndex) {

        Source.Stop();
        instance.CurrentSong = instance.songs[SongIndex];
        Source.loop = true;
        Source.clip = instance.CurrentSong.sound;
        Source.Play();
    }

    public static string currentSong() {
        return instance.currentSongName;
    }

}