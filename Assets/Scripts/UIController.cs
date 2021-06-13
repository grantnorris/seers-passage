using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public CanvasGroup headUI;
    public CanvasGroup footerUI;

    void Start() {
        Init();
    }

    void Init() {
        if (headUI == null || footerUI == null) {
            return;
        }

        headUI.alpha = 0f;
        footerUI.alpha = 0f;
        GameManager.instance.levelStart.AddListener(StartTransitionInUi);
    }

    void StartTransitionInUi() {
        StartCoroutine("TransitionInUI");
    }

    // Transition UI In
    public IEnumerator TransitionInUI() {
        if (headUI == null || footerUI == null) {
            yield break;
        }

        float progress = 0f;
        float speed = 2f;

        headUI.alpha = 0f;
        footerUI.alpha = 0f;

        while (progress < 1f) {
            progress += Time.deltaTime * speed;
            headUI.alpha = progress;
            footerUI.alpha = progress;
            yield return null;
        }

        headUI.alpha = 1f;
        footerUI.alpha = 1f;
    }
}
