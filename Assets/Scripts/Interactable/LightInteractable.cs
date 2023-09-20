using UnityEngine;

public class LightInteractable : Interactable
{
    public Dialogue pickupDialogue;

    [SerializeField]
    InventoryItem item;
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public override void Interact() {
        Pickup();
    }

    // Adds the light to the player's inventory and trigger's the dialogue
    void Pickup() {
        if (anim != null) {
            anim.SetTrigger("remove");
            AudioManager.instance.PlayOneShot("Light Torch");
            GameManager.instance.player.move.AddLight();

            if (item == null) {
                return;
            }

            Inventory.instance.Add(item);
        }

        StartDialogue();
        Remove();
    }

    // Start player pickup dialogue
    void StartDialogue() {
        if (DialogueManager.instance == null) {
            return;
        }

        DialogueManager.instance.StartDialogue(pickupDialogue);
    }
}
