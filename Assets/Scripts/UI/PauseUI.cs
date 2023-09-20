using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text subtitleTxt;
    [SerializeField]
    GameObject tipsUI;
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        
        SetSubtitleText();
    }

    // Set pause subtitle text to reflect the current level name
    void SetSubtitleText() {
        if (subtitleTxt == null) {
            return;
        }

        subtitleTxt.SetText(GameManager.instance.level.name);
    }

    // Transition the pause menu UI in
    public void TransitionIn() {
        gameObject.SetActive(true);
    }

    // Transition the pause menu UI out
    public void TransitionOut() {
        if (anim == null) {
            return;
        }

        anim.SetTrigger("Transition Out");
    }

    // Hide the pause menu UI
    public void Hide() {
        gameObject.SetActive(false);
    }

    // Quit the current level via pause UI
    public void Quit() {
        GameManager.instance.ReturnToLevelSelect();
    }

    // Open the pause menu tips UI
    public void OpenTipsUI() {
        if (tipsUI == null) {
            return;
        }

        tipsUI.SetActive(true);
    }

    // Hide the pause menu tips UI
    public void HideTipsUI() {
        if (tipsUI == null) {
            return;
        }

        tipsUI.SetActive(false);
    }
}
