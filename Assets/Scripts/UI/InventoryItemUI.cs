using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    // Enable the icon gameobject for the inventory item
    public void DisplayIcon() {
        if (transform.childCount == 0) {
            return;
        }

        GameObject child = transform.GetChild(0).gameObject;
        child.SetActive(true);
    }
}
