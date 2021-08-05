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

    // Start is called before the first frame update
    public void Setup(Level lvl)
    {
        if (lvl == null) {
            return;
        }

        level = lvl;
        score = SaveSystem.LevelScore(level);
        levelComplete = SaveSystem.LevelScore(level) != null ? true : false;
        
        Level prevLevel = GameLevels.PreviousLevel(level);
        bool prevLevelComplete = prevLevel != null && SaveSystem.LevelScore(prevLevel) != null ? true : false;

        // Enable relevant button based on if the level has already been completed
        levelStats.gameObject.SetActive(levelComplete);
        newFloorBtn.gameObject.SetActive(!levelComplete && (prevLevel == null || prevLevelComplete) ? true : false);

        SetLevelName();
        SetSubtitleText();
        SetButtonOnClick();

        if (score != null) {
            SetScoreText();
            SetStepsText();
        }
    }

    void SetLevelName() {
        if (floorNumTxt == null) {
            return;
        }

        floorNumTxt.SetText(level.floorNumber);
    }

    void SetSubtitleText() {
        if (subtitleTxt == null) {
            return;
        }

        string txt = levelComplete ? "Floor\nComplete" : "Floor\nIncomplete";
        subtitleTxt.SetText(txt);
    }

    void SetScoreText() {
        if (scoreTxt == null) {
            return;
        }

        scoreTxt.SetText(score.Score());
    }

    void SetStepsText() {
        if (stepsTxt == null) {
            return;
        }

        stepsTxt.SetText(score.steps.ToString());
    }

    void SetTimeText() {
        if (timeTxt == null) {
            return;
        }

        float time = score.time;

        timeTxt.SetText(time.ToString());
    }

    void SetButtonOnClick() {
        if (LevelSelect.instance == null) {
            return;
        }

        Button btn = levelComplete ? retryBtn : newFloorBtn;
        btn.onClick.AddListener(delegate {LevelSelect.instance.SelectLevel(level);});
    }
}
