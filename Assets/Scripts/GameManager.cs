using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public PlayerMove PlayerMove;
    public bool playerControllable = false;
    public Material viewShaderMat;
    [HideInInspector]
    public UnityEvent levelStart;
    [HideInInspector]
    public UnityEvent stepped;
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
    }

    void Start() {
        StartCoroutine("TransitionInView");
    }

    // Transition view in
    IEnumerator TransitionInView() {
        Time.timeScale = 0;

        if (viewShaderMat == null) {
            yield break;
        }

        float progress = 0f;
        float seconds = 1.5f;

        viewShaderMat.SetFloat("TransitionProgress", -1f);

        while (progress <= 1f) {
            progress += Time.unscaledDeltaTime / seconds;
            float val = Mathf.Lerp(-1f, 0f, Mathf.SmoothStep(0f, 1f, progress));
            viewShaderMat.SetFloat("TransitionProgress", val);
            yield return null;
        }

        viewShaderMat.SetFloat("TransitionProgress", 0f);

        Time.timeScale = 1;

        yield return new WaitForSeconds(.25f);

        if (levelStart != null) {
            levelStart.Invoke();
        }
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

    // Disable player inputs for a given durations
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
        playerControllable = false;
    }

    // Enable player inputs
    public void EnablePlayerMove() {
        playerControllable = true;
    }
}
