using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueBox;
    public Text DialogueText;
    public DialogueType[] types;

    Queue<string> sentences = new Queue<string>();

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    // Start dialogue
    public void StartDialogue(Dialogue dialogue) {
        sentences.Clear();
        GameManager.instance.DisablePlayerControl();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence.ToUpper());
        }

        DialogueType dialogueType = null;

        foreach (DialogueType type in types) {
            if (type.name == dialogue.type) {
                dialogueType = type;
                break;
            }
        }

        if (dialogueType != null) {
            dialogueBox.GetComponent<Image>().sprite = dialogueType.ui;
        }

        DisplayNextSentence();
    }

    // Display next sentence
    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        dialogueBox.SetActive(true);

        string sentence = sentences.Dequeue();
        StartCoroutine("TypeSentence", sentence);
    }

    // Type sentence into dialogue box
    IEnumerator TypeSentence(string sentence) {
        DialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            DialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    // End dialogue
    void EndDialogue() {
        dialogueBox.SetActive(false);
        GameManager.instance.EnablePlayerControl();
    }
}

[System.Serializable]
public class DialogueType {
    public string name;
    public Sprite ui;
}