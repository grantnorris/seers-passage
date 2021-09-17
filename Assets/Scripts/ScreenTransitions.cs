using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitions : MonoBehaviour
{
    public Material viewShaderMat;

    public void StartTransitionViewIn()
    {
        viewShaderMat.SetFloat("NoiseAmount", -.1f);
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

        viewShaderMat.SetFloat("CrossfadeAmount", -1f);

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(-1f, 0f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, time)));
            viewShaderMat.SetFloat("CrossfadeAmount", val);
            yield return null;
        }

        viewShaderMat.SetFloat("CrossfadeAmount", 0f);

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

        viewShaderMat.SetFloat("NoiseAmount", -.1f);

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(0f, 1f, time);
            viewShaderMat.SetFloat("NoiseAmount", val);
            yield return null;
        }

        viewShaderMat.SetFloat("NoiseAmount", 1f);

        GameManager.instance.ReturnToLevelSelect();
    }

    // Transition door
    public IEnumerator TransitionDoor(DoorInteractable door) {
        // Time.timeScale = 0;

        if (viewShaderMat == null || door == null) {
            yield break;
        }

        // GameManager.instance.playerMove.ReduceLightRadius();
        float time = 0f;
        float seconds = .3f;

        viewShaderMat.SetFloat("CrossfadeAmount", 0);

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(-.1f, 1.1f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, time)));
            viewShaderMat.SetFloat("NoiseAmount", val);
            yield return null;
        }

        viewShaderMat.SetFloat("NoiseAmount", 1.1f);

        door.MovePlayer();
        time = 0f;
        // GameManager.instance.playerMove.ExpandLightRadius();

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(1.1f, -.1f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, time)));
            viewShaderMat.SetFloat("NoiseAmount", val);
            yield return null;
        }

        viewShaderMat.SetFloat("NoiseAmount", -.1f);

        // Time.timeScale = 1;
    }
}
