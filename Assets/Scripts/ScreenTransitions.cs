using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitions : MonoBehaviour
{
    public Material viewShaderMat;

    public void StartTransitionViewIn()
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

        float time = 0f;
        float seconds = 2.25f;

        viewShaderMat.SetFloat("IntroProgress", -1f);

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(-1f, 0f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, time)));
            viewShaderMat.SetFloat("IntroProgress", val);
            yield return null;
        }

        viewShaderMat.SetFloat("IntroProgress", 0f);

        Time.timeScale = 1;

        yield return new WaitForSeconds(.25f);

        GameManager.instance.uiController.DisplayGameUI();
    }

    public void StartTransitionViewOut() {
        StartCoroutine("TransitionViewOut");
    }

    // Transition view out
    IEnumerator TransitionViewOut() {
        if (viewShaderMat == null) {
            yield break;
        }

        float time = 0f;
        float seconds = 2f;

        viewShaderMat.SetFloat("OutroProgress", -.1f);

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(0f, 1f, time);
            viewShaderMat.SetFloat("OutroProgress", val);
            yield return null;
        }

        viewShaderMat.SetFloat("OutroProgress", 1f);

        // Time.timeScale = 1;

        GameManager.instance.uiController.DisplayOutroCard();
    }
}
