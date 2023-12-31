﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueParent;
    public DialogueType[] types;
    [HideInInspector]
    public UnityEvent dialogueEnded = new UnityEvent();

    GameObject dialogueUI;
    DialogueUI dialogueScript;
    Queue<string> sentences = new Queue<string>();
    bool typing = false;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (!typing) {
                DisplayNextSentence();
            }
        }
    }

    // Open UI with a given dialogue
    public void StartDialogue(Dialogue dialogue) {
        sentences.Clear();
        GameManager.instance.DisablePlayerMove();
        dialogueParent.SetActive(true);

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        if (dialogue.type == "") {
            dialogue.type = "generic";
        }

        DialogueType dialogueType = DialogueTypeByName(dialogue.type);

        if (dialogueType == null) {
            StartCoroutine("EndDialogue");
            return;
        }

        dialogueUI = Instantiate(dialogueType.dialogueBoxPrefab);
        dialogueUI.transform.SetParent(dialogueParent.transform, false);
        dialogueScript = dialogueUI.GetComponent<DialogueUI>();

        StartCoroutine("OpenDialogueBox");
    }

    // Wait for a small amount of time and then start the dialogue
    IEnumerator OpenDialogueBox() {
        // Play audio clip
        AudioManager.instance.PlayOneShot("Dialogue Open");

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
        while (time <= 1f) {
            time += Time.deltaTime * speed;
            loaderRect.sizeDelta = Vector2.Lerp(Vector2.zero, loaderSize, time);
            yield return null;
        }

        // Swap loader out with actual dialogue box
        loader.gameObject.SetActive(false);
        box.gameObject.SetActive(true);

        DisplayNextSentence(false);
    }

    // Display next sentence in a dialogue
    public void DisplayNextSentence(bool playSound = true) {
        if (sentences.Count == 0) {
            StartCoroutine("EndDialogue");
            return;
        }

        if (playSound) {
            // Play audio clip
            AudioManager.instance.PlayOneShot("Dialogue Open");
        }

        dialogueParent.GetComponent<Button>().enabled = false;
        dialogueUI.SetActive(true);
        dialogueScript.arrow.SetActive(false);
        StartCoroutine("TypeSentence", sentences.Dequeue());
    }

    // Type sentence into dialogue box
    IEnumerator TypeSentence(string sentence) {
        TMP_Text txtUI = dialogueScript.text;
        Color txtColor = new Color(204, 204, 204, 1);

        if (txtUI == null) {
            yield break;
        }

        typing = true;

        txtUI.SetText(sentence);
        txtUI.color = new Color(204, 204, 204, 0);
        txtUI.ForceMeshUpdate();

        TMP_TextInfo textInfo = txtUI.textInfo;
        int charCount = textInfo.characterCount;

        for (int i = 0; i < charCount; ++i) {
            if (textInfo.meshInfo[0].mesh == null) {
                // Dialogue box has been closed / destroyed, so stop here
                yield break;
            }

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int index = charInfo.vertexIndex;
        
            if (!charInfo.isVisible) {
                continue;
            }
        
            for (int c = 0; c < 4; c++) {
                txtUI.textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[index + c] = txtColor;
                AudioManager.instance.PlayOneShot("Type Dialogue");
            }
            
            textInfo.meshInfo[0].mesh.vertices = textInfo.meshInfo[0].vertices;
            txtUI.UpdateVertexData();
            yield return new WaitForSeconds(.025f);
        }
        
        typing = false;

        AllowContinue();
    }

    // Enable continue button and display continue arrow graphic
    void AllowContinue() {
        if (dialogueScript.arrow != null) {
            dialogueScript.arrow.SetActive(true);
        }

        dialogueParent.GetComponent<Button>().enabled = true;
    }

    // End dialogue
    IEnumerator EndDialogue() {
        // Play audio clip
        AudioManager.instance.PlayOneShot("Dialogue Close");

        if (dialogueUI != null) {
            GameObject loader = dialogueScript.loader;
            RectTransform loaderRect = loader != null ? loader.GetComponent<RectTransform>() : null;
            GameObject box = dialogueScript.box;

            loader.SetActive(true);
            box.SetActive(false);
            yield return new WaitForSeconds(.1f);

            float time = 0f;
            float speed = 4f;

            // Animate loader
            while (time <= 1f) {
                time += Time.deltaTime * speed;

                if (loaderRect == null) {
                    break;
                }

                loaderRect.sizeDelta = Vector2.Lerp(loaderRect.sizeDelta, Vector2.zero, time);
                yield return null;
            }

            Destroy(dialogueUI);
        }

        if (dialogueEnded != null) {
            dialogueEnded.Invoke();
        }

        dialogueParent.SetActive(false);

        if (GameManager.instance.uiController.heartbreakUI == null) {
            GameManager.instance.EnablePlayerMove();
        }
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