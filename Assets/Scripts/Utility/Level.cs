using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
[System.Serializable]
public class Level : ScriptableObject
{
    public GameObject prefab;
    [Tooltip("Displayed in the UI")]
    public string floorNumber;
    [Tooltip("The lowest number of steps the level can be completed in – used for scoring.")]
    public int stepThreshold;
    public string id;
}
