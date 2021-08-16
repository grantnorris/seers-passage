using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseUI : MonoBehaviour
{
    public TMP_Text subtitleTxt;
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
}
