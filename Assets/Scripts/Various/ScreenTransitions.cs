using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitions : MonoBehaviour
{
    public Material viewShaderMat;

    // Transition the level scene view in
    public void StartTransitionViewIn()
    {
        viewShaderMat.SetFloat("NoiseAmount", -.1f);
        GameManager.instance.player.move.shadowAnim.SetTrigger("open");
        GameManager.instance.uiController.DisplayGameUI();
    }

    // Transition the level scene view in
    public void StartTransitionViewOut() {
        StartCoroutine("TransitionViewOut");
    }

    // Transition view of a level scene and back to the level select
    IEnumerator TransitionViewOut() {
        if (viewShaderMat == null) {
            yield break;
        }

        float time = 0f;
        float seconds = .5f;

        GameManager.instance.player.move.shadowAnim.SetTrigger("close");

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            yield return null;
        }

        GameManager.instance.ReturnToLevelSelect();
    }

    // Transition and move player through a given door
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
