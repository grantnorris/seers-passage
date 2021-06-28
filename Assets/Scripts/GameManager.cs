using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public PlayerMove playerMove;
    public PlayerControl playerControl;
    public ScreenTransitions screenTransitions;
    public DragUI dragUI;
    [HideInInspector]
    public UnityEvent levelStart = new UnityEvent();
    [HideInInspector]
    public UnityEvent stepped = new UnityEvent();
    [HideInInspector]
    public PlayerHealth playerHealth;
    [HideInInspector]
    public UIController uiController;

    int playerStepCount = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        playerHealth = GetComponent<PlayerHealth>();
        uiController = GetComponent<UIController>();
        screenTransitions = GetComponent<ScreenTransitions>();
    }

    // Invoke the levelStart unity event
    public void StartLevel() {
        GameManager.instance.levelStart.Invoke();
    }
    
    // Increase the player's step count by 1
    public void IncrementStepCount() {
        playerStepCount++;

        if (DialogueManager.instance == null) {
            return;
        }

        StepCountDialogue();

        if (stepped != null) {
            stepped.Invoke();
        }
    }

    public void StepCountDialogue() {
        int perfectSteps = 7;
        int goodSteps = perfectSteps * 2;
        int badSteps = perfectSteps * 3;
        
        if (playerStepCount == perfectSteps || playerStepCount == goodSteps || playerStepCount == badSteps) {
            if (playerHealth != null) {
                playerHealth.ReduceHealth();
                uiController.StartBreakHeart();
            }
        }
    }

    // Player step count
    public int StepCount() {
        return playerStepCount;
    }

    // Disable player inputs for a given duration
    public IEnumerator DisablePlayerMoveForDuration(float duration) {
        if (duration > 0f) {
            float elapsed = 0f;

            while (duration > elapsed) { 
                elapsed += Time.deltaTime;
                DisablePlayerMove();

                yield return null;
            }

            EnablePlayerMove();
        }
    }

    // Disable player inputs
    public void DisablePlayerMove() {
        playerControl.DisallowInput();
    }

    // Enable player inputs
    public void EnablePlayerMove() {
        playerControl.AllowInput();
    }

    public void FinishGame() {
        playerControl.DisallowInput();
        screenTransitions.StartTransitionViewOut();
        uiController.DisplayOutroCard();
    }
}
