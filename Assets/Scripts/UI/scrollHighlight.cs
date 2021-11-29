using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollHighlight : MonoBehaviour
{
    List<ScrollHighlightItem> items = new List<ScrollHighlightItem>();
    List<ScrollHighlightSubtitle> subtitles = new List<ScrollHighlightSubtitle>();
    [SerializeField]
    Transform content;
    float viewportCenter;
    float viewportSize;
    float contentHeight;

    void Start() {
        StartCoroutine("Init");
    }

    IEnumerator Init() {
        items.Clear();

        // Wait a frame for other UI scripts to initialise
        yield return null;

        if (content.childCount < 1) {
            yield break;
        }

        float viewportHeight = GetComponent<RectTransform>().rect.height;
        int topPadding = Mathf.RoundToInt((viewportHeight / 2) - (content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y / 2));
        int bottomPadding = Mathf.RoundToInt((viewportHeight / 2) - (content.GetChild(content.childCount - 1).GetComponent<RectTransform>().sizeDelta.y / 2));
        int spacing = topPadding;
        VerticalLayoutGroup contentLayout = content.GetComponent<VerticalLayoutGroup>();
        contentLayout.padding = new RectOffset(0, 0, topPadding, bottomPadding);
        contentLayout.spacing = spacing;

        // Wait a frame for other UI scripts to initialise
        yield return null;

        contentHeight = content.GetComponent<RectTransform>().sizeDelta.y - viewportHeight;
        viewportSize = viewportHeight / contentHeight;
        viewportCenter = viewportSize / 2;

        foreach (Transform child in content) {
            if (child.GetComponent<ScrollHighlightSubtitle>() != null) {
                CreateSubtitle(child);
            } else {
                CreateItem(child);
            }
        }

        // Add the end sticking point for each subtitle (based on the next subtitle after it)
        for (int i = 0; i < subtitles.Count - 1; i++) {
            subtitles[i].stickEnd = subtitles[i + 1].stickStart - subtitles[i].height;
        }
    }

    void CreateItem(Transform item) {
        GameObject obj = item.gameObject;
        RectTransform rect = obj.GetComponent<RectTransform>();
        float pos = (rect.anchoredPosition.y * -1) / contentHeight;
        float height = rect.sizeDelta.y / contentHeight;
        CanvasGroup group = item.GetComponent<CanvasGroup>();
        items.Add(new ScrollHighlightItem(
            obj,
            pos,
            height,
            group
        ));
    }

    void CreateSubtitle(Transform item) {
        GameObject obj = item.gameObject;
        RectTransform rect = obj.GetComponent<RectTransform>();
        float pos = (rect.anchoredPosition.y * -1) / contentHeight;
        float height = rect.sizeDelta.y / contentHeight;
        ScrollHighlightSubtitle subtitle = item.GetComponent<ScrollHighlightSubtitle>();

        subtitle.gameobject = obj;
        subtitle.pos = pos;
        subtitle.height = height;
        subtitle.stickStart = pos - height;
        subtitles.Add(subtitle);
    }

    public void UpdateScroll(Vector2 pos) {
        if (items.Count == 0) {
            return;
        }

        float scrollPos = (1 - pos.y);

        foreach (ScrollHighlightItem item in items) {
            if (item.group == null) {
                continue;
            }
            
            if (scrollPos + viewportSize >= item.pos) {
                float distanceFromCenter = Mathf.Abs((item.pos + (item.height / 2)) - (scrollPos + viewportCenter));
                float alpha = 1 - (distanceFromCenter / viewportCenter);

                if (alpha < 0) {
                    alpha = 0;
                }

                item.group.alpha = alpha;
            } else {
                item.group.alpha = 0;
            }
        }

        foreach (ScrollHighlightSubtitle subtitle in subtitles) {
            if (scrollPos < subtitle.stickStart) {
                subtitle.textObjRect.anchoredPosition = new Vector2(
                    subtitle.textObjRect.anchoredPosition.x,
                    0
                );
            } else if (subtitle.stickEnd != 0 && scrollPos > subtitle.stickEnd) {
                subtitle.textObjRect.anchoredPosition = new Vector2(
                    subtitle.textObjRect.anchoredPosition.x,
                    ((contentHeight * (subtitle.stickEnd - subtitle.pos)) + subtitle.textObjRect.rect.height) * -1
                );
            } else if (scrollPos >= subtitle.stickStart) {
                subtitle.textObjRect.anchoredPosition = new Vector2(
                    subtitle.textObjRect.anchoredPosition.x,
                    ((contentHeight * (scrollPos - subtitle.pos)) + subtitle.textObjRect.rect.height) * -1
                );
            }
        }
    }

    // Focus a given item
    public void FocusItem(GameObject item) {
        Canvas.ForceUpdateCanvases();
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchoredPosition = new Vector2(0, ItemPosition(item));
    }

    // Animates focus to the next item
    public IEnumerator FocusNextItem(GameObject curItem) {
        int curIndex = curItem.transform.GetSiblingIndex();
        Transform parent = curItem.transform.parent;

        if (parent == null || parent.childCount <= curIndex + 1) {
            yield break;
        }
  
        GameObject nextItem = parent.GetChild(curIndex + 1).gameObject;
        RectTransform contentRect = content.GetComponent<RectTransform>();
        
        float time = 0f;
        float seconds = 1f;
        Vector2 startPos = contentRect.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, ItemPosition(nextItem));

        while (time <= 1f) {
            time += Time.deltaTime / seconds;
            contentRect.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, time));
            yield return null;
        }

        contentRect.anchoredPosition = endPos;
    }

    // Y position of a given item
    float ItemPosition(GameObject item) {
        RectTransform outerRect = GetComponent<RectTransform>();
        RectTransform currentItemRect = item.GetComponent<RectTransform>();
        return (currentItemRect.anchoredPosition.y * -1) - (outerRect.rect.height / 2) + (currentItemRect.rect.height / 2);
    }
}