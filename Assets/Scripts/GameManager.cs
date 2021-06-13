using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text playerStepCountTxt;
    public GameObject player;
    public PlayerControl playerControl;
    public bool playerControllable = false;
    public Material viewShaderMat;
    public UnityEvent levelStart;

    int playerStepCount = 0;
    PlayerHealth playerHealth;
    UIController uiController;

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
        int perfectSteps = 7;
        int goodSteps = perfectSteps * 2;
        int badSteps = perfectSteps * 3;
        playerStepCount++;

        if (playerStepCountTxt != null) {
            playerStepCountTxt.text = playerStepCount.ToString();
        }

        if (DialogueManager.instance == null) {
            return;
        }

        if (playerStepCount == perfectSteps) {
            Dialogue dialogue = new Dialogue(new string[] {"An uneasy presence washes over you."});
            DialogueManager.instance.StartDialogue(dialogue);

            if (playerHealth != null) {
                playerHealth.ReduceHealth();
            }
        } else if (playerStepCount == goodSteps) {
            Dialogue dialogue = new Dialogue(new string[] {"A pressuring presence weighs on you."});
            DialogueManager.instance.StartDialogue(dialogue);

            if (playerHealth != null) {
                playerHealth.ReduceHealth();
            }
        } else if (playerStepCount == badSteps) {
            Dialogue dialogue = new Dialogue(new string[] {"A suffocating presence consumes you."});
            DialogueManager.instance.StartDialogue(dialogue);

            if (playerHealth != null) {
                playerHealth.ReduceHealth();
            }
        }
    }

    // Player step count
    public int StepCount() {
        return playerStepCount;
    }

    // Disable player inputs for a given durations
    public IEnumerator DisablePlayerControlForDuration(float duration) {
        if (duration > 0f) {
            float elapsed = 0f;

            while (duration > elapsed) { 
                elapsed += Time.deltaTime;
                DisablePlayerControl();

                yield return null;
            }

            EnablePlayerControl();
        }
    }

    // Disable player inputs
    public void DisablePlayerControl() {
        playerControllable = false;
    }

    // Enable player inputs
    public void EnablePlayerControl() {
        playerControllable = true;
    }
}
