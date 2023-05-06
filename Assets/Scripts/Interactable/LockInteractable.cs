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
            int r = Random.Range(1, 4);
            string[] d;

            switch (r)
            {
                case 1:
                    d = new string[] {"There's a padlock here, the key must be around somewhere."};
                    break;
                case 2:
                    d = new string[] {"I should find the key to progress."};
                    break;
                default:
                    d = new string[] {"I need to find a key for this."};
                    break;
            }

            DialogueManager.instance.StartDialogue(new Dialogue(d, "player"));
            return;
        }

        gate.anim.SetBool("locked", false);
        AudioManager.instance.PlayOneShot("Key Pickup");
    }
}
