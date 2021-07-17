using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollHighlight : MonoBehaviour
{
    List<ScrollHighlightItem> items = new List<ScrollHighlightItem>();
    [SerializeField]
    Transform content;
    float viewportCenter;
    float viewportSize;

    void Start() {
        StartCoroutine("Init");
    }

    IEnumerator Init() {
        items.Clear();

        // Wait a frame for other UI scripts to initialise
        yield return null;

        float viewportHeight = GetComponent<RectTransform>().rect.height;
        int topPadding = Mathf.RoundToInt((viewportHeight / 2) - (content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y / 2));
        int bottomPadding = Mathf.RoundToInt((viewportHeight / 2) - (content.GetChild(content.childCount - 1).GetComponent<RectTransform>().sizeDelta.y / 2));
        int spacing = topPadding;
        VerticalLayoutGroup contentLayout = content.GetComponent<VerticalLayoutGroup>();
        contentLayout.padding = new RectOffset(0, 0, topPadding, bottomPadding);
        contentLayout.spacing = spacing;

        // Wait a frame for other UI scripts to initialise
        yield return null;

        float contentHeight = content.GetComponent<RectTransform>().sizeDelta.y - viewportHeight;
        viewportSize = viewportHeight / contentHeight;
        viewportCenter = viewportSize / 2;

        foreach (Transform child in content) {
            GameObject obj = child.gameObject;
            RectTransform rect = obj.GetComponent<RectTransform>();
            float pos = (rect.anchoredPosition.y * -1) / contentHeight;
            float height = rect.sizeDelta.y / contentHeight;
            CanvasGroup group = child.GetComponent<CanvasGroup>();
            items.Add(new ScrollHighlightItem(
                obj,
                pos,
                height,
                group
            ));
        }
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

            // Debug.Log("trigger point = " + scrollPos + ", item pos = " + item.pos);
            
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
    }
}