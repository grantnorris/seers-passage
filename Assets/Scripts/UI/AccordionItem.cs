using System.Collections;
using System.Collections.Generic;
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

        switch (expanded)
        {
            case true:
            icon.localScale = new Vector3(1, -1, 1);
            break;
            
            case false:
            icon.localScale = new Vector3(1, 1, 1);
            break;
        }

        AudioManager.instance.PlayOneShot("Dialogue Open");
    }

    public void SetTitle(string txt) {
        if (titleTxt == null) {
            return;
        }

        titleTxt.SetText(txt);
    }

    public void SetContent(string txt) {
        if (contentTxt == null) {
            return;
        }

        contentTxt.SetText(txt);
    }
}
