using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccordionItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text titleTxt;
    [SerializeField]
    TMP_Text contentTxt;
    [SerializeField]
    RectTransform icon;
    [SerializeField]
    GameObject content;

    bool expanded = false;

    void Start() {
        if (content != null) {
            content.SetActive(false);
        }
    }

    // Toggle accordion on/off
    // This is triggered by a button click event
    public void Toggle() {
        if (content == null) {
            return;
        }

        expanded = !expanded;
        content.SetActive(expanded);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());

        if (icon == null) {
            return;
        }

        if (expanded) {
            icon.localScale = new Vector3(1, -1, 1);
        } else {
            icon.localScale = new Vector3(1, 1, 1);
        }

        AudioManager.instance.PlayOneShot("Dialogue Open");
    }

    // Set accordion item title UI text value
    public void SetTitle(string txt) {
        if (titleTxt == null) {
            return;
        }

        titleTxt.SetText(txt);
    }

    // Set accordion item content UI text value
    public void SetContent(string txt) {
        if (contentTxt == null) {
            return;
        }

        contentTxt.SetText(txt);
    }
}
