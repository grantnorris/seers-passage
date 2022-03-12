using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipListUI : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        Empty();
        Populate();
    }

    void Empty() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    void Populate() {
        if (itemPrefab == null) {
            return;
        }
        
        List<Tip> tips = TipManager.DisplayedTips();

        for (int i = 0; i < tips.Count; i++) {
            GameObject newItem = Instantiate(itemPrefab, transform);
            AccordionItem accordionItem = newItem.GetComponent<AccordionItem>();

            if (accordionItem == null) {
                continue;
            }

            accordionItem.SetTitle(tips[i].name);
            string content = "";

            foreach (string sentence in tips[i].dialogue.sentences) {
                if (content != "") {
                    content += " ";
                }

                content += sentence;
            }

            accordionItem.SetContent(content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}
