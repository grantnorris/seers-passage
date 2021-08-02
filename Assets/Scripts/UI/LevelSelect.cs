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
        contentRect.anchoredPosition = new Vector2(0, ItemPosition(item));

        yield return new WaitForSeconds(1f);

        if (SceneSwitcher.instance.prevLevel != null) {
            StartCoroutine("FocusNextLevel", item);
        }
    }

    IEnumerator FocusNextLevel(GameObject curItem) {
        int curIndex = curItem.transform.GetSiblingIndex();
        Transform parent = curItem.transform.parent;

        if (parent == null || parent.childCount <= curIndex + 1) {
            yield break;
        }
  
        GameObject nextItem = parent.GetChild(curIndex + 1).gameObject;
        RectTransform contentRect = content.GetComponent<RectTransform>();
        
        float time = 0f;
        float seconds = .5f;
        Vector2 startPos = contentRect.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, ItemPosition(nextItem));

        while (time <= 1f) {
            time += Time.deltaTime / seconds;
            contentRect.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, time));
            yield return null;
        }

        contentRect.anchoredPosition = endPos;
    }

    float ItemPosition(GameObject item) {
        RectTransform outerRect = GetComponent<RectTransform>();
        RectTransform currentItemRect = item.GetComponent<RectTransform>();
        return (currentItemRect.anchoredPosition.y * -1) - (outerRect.rect.height / 2) + (currentItemRect.rect.height / 2);
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
