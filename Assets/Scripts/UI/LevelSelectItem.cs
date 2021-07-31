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
    Button retryBtn;
    [SerializeField]
    Button newFloorBtn;
    bool levelComplete = false;

    // Start is called before the first frame update
    public void Setup(Level lvl)
    {
        if (lvl == null) {
            return;
        }

        level = lvl;
        levelComplete = level.complete;

        // Enable relevant button based on if the level has alreadty been completed
        retryBtn.gameObject.SetActive(levelComplete);
        newFloorBtn.gameObject.SetActive(!levelComplete);

        SetLevelName();
        SetButtonOnClick();
    }

    void SetLevelName() {
        if (floorNumTxt == null) {
            return;
        }

        floorNumTxt.SetText(level.floorNumber);
    }

    void SetButtonOnClick() {
        if (LevelSelect.instance == null) {
            return;
        }

        Button btn = levelComplete ? retryBtn : newFloorBtn;
        btn.onClick.AddListener(delegate {LevelSelect.instance.SelectLevel(level);});
    }
}
