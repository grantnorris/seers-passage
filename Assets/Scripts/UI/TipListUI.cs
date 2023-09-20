using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipListUI : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    RectTransform rect;

    void Start() {
        rect = GetComponent<RectTransform>();
        
        Empty();
        Populate();
    }

    // Empty the tips UI
    void Empty() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    // Populate tips UI with the tips that the player has already seen
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

            string content = string.Join(" ", tip.dialogue.sentences);

            accordionItem.SetTitle(tip.name);
            accordionItem.SetContent(content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}
