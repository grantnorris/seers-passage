using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] sentences;
    public string type;

    public Dialogue(string[] newSentences, string newType = "generic") {
        sentences = newSentences;
        type = newType;
    }
}
