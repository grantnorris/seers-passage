using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    public void DisplayIcon() {
        if (transform.childCount == 0) {
            return;
        }

        GameObject child = transform.GetChild(0).gameObject;
        child.SetActive(true);
    }
}
