using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    public float masterVolume = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        masterVolume = PlayerPrefs.GetFloat("masterVolume");
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
        PlayerPrefs.SetFloat("masterVolume", volume);

        foreach (Sound sound in sounds) {
            sound.source.volume = sound.volume * masterVolume;
        }
    }
}
