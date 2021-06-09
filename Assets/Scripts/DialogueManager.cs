using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueUI;
    public DialogueType[] types;

    GameObject dialogueBox;

    Queue<string> sentences = new Queue<string>();

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        dialogueUI.SetActive(false);
    }

    // Start dialogue
    public void StartDialogue(Dialogue dialogue) {
        sentences.Clear();
        GameManager.instance.DisablePlayerControl();
        Destroy(dialogueBox);

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence.ToUpper());
        }

        DialogueType dialogueType = null;

        foreach (DialogueType type in types) {
            if (type.name == dialogue.type) {
                dialogueType = type;

                if (type.dialogueBoxPrefab != null && dialogueUI != null) {
                    dialogueBox = Instantiate(type.dialogueBoxPrefab);
                    dialogueBox.transform.SetParent(dialogueUI.transform, false);
                }

                break;
            }
        }

        if (dialogueBox == null) {
            EndDialogue();
            return;
        }

        StartCoroutine("WaitAndStartDialogue");
    }

    // Wait for a small amount of time and then start the dialogue
    IEnumerator WaitAndStartDialogue() {
        yield return new WaitForSeconds(.5f);
        dialogueUI.SetActive(true);
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
        TMP_Text textUi = dialogueBox.GetComponentInChildren<TMP_Text>(true);

        if (textUi == null) {
            Debug.Log("no text ui found");
            yield break;
        }

        textUi.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            textUi.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    // End dialogue
    void EndDialogue() {
        if (dialogueBox != null) {
            Destroy(dialogueBox);
        }

        dialogueUI.SetActive(false);
        GameManager.instance.EnablePlayerControl();
    }
}

[System.Serializable]
public class DialogueType {
    public string name;
    public Sprite ui;
    public GameObject dialogueBoxPrefab;
}