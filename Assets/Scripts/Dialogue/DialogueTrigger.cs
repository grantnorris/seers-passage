using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance;

    public Dialogue onStartDialogue;
    public StepDialogueTrigger[] stepTriggers;

    
    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        // if (onStartDialogue.sentences.Length > 0) {
        //     GameManager.instance.levelStart.AddListener(OnStartDialogue);
        // }
    }

    // Initial dialogue to play on level start
    public void OnStartDialogue() {
        if (DialogueManager.instance == null) {
            return;
        }

        DialogueManager.instance.StartDialogue(onStartDialogue);
    }

    // Step count based triggers
    public void StepTriggers() {
        foreach (StepDialogueTrigger trigger in stepTriggers) {
            if (GameManager.instance.player.steps.StepCount() == trigger.steps) {
                DialogueManager.instance.StartDialogue(trigger.dialogue);
            }
        }
    }
}

[System.Serializable]
public class StepDialogueTrigger {
    public int steps;
    public Dialogue dialogue; 
}