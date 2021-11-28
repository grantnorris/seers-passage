using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect instance;

    public GameObject content;
    public GameObject itemPrefab;

    Chapter[] chapters;
    Animator anim;
    ScrollRect scrollRect;
    ScrollHighlight scrollHighlight;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        chapters = GameLevels.chapters;
        anim = GetComponent<Animator>();
        scrollRect = GetComponent<ScrollRect>();
        scrollHighlight = GetComponent<ScrollHighlight>();
        Init();
    }
    
    void Init() {
        if (chapters.Length < 1 || content == null || itemPrefab == null) {
            return;
        }

        ProgressData progressData = SaveSystem.ProgressData();
        GameObject itemToFocus = null;
        Level levelJustPlayed = SceneSwitcher.instance != null ? SceneSwitcher.instance.prevLevel : null;
        
        for (int c = 0; c < chapters.Length; c++) {
            for (int l = 0; l < chapters[c].levels.Length; l++) {
                Level level = chapters[c].levels[l];

                if (level == null) {
                    continue;
                }

                LevelScore score = progressData.GetScore(level);
                GameObject item = Instantiate(itemPrefab, content.transform);
                item.GetComponent<LevelSelectItem>().Setup(level);
                Level prevLevel = GameLevels.PreviousLevel(level);
                bool prevLevelCompleted = SaveSystem.LevelScore(prevLevel) != null ? true : false;

                if (levelJustPlayed == level || (levelJustPlayed == null && prevLevel != null && prevLevelCompleted)) {
                    itemToFocus = item;
                }
            }
        }
        
        if (itemToFocus != null) {
            StartCoroutine("FocusLevel", itemToFocus);
        }
    }

    // Focus level after build
    IEnumerator FocusLevel(GameObject item) {
        if (item == null || scrollRect == null) {
            yield break;
        }

        // Wait for scrollhighlight to be initialised
        yield return new WaitForSeconds(.025f);

        scrollHighlight.FocusItem(item);

        yield return new WaitForSeconds(2f);

        if (SceneSwitcher.instance.prevLevel != null) {
            StartCoroutine(scrollHighlight.FocusNextItem(item));
        }
    }

    public void SelectLevel(Level level) {
        Debug.Log("Select level - " + level.name);
        StartCoroutine("TransitionOut", level);
    }

    IEnumerator TransitionOut(Level level) {
        anim.SetTrigger("Out");

        yield return new WaitForSeconds(1f);
        SceneSwitcher.instance.LoadLevel(level);
    }
}
