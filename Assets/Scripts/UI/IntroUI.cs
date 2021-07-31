using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text titletxt;

    void Start() {
        SetTitleText();
    }

    void SetTitleText() {
        if (titletxt == null || SceneSwitcher.instance == null) {
            return;
        }

        titletxt.text = SceneSwitcher.instance.level.name;
    }

    public void AnimationComplete() {
        GameManager.instance.screenTransitions.StartTransitionViewIn();
        gameObject.SetActive(false);
    }
}
