using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect instance;

    public Level[] levels;
    public GameObject content;
    public GameObject itemPrefab;
    Animator anim;
    ScrollRect scrollRect;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        anim = GetComponent<Animator>();
        scrollRect = GetComponent<ScrollRect>();
        Init();
    }

    void Init() {
        if (levels.Length < 1 || content == null || itemPrefab == null) {
            return;
        }

        GameObject itemToFocus = null;
        Level prevLvl = SceneSwitcher.instance != null ? SceneSwitcher.instance.prevLevel : null;
        
        foreach (Level level in levels) {
            if (level == null) {
                continue;
            }

            GameObject item = Instantiate(itemPrefab, content.transform);
            item.GetComponent<LevelSelectItem>().Setup(level);

            if (prevLvl == level || (prevLvl == null && level.previousLevel != null && level.previousLevel.complete)) {
                itemToFocus = item;
            }
        }
        
        if (itemToFocus != null) {
            StartCoroutine("FocusLevel", itemToFocus);
        }
    }

    IEnumerator FocusLevel(GameObject item) {
        if (item == null || scrollRect == null) {
            yield break;
        }

        // Wait for scrollhighlight to be initialised
        yield return new WaitForSeconds(.025f);

        Canvas.ForceUpdateCanvases();
        RectTransform contentRect = content.GetComponent<RectTransform>();
        RectTransform outerRect = GetComponent<RectTransform>();
        RectTransform itemRect = item.GetComponent<RectTransform>();
        float pos = (itemRect.anchoredPosition.y * -1) - (outerRect.rect.height / 2) + (itemRect.rect.height / 2);
        contentRect.anchoredPosition = new Vector2(0, pos);
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
