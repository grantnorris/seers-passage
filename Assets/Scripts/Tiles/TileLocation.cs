using UnityEngine;

[System.Serializable]
public class TileLocation {
    public GameObject obj;
    public int x;
    public int y;
    public string type = "";

    public TileLocation(GameObject newObj, int newX, int newY, string newType) {
        obj  = newObj;
        x    = newX;
        y    = newY;
        type = newType;
    }
}