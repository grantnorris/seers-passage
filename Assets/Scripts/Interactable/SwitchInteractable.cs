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
                // Target is gate tile type
                // Run animation
                target.GetComponent<Gate>().Activate();
                
                // Shake the camera
                if (CameraShake.instance != null) {
                    StartCoroutine(CameraShake.instance.Shake(.8f, .05f));
                }
            }
        }
    }
}
