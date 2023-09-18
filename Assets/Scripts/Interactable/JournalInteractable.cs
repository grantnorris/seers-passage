using UnityEngine;

public class JournalInteractable : Interactable
{
    [SerializeField]
    Dialogue dialogue;
    Animator anim;

    void Start() {
        anim = GetComponentInChildren<Animator>();
    }
    
    public override void Interact() {
        OpenJournal();
    }

    // Triggers the open animation and dialogue
    void OpenJournal() {
        if (anim == null) {
            return;
        }

        anim.SetBool("Open", true);

        if (dialogue == null) {
            return;
        }

        if (DialogueManager.instance == null) {
            return;
        }

        if (dialogue.type == null) {
            dialogue.type = "journal";
        }

        DialogueManager.instance.StartDialogue(dialogue);
        DialogueManager.instance.dialogueEnded.AddListener(CloseJournal);
    }

    // Resets the journal animator state and removes the dialogue listener
    public void CloseJournal() {
        anim.SetBool("Open", false);
        DialogueManager.instance.dialogueEnded.RemoveListener(CloseJournal);
    }
}
