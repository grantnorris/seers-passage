using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance;

    public Dialogue onStartDialogue;
    public StepDialogueTrigger[] stepTriggers;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        if (onStartDialogue.sentences.Length > 0) {
            GameManager.instance.DisablePlayerControl();
            StartCoroutine("StartDialogue");
        }
    }
    
    // Start dialogue
    IEnumerator StartDialogue() {
        if (DialogueManager.instance == null) {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        DialogueManager.instance.StartDialogue(onStartDialogue);
    }

    public void StepTriggers() {
        Debug.Log("step triggers");

        foreach (StepDialogueTrigger trigger in stepTriggers) {
            if (GameManager.instance.StepCount() == trigger.steps) {
                Debug.Log("yep, this trigger should go off");
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