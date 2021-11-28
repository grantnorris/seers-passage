using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitions : MonoBehaviour
{
    public Material viewShaderMat;

    public void StartTransitionViewIn()
    {
        viewShaderMat.SetFloat("NoiseAmount", -.1f);
        // StartCoroutine("TransitionViewIn");
        GameManager.instance.player.move.shadowAnim.SetTrigger("open");
        GameManager.instance.uiController.DisplayGameUI();
    }

    // Transition view in
    // IEnumerator TransitionViewIn() {
    //     // Time.timeScale = 0;

    //     if (viewShaderMat == null) {
    //         yield break;
    //     }

    //     float time = 0f;
    //     float seconds = .5f;

    //     viewShaderMat.SetFloat("NoiseAmount", 1f);

    //     while (time <= 1f) {
    //         time += Time.unscaledDeltaTime / seconds;
    //         float val = Mathf.Lerp(1f, -.1f, time);
    //         viewShaderMat.SetFloat("NoiseAmount", val);
    //         yield return null;
    //     }

    //     viewShaderMat.SetFloat("NoiseAmount", -.1f);

    //     // Time.timeScale = 1;

    //     yield return new WaitForSeconds(.25f);

    //     GameManager.instance.uiController.DisplayGameUI();
    // }

    public void StartTransitionViewOut() {
        StartCoroutine("TransitionViewOut");
    }

    // Transition view out
    IEnumerator TransitionViewOut() {
        if (viewShaderMat == null) {
            yield break;
        }

        float time = 0f;
        float seconds = .5f;

        GameManager.instance.player.move.shadowAnim.SetTrigger("close");

        // viewShaderMat.SetFloat("NoiseAmount", -.1f);

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            // float val = Mathf.Lerp(0f, 1f, time);
            // viewShaderMat.SetFloat("NoiseAmount", val);
            yield return null;
        }

        // viewShaderMat.SetFloat("NoiseAmount", 1f);

        GameManager.instance.ReturnToLevelSelect();
    }

    // Transition door
    public IEnumerator TransitionDoor(DoorInteractable door) {
        if (viewShaderMat == null || door == null) {
            yield break;
        }

        GameManager.instance.player.interaction.StopWatchingInteractableChanges();
        GameManager.instance.player.move.ReduceLightRadius();

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
        
        GameManager.instance.player.move.ExpandLightRadius();

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(1.1f, -.1f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, time)));
            viewShaderMat.SetFloat("NoiseAmount", val);
            yield return null;
        }

        viewShaderMat.SetFloat("NoiseAmount", -.1f);
        GameManager.instance.player.interaction.StartWatchingInteractableChanges();
    }
}
