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
        }
    }

    public void Remove() {
        gameObject.SetActive(false);
    }
}
