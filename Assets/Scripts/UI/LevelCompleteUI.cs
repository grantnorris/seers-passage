using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField]
    GameObject heartsUI;
    [SerializeField]
    Sprite heartSolid;
    [SerializeField]
    Sprite heartOutline;
    [SerializeField]
    TMP_Text scoreTxt;
    int playerHealth;

    void Awake() {
        playerHealth = GameManager.instance.playerHealth.Health();
        SetHearts();
        SetScoreText();
    }
    
    void SetHearts() {
        if (heartsUI == null || heartSolid == null || heartOutline == null) {
            return;
        }

        int counter = 0;

        foreach (Image img in heartsUI.GetComponentsInChildren<Image>()) {
            counter++;

            if (playerHealth < counter) {
                img.sprite = heartOutline;
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
}
