using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

    public List<Sound> soundEffects;
    public float DefaultVolume;
    public static SoundEffects DefaultSounds;

    public void PlaySound(string Sound) {
        PlaySound(Sound, DefaultVolume);
    }

    void Start(){
        DefaultSounds = this;
    }

    public void PlaySound(string Sound, float volume) {
        if (soundEffects == null) {
            soundEffects = new List<Sound>();
        }
        if (HasSound(Sound)) {
            Sound s = soundEffects.Where(x => x.name == Sound).First();
            Music.PlaySound(s, volume);
            Music.Source.pitch = 1;
        } else if (this != DefaultSounds) {
            DefaultSounds.PlaySound(Sound, volume);
        } else {
            Debug.LogWarning("No sound effect named " + Sound);
        }
    }

    public void PlaySound(Sound Sound, float volume) {

        Music.PlaySound(Sound, volume);
        Music.Source.pitch = 1;

    }

    public void PlaySound(string Sound, AudioSource sounds, float volume) {
        if (soundEffects == null) {
            soundEffects = new List<Sound>();
        }
        if (HasSound(Sound)) {

            AudioClip s = soundEffects.Where(x => x.name == Sound).First().sound;
            Music.PlaySound(s, sounds, volume);
            Music.Source.pitch = 1;
        } else if (this != DefaultSounds) {
            DefaultSounds.PlaySound(Sound, sounds, volume);
        } else {
            Debug.LogWarning("No sound effect named " + Sound);
        }
    }

    public bool HasSound(string Sound) {
        return soundEffects.Where(x => x.name == Sound).Count() > 0;
    }

}