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

    void Start() {
        if (GameManager.instance != null) {
            GameManager.instance.player.move.finishMoving.AddListener(IncrementStepCount);
            stepped.AddListener(GameManager.instance.uiController.StartUpdateStepCountUI);
        }
    }

    // Increase the player's step count by 1
    public void IncrementStepCount() {
        stepCount--;

        if (DialogueManager.instance == null) {
            return;
        }

        StepCountDialogue();

        if (stepped != null) {
            stepped.Invoke();
        }
    }

    // Initialise the step count dialogue if a new threshold has just been met
    public void StepCountDialogue() {
        int goodSteps = stepThreshold * 2;
        int badSteps = stepThreshold;
        int die = 0;
        
        if (stepCount == goodSteps || stepCount == badSteps || stepCount == die) {
            GameManager.instance.uiController.StartBreakHeart();
        }
    }

    // Current player step score string
    public string StepScore() {
        switch (currentStepThreshold())
        {
            case 2:
            return "Good";

            case 3:
            return "Bad";

            default:
            return"Perfect";
        }
    }

    // Player step count
    public int StepCount() {
        return stepCount;
    }

    // Set the level step threshold
    public void SetStepThreshold(int steps) {
        stepThreshold = steps;
        stepCount = stepThreshold * 3;
    }

    // Retrieve the current step threshold the player is in
    public int StepThreshold() {
        return stepThreshold;
    }

    // Set the current step threshold based on the number of steps the player has made
    public int currentStepThreshold() {
        if (stepCount >= stepThreshold * 2) {
            return 1;
        } else if (stepCount >= stepThreshold) {
            return 2;
        } else {
            return 3;
        }
    }
}
