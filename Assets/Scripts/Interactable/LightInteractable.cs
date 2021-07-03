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
            GameManager.instance.audioManager.Play("Light Torch");
            GameManager.instance.playerMove.ExpandLightRadius();
            GameManager.instance.uiController.ActivateTorchUI();
            Inventory.instance.Add(new InventoryItem("Torch"));
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
        GameManager.instance.playerMove.CloseInteractNotice();
    }
}
