using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInteractable : Interactable
{
    public override void Interact() {
        FinishGame();
    }

    // Completes the level via the GameManager
    void FinishGame() {
        GameManager.instance.FinishGame();
    }
}
