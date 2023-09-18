using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [HideInInspector]
    public Level level;
    public Player player;
    public ScreenTransitions screenTransitions;
    public DragUI dragUI;
    [HideInInspector]
    public UnityEvent levelStart = new UnityEvent();
    [HideInInspector]
    public UIController uiController;
    [SerializeField]
    [Tooltip("Used for dev purposes if the level scene is played directly.")]
    Level fallbackLevel;
    float time = 0f;
    bool paused;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void Start() {
        screenTransitions = GetComponent<ScreenTransitions>();
        AudioManager.instance.PlayTheme("Theme", 1f);
        SetupLevel();
    }

    void Update() {
        if (!paused) {
            time += Time.deltaTime;
        }

        // Inputs for debugging
        if (Input.GetKeyDown(KeyCode.K)) {
            Logger.Send("Delete progress via key input.");
            SaveSystem.DeleteProgress();
        } else if (Input.GetKeyDown(KeyCode.Y)) {
            Logger.Send("Die via key input.");
            StartDie();
        }
    }

    void SetupLevel() {
        level = SceneSwitcher.instance != null ? SceneSwitcher.instance.level : fallbackLevel;

        if (level == null || level.prefab == null) {
            return;
        }

        if (SceneSwitcher.instance == null) {
            // Create scene switcher if not present (via loading level scene directly in editor)
            new GameObject("Scene Switcher").AddComponent<SceneSwitcher>().level = level;
        }

        player.steps.SetStepThreshold(level.stepThreshold);
        Instantiate(level.prefab);
    }

    // Invoke the levelStart unity event
    public void StartLevel() {
        paused = false;
        EnablePlayerMove();
        levelStart.Invoke();
        TipManager.DisplayTip("Movement");
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
        paused = true;

        player.control.DisallowInput();
    }

    // Enable player inputs
    public void EnablePlayerMove() {
        paused = false;

        player.control.AllowInput();
    }

    // Start the death senquence
    public void StartDie() {
        paused = true;
        player.interaction.interactNoticeScript.Close();
        player.transform.Find("Visual").GetComponent<PlayerVisual>().StartDie();
    }

    // End the death sequence
    public void EndDie() {
        GameManager.instance.uiController.DisplayLoseUI();
    }

    // Finish the level successfully
    public void FinishGame() {
        player.control.DisallowInput();
        screenTransitions.StartTransitionViewOut();
        LevelScore score = new LevelScore(
            player.health.Health(),
            player.steps.StepCount(),
            time,
            level.id
        );

        LevelScore prevScore = SaveSystem.LevelScore(level);

        if (prevScore == null || LevelScore.ScoreIsBetter(score, prevScore)) {
            SaveSystem.UpdateLevelScore(score);
        }
    }

    // Retry level via UI
    public void RetryLevel() {
        SceneSwitcher.instance.ReloadLevel();
    }

    // Return to level select via UI or finish game
    public void ReturnToLevelSelect() {
        SceneSwitcher.instance.LoadLevelSelect();
    }

    // Pause game and display UI
    public void PauseGame() {
        paused = true;
        uiController.DisplayPauseUI();
    }

    // Unpause game and hide UI
    public void UnpauseGame() {
        paused = false;
        uiController.HidePauseUI();
    }

    public bool Paused() {
        return paused;
    }
}
