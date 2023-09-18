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
    RectTransform contentRect;
    [SerializeField]
    GameObject closestItem;
    [SerializeField]
    GameObject selectedItem;
    bool watchMove = false;
    Vector2 scrolPos = Vector2.zero;
    Vector2 prevScrolPos = Vector2.zero;
    [SerializeField]
    CustomScrollRect scrollRect;

    void Start() {
        StartCoroutine("Init");
    }
    
    void LateUpdate() {
        // Snap to the closest item if the scroll is still moving but the player is no longer scrolling
        if (Mathf.Abs(scrolPos.y - prevScrolPos.y) < .0025f && watchMove && !UserScrolling()) {
            watchMove = false;
            scrollRect.inertia = false;
            StartCoroutine(SnapToItem());
        }

        prevScrolPos = scrolPos;
    }

    // Initialise the scroll highlight UI
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

        contentRect = content.GetComponent<RectTransform>();
        contentHeight = contentRect.sizeDelta.y - viewportHeight;
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

    // Create a new scroll highlight item with position references to a given UI transform
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

    // Create a chapter subtitle with position references to the UI transform it should appear before
    void CreateSubtitle(Transform item) {
        GameObject obj = item.gameObject;
        RectTransform rect = obj.GetComponent<RectTransform>();
        float pos = (rect.anchoredPosition.y * -1) / contentHeight - .01f;
        float height = rect.sizeDelta.y / contentHeight;
        ScrollHighlightSubtitle subtitle = item.GetComponent<ScrollHighlightSubtitle>();

        subtitle.gameobject = obj;
        subtitle.pos = pos;
        subtitle.height = height;
        subtitle.stickStart = pos - height;
        subtitles.Add(subtitle);
    }

    // Update highlighted item and sticky chapter subtitle based on current scroll location
    public void UpdateScroll(Vector2 pos) {
        scrollRect.inertia = true;
        scrolPos = pos;

        if (items.Count == 0) {
            return;
        }

        float scrollPos = (1 - pos.y);
        float closestDistance = 999f;

        foreach (ScrollHighlightItem item in items) {
            if (item.group == null) {
                continue;
            }
            
            if (scrollPos + viewportSize >= item.pos) {
                float distanceFromCenter = Mathf.Abs((item.pos + (item.height / 2)) - (scrollPos + viewportCenter));
                float alpha = 1 - (distanceFromCenter / viewportCenter);

                if (distanceFromCenter < closestDistance) {
                    closestDistance = distanceFromCenter;
                    closestItem = item.gameobject;
                }

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

        // Stop if the user didn't trigger the scroll manually
        if (!UserScrolling()) {
            return;
        }

        watchMove = true;
    }

    // Focus a given item
    public void FocusItem(GameObject item) {
        Canvas.ForceUpdateCanvases();
        contentRect.anchoredPosition = new Vector2(0, ItemPosition(item));
    }
    

    // Animates focus to the next item
    public void FocusNextItem(GameObject curItem) {
        StartCoroutine(ScrollToItem(NextItem(curItem)));
    }

    // Retrieve the next item after a given one
    GameObject NextItem(GameObject curItem) {
        int curIndex = curItem.transform.GetSiblingIndex();
        Transform parent = curItem.transform.parent;

        if (parent == null || parent.childCount <= curIndex + 1) {
            return null;
        }
  
        return parent.GetChild(curIndex + 1).gameObject;
    }

    // Retrieve the previous item before a given one
    GameObject PreviousItem(GameObject curItem) {
        int curIndex = curItem.transform.GetSiblingIndex();
        Transform parent = curItem.transform.parent;

        if (parent == null || curIndex == 0) {
            return null;
        }
  
        return parent.GetChild(curIndex - 1).gameObject;
    }

    // Scroll to a given item
    IEnumerator ScrollToItem(GameObject item) {
        float time = 0f;
        float seconds = .25f;
        Vector2 startPos = contentRect.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, ItemPosition(item));

        while (time <= 1f) {
            // Stop if the user starts scrolling
            if (UserScrolling()) {
                yield break;
            }

            time += Time.deltaTime / seconds;
            contentRect.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, time));
            yield return null;
        }

        contentRect.anchoredPosition = endPos;
        selectedItem = item;
    }

    // Y position of a given item
    float ItemPosition(GameObject item) {
        RectTransform outerRect = GetComponent<RectTransform>();
        RectTransform currentItemRect = item.GetComponent<RectTransform>();
        return (currentItemRect.anchoredPosition.y * -1) - (outerRect.rect.height / 2) + (currentItemRect.rect.height / 2);
    }

    // Snap to the closest item
    IEnumerator SnapToItem() {
        // Wait for next frame
        yield return null;

        if (closestItem == null || contentRect == null) {
            yield break;
        }

        if (closestItem == selectedItem) {
            float scrollDiff = (contentRect.anchoredPosition.y + viewportCenter) - ItemPosition(closestItem);

            if (Mathf.Abs(scrollDiff) > 50f) {
                if (scrollDiff > 0) {
                    GameObject nextItem = NextItem(selectedItem);
                    closestItem = nextItem != null ? nextItem : closestItem;
                } else {
                    GameObject prevItem = PreviousItem(selectedItem);
                    closestItem = prevItem != null ? prevItem : closestItem;
                }
            }
        }

        if (closestItem == null) {
            yield break;
        }

        StartCoroutine(ScrollToItem(closestItem));
    }

    // Whether or not the user is currently scrolling via interaction
    bool UserScrolling() {
        if (Input.GetMouseButton(0) || Input.touchCount > 0) {
            return true;
        }

        return false;
    }
}