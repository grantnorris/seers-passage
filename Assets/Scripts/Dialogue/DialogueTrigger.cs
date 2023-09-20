using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance;

    public Dialogue onStartDialogue;
    public StepDialogueTrigger[] stepTriggers;

    void Start() {
        if (instance == null) {
            instance = this;
        }
        
        InitialiseOnStartDialogue();
        InitialiseStepTriggerDialogue();
    }

    // Add a listener to trigger the on start dialogue at the beginning of the level if present
    void InitialiseOnStartDialogue() {
        if (onStartDialogue.sentences.Length > 0) {
            GameManager.instance.levelStart.AddListener(OnStartDialogue);
        }
    }

    // Add player moved listener for step triggers
    void InitialiseStepTriggerDialogue() {
        if (stepTriggers.Length > 0) {
            GameManager.instance.player.move.finishMoving.AddListener(DialogueTrigger.instance.StepTriggers);
        }
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