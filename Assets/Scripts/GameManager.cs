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
        SetupLevel();
    }

    void Update() {
        if (!paused) {
            time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("try to save");
            // SaveSystem.SaveProgress(new ProgressData());
        } else if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("try to load");
            // SaveSystem.LoadProgress();
        } else if (Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("try to delete progress");
            SaveSystem.DeleteProgress();
        }
    }

    void SetupLevel() {
        level = SceneSwitcher.instance != null ? SceneSwitcher.instance.level : fallbackLevel;

        if (level == null || level.prefab == null) {
            return;
        }

        if (SceneSwitcher.instance == null) {
            // Create scene switcher if not present (via loading level scene directly [dev])
            new GameObject("Scene Switcher").AddComponent<SceneSwitcher>().level = level;
        }

        player.steps.SetStepThreshold(level.stepThreshold);
        Instantiate(level.prefab);
    }

    // Invoke the levelStart unity event
    public void StartLevel() {
        EnablePlayerMove();
        levelStart.Invoke();
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
        player.control.DisallowInput();
    }

    // Enable player inputs
    public void EnablePlayerMove() {
        if (player.control == null) {
            return;
        }

        player.control.AllowInput();
    }

    // Start the death senquence
    public void StartDie() {
        DisablePlayerMove();
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
        DisablePlayerMove();
        paused = true;
        uiController.DisplayPauseUI();
    }

    // Unpause game and hide UI
    public void UnpauseGame() {
        EnablePlayerMove();
        paused = false;
        uiController.HidePauseUI();
    }
}
