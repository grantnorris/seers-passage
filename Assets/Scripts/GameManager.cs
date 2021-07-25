using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [HideInInspector]
    public Level level;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerMove playerMove;
    [HideInInspector]
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
    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public int stepThreshold;
    [SerializeField]
    [Tooltip("Used for dev purposes if the level scene is played directly.")]
    Level fallbackLevel;

    int playerStepCount = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void Start() {
        playerHealth = GetComponent<PlayerHealth>();
        uiController = GetComponent<UIController>();
        screenTransitions = GetComponent<ScreenTransitions>();
        audioManager = GetComponent<AudioManager>();
        SetupLevel();
    }

    void SetupLevel() {
        level = SceneSwitcher.instance != null ? SceneSwitcher.instance.level : fallbackLevel;

        if (level == null || level.prefab == null) {
            return;
        }

        if (SceneSwitcher.instance == null) {
            SceneSwitcher ss = new GameObject("Scene Switcher").AddComponent<SceneSwitcher>();
            ss.level = level;
        }

        stepThreshold = level.stepThreshold;
        Instantiate(level.prefab);
    }

    public void SetPlayer(GameObject p, PlayerMove pMove, PlayerControl pControl) {
        player = p;
        playerMove = pMove;
        playerControl = pControl;
        CameraManager.instance.Init();
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
        int goodSteps = stepThreshold * 2;
        int badSteps = stepThreshold * 3;
        
        if (playerStepCount == stepThreshold || playerStepCount == goodSteps || playerStepCount == badSteps) {
            uiController.StartBreakHeart();
        }
    }

    // Currentl player step score string
    public string StepScore() {
        int goodSteps = stepThreshold * 2;
        int badSteps = stepThreshold * 3;

        if (playerStepCount >= stepThreshold && playerStepCount < goodSteps) {
            return "Good";
        } else if (playerStepCount >= goodSteps) {
            return "Bad";
        }

        return "Perfect";
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

    // Start the death senquence
    public void StartDie() {
        playerMove.enabled = false;
        playerMove.interactNoticeScript.Close();
        player.transform.Find("Visual").GetComponent<Animator>().SetBool("dead", true);
    }

    // End the death sequence
    public void EndDie() {
        GameManager.instance.uiController.DisplayLoseUI();
    }

    // Finish the level successfully
    public void FinishGame() {
        playerControl.DisallowInput();
        screenTransitions.StartTransitionViewOut();
        uiController.DisplayOutroCard();
    }

    // Retry level via UI
    public void RetryLevel() {
        SceneSwitcher.instance.ReloadLevel();
    }
}
