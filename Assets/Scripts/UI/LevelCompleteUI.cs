using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField]
    GameObject frame;
    [SerializeField]
    GameObject hearts;
    [SerializeField]
    Sprite heartSolid;
    [SerializeField]
    Sprite heartOutline;
    [SerializeField]
    TMP_Text scoreTxt;
    [SerializeField]
    GameObject Torches;
    List<Animator> torchAnimators = new List<Animator>();
    [SerializeField]
    CanvasGroup continueUI;
    int playerHealth;

    void Awake() {
        playerHealth = GameManager.instance.player.health.Health();
        SetHearts();
        SetScoreText();
        SetTorchAnimators();
        frame.GetComponent<Animator>().enabled = false;
    }
    
    void SetHearts() {
        if (hearts == null || heartSolid == null || heartOutline == null) {
            return;
        }

        int counter = 0;
        Debug.Log("player health = " + playerHealth);

        foreach (Image img in hearts.GetComponentsInChildren<Image>()) {
            counter++;
            Debug.Log("Check heart " + counter);

            if (playerHealth < counter) {
                img.GetComponent<SpriteRenderer>().sprite = heartOutline;
                Debug.Log(counter + " heart should be outline");
            }

            img.sprite = heartSolid;
        }
    }

    void SetScoreText() {
        if (scoreTxt == null) {
            return;
        }

        string txt = null;
        switch (playerHealth)
        {
        case 3:
            txt = "Perfect";
            break;
        case 2:
            txt = "Great";
            break;
        case 1:
            txt = "Close call";
            break;
        }

        scoreTxt.SetText(txt.ToUpper());
    }

    void SetTorchAnimators() {
        foreach (Animator anim in Torches.GetComponentsInChildren<Animator>()) {
            torchAnimators.Add(anim);
            anim.enabled = false;
        }
    }

    void EnableFrame() {
        frame.GetComponent<Animator>().enabled = true;
    }
    
    void EnableTorches() {
        foreach (Animator anim in torchAnimators) {
            anim.enabled = true;
        }
    }
}
