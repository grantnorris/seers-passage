using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractable : Interactable
{
    public Dialogue pickupDialogue;

    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public override void Interact() {
        if (anim != null) {
            anim.SetTrigger("remove");

            if (GameManager.instance.playerControl != null) {
                GameManager.instance.playerControl.ExpandLightRadius();
                GameManager.instance.uiController.activateTorchUI();
            }
        }

        StartDialogue();
    }

    void StartDialogue() {
        if (DialogueManager.instance == null) {
            return;
        }

        DialogueManager.instance.StartDialogue(pickupDialogue);
    }

    public void Remove() {
        this.enabled = false;
        gameObject.tag = "Untagged";
        GameManager.instance.playerControl.CloseInteractNotice();
    }
}
