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
        OpenGate();
    }

    // Uses a key from the inventory to open the associated gate
    void OpenGate() {
        if (gate == null) {
            return;
        }

        if (!Inventory.instance.Use("Key")) {
            NoKeyDialogue();
            return;
        }

        gate.anim.SetBool("locked", false);
        AudioManager.instance.PlayOneShot("Key Pickup");
    }

    // Triggers the player dialogue when a key is required
    void NoKeyDialogue() {
        int random = Random.Range(1, 4);
        string[] dialogue;

        // Set the dialogue string to one of three phrases
        switch (random) {
            case 1:
                dialogue = new string[] {"There's a padlock here, the key must be around somewhere."};
                break;
            case 2:
                dialogue = new string[] {"I should find the key to progress."};
                break;
            default:
                dialogue = new string[] {"I need to find a key for this."};
                break;
        }

        DialogueManager.instance.StartDialogue(new Dialogue(dialogue, "player"));
    }
}
