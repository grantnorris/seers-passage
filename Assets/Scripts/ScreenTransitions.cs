using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitions : MonoBehaviour
{
    public Material viewShaderMat;

    // Start is called before the first frame update
    void Start()
    {
        viewShaderMat.SetFloat("OutroProgress", -.1f);
        StartCoroutine("TransitionViewIn");
    }

    // Transition view in
    IEnumerator TransitionViewIn() {
        Time.timeScale = 0;

        if (viewShaderMat == null) {
            yield break;
        }

        float progress = 0f;
        float seconds = 1.5f;

        viewShaderMat.SetFloat("IntroProgress", -1f);

        while (progress <= 1f) {
            progress += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(-1f, 0f, Mathf.SmoothStep(0f, 1f, progress));
            viewShaderMat.SetFloat("IntroProgress", val);
            yield return null;
        }

        viewShaderMat.SetFloat("IntroProgress", 0f);

        Time.timeScale = 1;

        yield return new WaitForSeconds(.25f);

        if (GameManager.instance.levelStart != null) {
            GameManager.instance.levelStart.Invoke();
        }
    }

    public void StartTransitionViewOut() {
        StartCoroutine("TransitionViewOut");
    }

    // Transition view out
    IEnumerator TransitionViewOut() {
        Time.timeScale = 0;

        if (viewShaderMat == null) {
            yield break;
        }

        float progress = 0f;
        float seconds = 2f;

        viewShaderMat.SetFloat("OutroProgress", -.1f);

        while (progress <= 1f) {
            progress += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(0f, 1f, progress);
            viewShaderMat.SetFloat("OutroProgress", val);
            yield return null;
        }

        viewShaderMat.SetFloat("OutroProgress", 0f);

        Time.timeScale = 1;

        yield return new WaitForSeconds(.25f);
    }
}
