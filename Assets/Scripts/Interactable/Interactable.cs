using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact() {
        Logger.Send("Interact.");
        // Inherited interactable class functionality happens here
    }
}
