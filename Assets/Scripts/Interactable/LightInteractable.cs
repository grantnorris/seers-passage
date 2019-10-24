using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractable : Interactable
{
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public override void Interact() {
        if (anim != null) {
            anim.SetTrigger("remove");

            if (GameManager.instance.playerControl != null) {
                GameManager.instance.playerControl.ExpandLightRadius();
            }
        }
    }

    public void Remove() {
        gameObject.SetActive(false);
    }
}
