using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectItem : MonoBehaviour
{
    [HideInInspector]
    public Level level;

    [SerializeField]
    TMP_Text floorNumTxt;
    [SerializeField]
    TMP_Text subtitleTxt;
    [SerializeField]
    Button retryBtn;
    [SerializeField]
    Button newFloorBtn;
    [SerializeField]
    GameObject levelStats;
    [SerializeField]
    TMP_Text scoreTxt;
    [SerializeField]
    TMP_Text stepsTxt;
    [SerializeField]
    TMP_Text timeTxt;

    bool levelComplete = false;
    LevelScore score;

    // Set initial state of level select item UI based on level and chapter
    public void Setup(Level lvl, Chapter chapter) {
        if (lvl == null) {
            return;
        }

        level = lvl;
        bool chapterUnlocked = chapter.Unlocked();

        SetLevelName();

        if (!chapterUnlocked) {
            levelStats.gameObject.SetActive(false);
            newFloorBtn.gameObject.SetActive(false);
            SetSubtitleText();
            return;
        }
        
        score = SaveSystem.LevelScore(level);
        levelComplete = score != null ? true : false;
        Level prevLevel = GameLevels.PreviousLevel(level);
        bool prevLevelComplete = prevLevel != null && SaveSystem.LevelScore(prevLevel) != null ? true : false;
        
        SetSubtitleText();

        // Enable relevant button based on if the level has already been completed
        levelStats.gameObject.SetActive(levelComplete);
        newFloorBtn.gameObject.SetActive(!levelComplete && (prevLevel == null || prevLevelComplete) ? true : false);

        SetButtonOnClick();

        if (score != null) {
            SetScoreText();
            SetStepsText();
            SetTimeText();
        }
    }

    // Set the level name UI text value based on the level floor number
    void SetLevelName() {
        if (floorNumTxt == null) {
            return;
        }

        floorNumTxt.SetText(level.floorNumber);
    }

    // Set the subtitle UI text value based on the current completion status
    void SetSubtitleText() {
        if (subtitleTxt == null) {
            return;
        }

        string txt = levelComplete ? "Floor\nComplete" : "Floor\nIncomplete";
        subtitleTxt.SetText(txt);
    }

    // Set the score UI text value based on the current score
    void SetScoreText() {
        if (scoreTxt == null) {
            return;
        }

        scoreTxt.SetText(score.Score());
    }

    // Set the steps UI text value based on the current score
    void SetStepsText() {
        if (stepsTxt == null) {
            return;
        }

        stepsTxt.SetText(score.steps.ToString());
    }

    // Set the steps UI text value based on the current score
    void SetTimeText() {
        if (timeTxt == null) {
            return;
        }

        timeTxt.SetText(score.FormattedTime());
    }

    // Set relevant button listener to open the correlating level based on level completion status
    void SetButtonOnClick() {
        if (LevelSelect.instance == null) {
            return;
        }

        Button btn = levelComplete ? retryBtn : newFloorBtn;
        btn.onClick.AddListener(delegate {LevelSelect.instance.SelectLevel(level);});
    }
}
