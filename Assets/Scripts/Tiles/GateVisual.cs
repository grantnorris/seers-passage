using UnityEngine;

public class GateVisual : MonoBehaviour
{
    Gate gate;

    void Start() {
        gate = GetComponentInParent<Gate>();
    }

    // Begin the sound that plays alongside the gate animation
    public void StartSound() {
        AudioManager.instance.PlayOneShot("Grinding Start", gate.volume);
        AudioManager.instance.Play("Grinding", gate.volume);
    }

    // Stop the sound that plays alongside the gate animation
    public void StopSound() {
        AudioManager.instance.Stop("Grinding");
        AudioManager.instance.PlayOneShot("Grinding End", gate.volume);
    }

    // Finish unlocking gate
    // This is triggered by an animation
    public void FinishUnlock() {
        float animationDuration = .8f;

        // Run animation
        gate.Activate();

        // Disable player input while the animation occurs
        StartCoroutine(GameManager.instance.DisablePlayerMoveForDuration(animationDuration));
        
        // Shake the camera
        if (CameraShake.instance != null) {
            StartCoroutine(CameraShake.instance.Shake(animationDuration, .05f));
        }
    }
}
