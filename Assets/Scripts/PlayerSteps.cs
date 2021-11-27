using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSteps : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent stepped = new UnityEvent();

    int stepCount = 0;
    int stepThreshold = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null) {
            GameManager.instance.player.move.finishMoving.AddListener(IncrementStepCount);
            stepped.AddListener(GameManager.instance.uiController.StartUpdateStepCountUI);
        }
    }

    // Increase the player's step count by 1
    public void IncrementStepCount() {
        stepCount++;

        if (DialogueManager.instance == null) {
            return;
        }

        StepCountDialogue();

        if (stepped != null) {
            stepped.Invoke();
        }
    }

    public void StepCountDialogue() {
        int goodSteps = stepThreshold * 2;
        int badSteps = stepThreshold * 3;
        
        if (stepCount == stepThreshold || stepCount == goodSteps || stepCount == badSteps) {
            GameManager.instance.uiController.StartBreakHeart();
        }
    }

    // Currentl player step score string
    public string StepScore() {
        int goodSteps = stepThreshold * 2;
        int badSteps = stepThreshold * 3;

        if (stepCount >= stepThreshold && stepCount < goodSteps) {
            return "Good";
        } else if (stepCount >= goodSteps) {
            return "Bad";
        }

        return "Perfect";
    }

    // Player step count
    public int StepCount() {
        return stepCount;
    }

    // Set the level step threshold
    public void SetStepThreshold(int steps) {
        stepThreshold = steps;
    }

    public int StepThreshold() {
        return stepThreshold;
    }
}
