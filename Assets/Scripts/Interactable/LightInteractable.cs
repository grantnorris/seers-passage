using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractable : Interactable
{
    public Dialogue pickupDialogue;

    [SerializeField]
    InventoryItem item;
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public override void Interact() {
        gameObject.tag = "Untagged";
        GameManager.instance.playerMove.CloseInteractNotice();

        if (anim != null) {
            anim.SetTrigger("remove");
            GameManager.instance.audioManager.Play("Light Torch");
            GameManager.instance.playerMove.ExpandLightRadius();
            GameManager.instance.uiController.ActivateTorchUI();

            if (item == null) {
                return;
            }

            Inventory.instance.Add(item);
        }

        StartDialogue();
        Remove();
    }

    void StartDialogue() {
        if (DialogueManager.instance == null) {
            return;
        }

        DialogueManager.instance.StartDialogue(pickupDialogue);
    }

    public void Remove() {
        this.enabled = false;
    }
}
