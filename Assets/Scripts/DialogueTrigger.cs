using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        if (DialogueManager.instance == null) {
            return;
        }

        DialogueManager.instance.StartDialogue(dialogue);
    }
}
