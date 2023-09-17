using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect instance;

    public GameObject content;
    public GameObject subtitlePrefab;
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

        if (AudioManager.instance != null) {
            AudioManager.instance.PlayTheme("Theme 2");
        }

        if (chapters.Length < 1 || content == null || itemPrefab == null) {
            Debug.LogWarning("Cancel because something's not right");
            return;
        }

        ProgressData progressData = SaveSystem.ProgressData();
        GameObject itemToFocus = null;
        Level levelJustPlayed = SceneSwitcher.instance != null ? SceneSwitcher.instance.prevLevel : null;
        int totalScore = progressData.TotalScore();
        
        for (int c = 0; c < chapters.Length; c++) {
            Chapter chapter = chapters[c];

            if (chapter.levels.Length > 0) {
                GameObject subtitle = Instantiate(subtitlePrefab, content.transform);
                subtitle.GetComponent<LevelSelectSubtitle>().Setup(chapter, totalScore);
            }

            for (int l = 0; l < chapter.levels.Length; l++) {
                Level level = chapter.levels[l];

                if (level == null) {
                    continue;
                }

                LevelScore score = progressData.GetScore(level);
                GameObject item = Instantiate(itemPrefab, content.transform);
                item.GetComponent<LevelSelectItem>().Setup(level, chapter);
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
            scrollHighlight.FocusNextItem(item);
        }
    }

    public void SelectLevel(Level level) {
        Debug.Log("Select level - " + level.name);
        StartCoroutine("TransitionOut", level);
    }

    IEnumerator TransitionOut(Level level) {
        anim.SetTrigger("Out");

        yield return new WaitForSeconds(.1f);
        SceneSwitcher.instance.LoadLevel(level);
    }
}
