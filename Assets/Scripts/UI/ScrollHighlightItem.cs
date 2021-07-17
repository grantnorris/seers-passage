using UnityEngine;

[System.Serializable]
public class ScrollHighlightItem {
    public GameObject gameobject;
    public float pos;
    public float height;
    public CanvasGroup group;

    public ScrollHighlightItem(GameObject newGameobject, float newPos, float newHeight, CanvasGroup newGroup) {
        gameobject = newGameobject;
        pos = newPos;
        height = newHeight;
        group = newGroup;
    }
}