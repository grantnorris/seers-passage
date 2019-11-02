using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteractable : Interactable
{
    public Animator anim;
    public GameObject target;

    public override void Interact() {
        if (anim != null) {
            anim.SetTrigger("interact");
        }

        if (target != null) {
            if (target.GetComponent<Gate>()) {
                float animationDuration = .8f;

                // Target is gate tile type
                // Run animation
                target.GetComponent<Gate>().Activate();

                StartCoroutine(GameManager.instance.DisablePlayerControlForDuraction(animationDuration));
                
                // Shake the camera
                if (CameraShake.instance != null) {
                    StartCoroutine(CameraShake.instance.Shake(animationDuration, .05f));
                }
            }
        }
    }
}
