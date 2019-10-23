using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractable : Interactable
{
    public override void Interact() {
        Debug.Log("Interact with light!");
    }
}
