using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    [Range(0f, 1f)]
    public float volume;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
    [Tooltip("Ignored if One Shot is true")]
    public bool loop;
    [Tooltip("If true, a dedicated audio source will not be created for this sound")]
    public bool oneShot;
    [Tooltip("The sound can be played as a theme and will be looped in the background")]
    public bool theme;
}
