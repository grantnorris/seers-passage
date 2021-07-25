using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public GameObject prefab;
    [Header("Displayed in the UI")]
    public string floorNumber;
}
