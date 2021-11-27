using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : Interactable
{
    [SerializeField]
    InventoryItem item;

    public override void Interact() {
        GetComponent<Animator>().SetBool("Interacted", true);

        if (item == null) {
            return;
        }
        
        Inventory.instance.Add(item);
        AudioManager.instance.PlayOneShot("Key Pickup");
        Remove();
    }

    public void Remove() {
        this.enabled = false;
        gameObject.tag = "Untagged";
        GameManager.instance.player.interaction.CloseInteractNotice();
    }
}
