using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : Interactable
{
    [SerializeField]
    InventoryItem item;

    public override void Interact() {
        AddToInventory();
        Remove();
    }
    
    // Adds the associated item to the inventory and triggers the animation
    void AddToInventory() {
        GetComponent<Animator>().SetBool("Interacted", true);

        if (item == null) {
            return;
        }
        
        Inventory.instance.Add(item);
        AudioManager.instance.PlayOneShot("Key Pickup");
        TipManager.DisplayTip("Keys");
    }
}
