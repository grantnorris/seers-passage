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
        
        List<string> tips = SaveSystem.DisplayedTips();

        for (int i = 0; i < tips.Count; i++) {
            GameObject newItem = Instantiate(itemPrefab, transform);
            AccordionItem accordionItem = newItem.GetComponent<AccordionItem>();
            Tip tip = TipManager.GetTip(tips[i]);

            if (accordionItem == null || tip == null) {
                continue;
            }

            accordionItem.SetTitle(tip.name);
            string content = "";

            foreach (string sentence in tip.dialogue.sentences) {
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
