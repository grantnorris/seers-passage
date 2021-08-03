using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect instance;

    public GameObject content;
    public GameObject itemPrefab;

    Level[] levels;
    Animator anim;
    ScrollRect scrollRect;
    ScrollHighlight scrollHighlight;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        levels = GameLevels.levels;
        anim = GetComponent<Animator>();
        scrollRect = GetComponent<ScrollRect>();
        scrollHighlight = GetComponent<ScrollHighlight>();
        Init();
    }

    void Init() {
        if (levels.Length < 1 || content == null || itemPrefab == null) {
            return;
        }

        GameObject itemToFocus = null;
        Level levelJustPlayed = SceneSwitcher.instance != null ? SceneSwitcher.instance.prevLevel : null;
        
        foreach (Level level in levels) {
            if (level == null) {
                continue;
            }

            GameObject item = Instantiate(itemPrefab, content.transform);
            item.GetComponent<LevelSelectItem>().Setup(level);
            Level prevLevel = GameLevels.PreviousLevel(level);

            if (levelJustPlayed == level || (levelJustPlayed == null && prevLevel != null && prevLevel.complete)) {
                itemToFocus = item;
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
