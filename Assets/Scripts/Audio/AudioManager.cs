using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;
    [HideInInspector]
    public float masterVolume = .8f;
    AudioSource oneShotSource;

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

        if (masterVolume == default) {
            PlayerPrefs.SetFloat("masterVolume", masterVolume);
        }

        CreateSources();
        Play("Theme");
    }

    void CreateSources() {
        if (sounds.Length == 0) {
            return;
        }

        foreach (Sound sound in sounds) {
            if (sound.oneShot) {
                continue;
            }

            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.volume = sound.volume * masterVolume;
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
        }

        oneShotSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play(string name, float playVolume = 1f) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null || sound.source == null) {
            return;
        }

        sound.source.volume = (sound.volume * masterVolume) * playVolume;
        sound.source.Play();
    }

    public void PlayOneShot(string name, float playVolume = 1f) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null) {
            return;
        }

        float oneShotVolume = (sound.volume * masterVolume) * playVolume;
        oneShotSource.PlayOneShot(sound.clip, oneShotVolume);
    }

    public void Stop(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null) {
            return;
        }

        sound.source.Stop();
    }

    public void UpdateVolume(System.Single volume) {
        masterVolume = volume;
        PlayerPrefs.SetFloat("masterVolume", volume);

        foreach (Sound sound in sounds) {
            sound.source.volume = sound.volume * masterVolume;
        }
    }
}
