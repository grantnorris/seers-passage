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

        DialogueManager.instance.StartDialogue(dialogue);
        DialogueManager.instance.dialogueEnded.AddListener(CloseJournal);
    }

    public void CloseJournal() {
        anim.SetBool("Open", false);
        DialogueManager.instance.dialogueEnded.RemoveListener(CloseJournal);
    }
}
