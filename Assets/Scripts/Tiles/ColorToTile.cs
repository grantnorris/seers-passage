using UnityEngine;

[System.Serializable]
public class ColorToTile
{   
    public string name;
    public Color color;
    public GameObject prefab;

    public ColorToTile(string newName, Color newColor, GameObject newPrefab) {
        name = newName;
        color = newColor;
        prefab = newPrefab;
    }
}
