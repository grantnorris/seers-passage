using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory Item")]
public class InventoryItem : ScriptableObject
{
    [HideInInspector]
    public GameObject ui;
    public GameObject UiPrefab;
}
