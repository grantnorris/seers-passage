using System.Collections;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;
    [HideInInspector]
    public float masterVolume = .8f;
    AudioSource oneShotSource;
    Sound theme;

    void Awake() {
        SetInstanceOrDestroy();
        DontDestroyOnLoad(this.gameObject);
        masterVolume = PlayerPrefs.GetFloat("masterVolume", .5f);
        CreateSources();
        PlayTheme("Theme");
    }

    // Set this to be the instance of the script or destroy if one already exists
    void SetInstanceOrDestroy() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    // Create audio sources for each relevant defined audio file
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

            if (sound.theme) {
                // All themes should be playing by default with only the active one having a volume above zero
                // This allows us to fade in/out seamlessly
                sound.source.volume = 0;
                sound.source.Play();
            }
        }

        oneShotSource = gameObject.AddComponent<AudioSource>();
    }

    // Play looping sound
    public void Play(string name, float playVolume = 1f) {
        Sound sound = GetSound(name);

        if (sound == null) {
            return;
        }

        sound.source.volume = (sound.volume * masterVolume) * playVolume;
        sound.source.Play();
    }

    // Play one shot sound effect
    public void PlayOneShot(string name, float playVolume = 1f) {
        Sound sound = GetSound(name);

        if (sound == null) {
            return;
        }

        float oneShotVolume = (sound.volume * masterVolume) * playVolume;
        oneShotSource.PlayOneShot(sound.clip, oneShotVolume);
    }

    // Only one theme can be playing at a given time
    public void PlayTheme(string name, float fadeSpeed = .5f) {
        Sound sound = GetSound(name);

        if (sound == null || !sound.theme) {
            return;
        }

        StartCoroutine(FadeSounds(theme, sound, fadeSpeed));
        theme = sound;
    }

    // Fade between sounds
    IEnumerator FadeSounds(Sound current, Sound next, float seconds = .25f) {
        if (current == null && next == null) {
            yield break;
        }

        float time = 0f;

        while (time < 1f) {
            time += Time.deltaTime / seconds;

            if (current != null) {
                current.source.volume = (current.volume * masterVolume) * (1 - time);
            }

            if (next != null) {
                next.source.volume = (next.volume * masterVolume) * time;
            }
            
            yield return null;
        }

        if (current != null) {
            current.source.volume = 0;
        }

        if (next != null) {
            next.source.volume = next.volume * masterVolume;
        }
    }

    // Stop a sound playing
    public void Stop(string name) {
        Sound sound = GetSound(name);

        if (sound == null) {
            return;
        }

        sound.source.Stop();
    }

    // Update the master volume
    public void UpdateVolume(System.Single volume) {
        masterVolume = volume;
        PlayerPrefs.SetFloat("masterVolume", volume);

        foreach (Sound sound in sounds) {
            if ((sound.theme && sound != theme) || sound.source == null) {
                continue;
            }

            sound.source.volume = sound.volume * masterVolume;
        }
    }
    
    // Get a sound by name
    Sound GetSound(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null || (!sound.oneShot && sound.source == null)) {
            return null;
        }

        return sound;
    }
}
