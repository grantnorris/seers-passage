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
        SetSubtitleTxt();
    }

    void SetSubtitleTxt() {
        if (subtitleTxt == null) {
            return;
        }

        subtitleTxt.SetText(GameManager.instance.level.name);
    }

    public void TransitionIn() {
        gameObject.SetActive(true);
    }

    public void TransitionOut() {
        if (anim == null) {
            return;
        }

        anim.SetTrigger("Transition Out");
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Quit() {
        GameManager.instance.ReturnToLevelSelect();
    }

    public void OpenTipsUI() {
        if (tipsUI == null) {
            return;
        }

        tipsUI.SetActive(true);
    }

    public void HideTipsUI() {
        if (tipsUI == null) {
            return;
        }

        tipsUI.SetActive(false);
    }
}
