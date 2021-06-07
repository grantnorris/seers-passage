using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] sentences;

    public Dialogue(string[] newSentences) {
        sentences = newSentences;
    }
}
