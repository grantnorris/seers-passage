using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollHighlight : MonoBehaviour
{
    [SerializeField]
    List<ScrollHighlightItem> items = new List<ScrollHighlightItem>();
    [SerializeField]
    Transform content;
    float viewportCenter;

    void Start() {
        StartCoroutine("Init");
    }

    IEnumerator Init() {
        items.Clear();

        // Wait a frame for other UI scripts to initialise
        yield return null;

        float contentSize = content.GetComponent<RectTransform>().sizeDelta.y;
        float viewportSize = GetComponent<RectTransform>().rect.height;

        if (contentSize <= viewportSize) {
            yield return false;
        }

        viewportCenter = (viewportSize / 2) / contentSize;

        Debug.Log("viewport size = " + viewportSize);
        Debug.Log("viewport center = " + viewportCenter);

        foreach (Transform child in content) {
            GameObject obj = child.gameObject;
            float pos = (obj.GetComponent<RectTransform>().anchoredPosition.y * -1) / contentSize;
            items.Add(new ScrollHighlightItem(obj, pos));
        }
    }

    public void UpdateScroll(Vector2 pos) {
        if (items.Count == 0) {
            return;
        }

        float scrollPos = 1 - pos.y;

        foreach (ScrollHighlightItem item in items) {
            CanvasGroup group = item.gameobject.GetComponent<CanvasGroup>();

            if (group != null) {
                if (scrollPos + viewportCenter > item.pos) {
                    group.alpha = 1;
                } else {
                    group.alpha = 0;
                }
            }
        }

        Debug.Log(scrollPos);
    }
}

[System.Serializable]
public class ScrollHighlightItem {
    public GameObject gameobject;
    public float pos;

    public ScrollHighlightItem(GameObject newGameobject, float newPos) {
        gameobject = newGameobject;
        pos = newPos;
    }
}