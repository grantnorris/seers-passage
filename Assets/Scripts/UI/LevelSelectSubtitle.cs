using UnityEngine;
using TMPro;

public class LevelSelectSubtitle : MonoBehaviour
{
    [SerializeField]
    TMP_Text mainTxt;
    [SerializeField]
    TMP_Text subTxt;

    public void Setup(Chapter chapter, int totalScore) {
        mainTxt.SetText(chapter.name);

        if (chapter.scoreToUnlock > totalScore) {
            subTxt.SetText($"Collect {chapter.scoreToUnlock - totalScore} more to progress");
        } else {
            subTxt.gameObject.SetActive(false);
        }
    }
}
