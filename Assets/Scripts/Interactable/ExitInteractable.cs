using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInteractable : Interactable
{
    public override void Interact() {
        Debug.Log("exit interact!");
    }
}
