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
        Initialise();
    }
    
    void Initialise() {
        if (chapters.Length < 1 || content == null || itemPrefab == null) {
            Logger.Send("Cancel because something's not right.", "general", "assertion");
            return;
        }

        PlayTheme();
        PopulateUI();
    }

    // Play level select audio theme
    void PlayTheme() {
        if (AudioManager.instance != null) {
            AudioManager.instance.PlayTheme("Theme 2");
        }
    }

    // Populate level select UI
    void PopulateUI() {
        ProgressData progressData = SaveSystem.ProgressData();
        GameObject itemToFocus = null;
        Level levelJustPlayed = SceneSwitcher.instance != null ? SceneSwitcher.instance.prevLevel : null;
        int totalScore = progressData.TotalScore();
        
        // Loop through all chapters
        for (int c = 0; c < chapters.Length; c++) {
            Chapter chapter = chapters[c];

            if (chapter.levels.Length == 0) {
                continue;
            }

            GameObject subtitle = Instantiate(subtitlePrefab, content.transform);
            subtitle.GetComponent<LevelSelectSubtitle>().Setup(chapter, totalScore);

            // Loop through all levels in the chapter
            for (int l = 0; l < chapter.levels.Length; l++) {
                Level level = chapter.levels[l];

                if (level == null) {
                    continue;
                }

                // Instantiate and initialise item
                LevelScore score = progressData.GetScore(level);
                GameObject item = Instantiate(itemPrefab, content.transform);
                item.GetComponent<LevelSelectItem>().Setup(level, chapter);
                Level prevLevel = GameLevels.PreviousLevel(level);
                bool prevLevelCompleted = SaveSystem.LevelScore(prevLevel) != null ? true : false;

                if (levelJustPlayed == level || (levelJustPlayed == null && prevLevel != null && prevLevelCompleted)) {
                    // Set this item as the one to focus if we've just played the previous level or this level is the first uncompleted one
                    itemToFocus = item;
                }
            }
        }
        
        if (itemToFocus != null) {
            // Focus the intended item
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

    // Start the outward transition animation and load the given level
    public void SelectLevel(Level level) {
        Logger.Send($"Select level - {level.name}.");
        StartCoroutine("TransitionOut", level);
    }

    IEnumerator TransitionOut(Level level) {
        anim.SetTrigger("Out");

        yield return new WaitForSeconds(.1f);
        SceneSwitcher.instance.LoadLevel(level);
    }
}
