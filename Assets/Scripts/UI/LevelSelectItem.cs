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
    Button button;

    // Start is called before the first frame update
    public void Setup(Level lvl)
    {
        if (lvl == null) {
            return;
        }

        level = lvl;

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

        button.onClick.AddListener(delegate {LevelSelect.instance.SelectLevel(level);});
    }
}
