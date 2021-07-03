using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : Interactable
{
    public override void Interact() {
        Debug.Log("KEY!!");
        GetComponent<Animator>().SetBool("Interacted", true);
    }
}
