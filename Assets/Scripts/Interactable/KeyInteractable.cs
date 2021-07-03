using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : Interactable
{
    public override void Interact() {
        GetComponent<Animator>().SetBool("Interacted", true);
        Inventory.instance.Add(new InventoryItem("Key"));
    }
}
