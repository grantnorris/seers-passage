using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueParent;
    public DialogueType[] types;

    GameObject dialogueUI;
    DialogueUI dialogueScript;


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
        dialogueParent.SetActive(true);

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence.ToUpper());
        }

        DialogueType dialogueType = DialogueTypeByName(dialogue.type);

        if (dialogueType == null) {
            EndDialogue();
            return;
        }

        dialogueUI = Instantiate(dialogueType.dialogueBoxPrefab);
        dialogueUI.transform.SetParent(dialogueParent.transform, false);
        dialogueScript = dialogueUI.GetComponent<DialogueUI>();

        StartCoroutine("OpenDialogueBox");
    }

    // Wait for a small amount of time and then start the dialogue
    IEnumerator OpenDialogueBox() {
        dialogueParent.SetActive(true);

        GameObject loader = dialogueScript.loader;
        RectTransform loaderRect = loader != null ? loader.GetComponent<RectTransform>() : null;
        GameObject box = dialogueScript.box;

        if (loader == null || loaderRect == null || box == null) {
            DisplayNextSentence();
            yield break;
        }

        loader.SetActive(true);
        box.SetActive(false);

        Vector2 loaderSize = loaderRect.sizeDelta;
        loaderRect.sizeDelta = Vector2.zero;
        
        yield return new WaitForSeconds(.1f);

        float time = 0f;
        float speed = 4f;

        // Animate loader
        while (time < 1f) {
            time += Time.deltaTime * speed;
            loaderRect.sizeDelta = Vector2.Lerp(Vector2.zero, loaderSize, time);
            yield return null;
        }

        // Swap loader out with actual dialogue box
        loader.gameObject.SetActive(false);
        box.gameObject.SetActive(true);

        DisplayNextSentence();
    }

    // Display next sentence
    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        dialogueParent.GetComponent<Button>().enabled = false;
        dialogueUI.SetActive(true);
        dialogueScript.arrow.SetActive(false);

        string sentence = sentences.Dequeue();
        StartCoroutine("TypeSentence", sentence);
    }

    // Type sentence into dialogue box
    IEnumerator TypeSentence(string sentence) {
        TMP_Text textUi = dialogueScript.text;

        if (textUi == null) {
            yield break;
        }

        textUi.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            textUi.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.25f);

        AllowContinue();
    }

    // Enable continue button and display continue arrow graphic
    void AllowContinue() {
        dialogueScript.arrow.SetActive(true);
        dialogueParent.GetComponent<Button>().enabled = true;
    }

    // End dialogue
    void EndDialogue() {
        if (dialogueUI != null) {
            Destroy(dialogueUI);
        }

        dialogueParent.SetActive(false);
        GameManager.instance.EnablePlayerControl();
    }

    // Get dialogue type by name
    DialogueType DialogueTypeByName(string name) {
        foreach (DialogueType type in types) {
            if (type.name == name) {
                return type;
            }
        }

        return null;
    }
}

[System.Serializable]
public class DialogueType {
    public string name;
    public Sprite ui;
    public GameObject dialogueBoxPrefab;
}