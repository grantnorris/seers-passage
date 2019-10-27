using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteractable : Interactable
{
    public Animator anim;
    public GameObject target;

    public override void Interact() {
        Debug.Log("interact with switch");

        if (anim != null) {
            anim.SetTrigger("interact");
        }

        if (target != null) {
            if (target.GetComponent<Gate>()) {
                // Target is gate tile type
                target.GetComponent<Gate>().Activate();
            }
        }
    }
}
