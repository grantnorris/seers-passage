using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    float masterVolume = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        CreateSources();
        Play("Theme");
    }

    void CreateSources() {
        if (sounds.Length == 0) {
            return;
        }

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.volume = sound.volume * masterVolume;
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null) {
            return;
        }

        sound.source.Play();
    }

    public void UpdateVolume(System.Single volume) {
        masterVolume = volume;

        foreach (Sound sound in sounds) {
            sound.source.volume = sound.volume * masterVolume;
        }
    }
}
