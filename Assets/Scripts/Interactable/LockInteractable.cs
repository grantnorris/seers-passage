using UnityEngine;

public class LockInteractable : Interactable
{
    Gate gate;

    void Start() {
        gate = GetComponent<Gate>();

        if (gate != null && gate.anim != null) {
            gate.anim.SetBool("locked", true);
        }  
    }

    public override void Interact() {
        if (gate == null) {
            return;
        }

        if (!Inventory.instance.Use("Key")) {
            DialogueManager.instance.StartDialogue(new Dialogue(new string[] {"There's a padlock here, the key must be around somewhere."}));
            return;
        }

        gate.anim.SetBool("locked", false);
        AudioManager.instance.PlayOneShot("Key Pickup");
    }
}
