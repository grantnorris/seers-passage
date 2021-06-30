using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        CreateSources();
    }

    void CreateSources() {
        if (sounds.Length == 0) {
            return;
        }

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.volume = sound.volume;
            sound.source.clip = sound.clip;
        }
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null) {
            return;
        }

        sound.source.Play();
    }
}
