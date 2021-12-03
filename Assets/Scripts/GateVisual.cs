using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateVisual : MonoBehaviour
{
    Gate gate;

    void Start() {
        gate = GetComponentInParent<Gate>();
    }

    public void StartSound() {
        AudioManager.instance.PlayOneShot("Grinding Start", gate.volume);
        AudioManager.instance.Play("Grinding", gate.volume);
    }

    public void StopSound() {
        AudioManager.instance.Stop("Grinding");
        AudioManager.instance.PlayOneShot("Grinding End", gate.volume);
    }

    public void FinishUnlock() {
        // Run animation
        gate.Activate();

        float animationDuration = .8f;

        StartCoroutine(GameManager.instance.DisablePlayerMoveForDuration(animationDuration));
        
        // Shake the camera
        if (CameraShake.instance != null) {
            StartCoroutine(CameraShake.instance.Shake(animationDuration, .05f));
        }
    }
}
