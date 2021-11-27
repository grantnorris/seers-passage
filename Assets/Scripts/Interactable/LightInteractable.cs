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
        GameManager.instance.player.move.CloseInteractNotice();

        if (anim != null) {
            anim.SetTrigger("remove");
            AudioManager.instance.PlayOneShot("Light Torch");
            GameManager.instance.player.move.AddLight();

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
