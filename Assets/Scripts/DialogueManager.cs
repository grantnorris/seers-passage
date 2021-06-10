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

        DialogueType dialogueType = DialogueTypeByName(dialogue.type);

        if (dialogueType != null) {
            dialogueBox = Instantiate(dialogueType.dialogueBoxPrefab);
            dialogueBox.transform.SetParent(dialogueUI.transform, false);
        }

        if (dialogueBox == null) {
            EndDialogue();
            return;
        }

        StartCoroutine("OpenDialogueBox");
    }

    // Wait for a small amount of time and then start the dialogue
    IEnumerator OpenDialogueBox() {
        yield return new WaitForSeconds(.1f);
        dialogueUI.SetActive(true);

        Transform loader = dialogueBox.transform.GetChild(0).Find("Dialogue Box Loader");
        RectTransform loaderRect = loader != null ? loader.GetComponent<RectTransform>() : null;
        Transform dialogueBoxMain = dialogueBox.transform.GetChild(0).Find("Dialogue Box");

        if (loader == null || loaderRect == null || dialogueBoxMain == null) {
            DisplayNextSentence();
            yield break;
        }

        Vector2 loaderSize = loaderRect.sizeDelta;
        loader.gameObject.SetActive(true);
        dialogueBoxMain.gameObject.SetActive(false);

        float time = 0f;
        float speed = 4f;

        // Animate loader
        while (time < 1f) {
            time += Time.deltaTime * speed;
            loaderRect.sizeDelta = Vector2.Lerp(new Vector2(0,0), loaderSize, time);
            yield return null;
        }

        // Swap loader out with actual dialogue box
        loader.gameObject.SetActive(false);
        dialogueBoxMain.gameObject.SetActive(true);

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