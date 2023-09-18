using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{   
    // Inherited interactable class functionality happens here on interaction
    public virtual void Interact() {
        Logger.Send("Interact.");
    }

    // Disables this script and untags the gameobject so it's no longer considered interactable
    public virtual void Remove() {
        GameManager.instance.player.interaction.CloseInteractNotice();
        gameObject.tag = "Untagged";
        this.enabled = false;
    }
}
