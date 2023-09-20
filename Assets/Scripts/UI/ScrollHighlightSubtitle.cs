using UnityEngine;

public class ScrollHighlightSubtitle : MonoBehaviour
{
    [HideInInspector]
    public GameObject gameobject;
    [HideInInspector]
    public float pos;
    [HideInInspector]
    public float height;
    [HideInInspector]
    public CanvasGroup group;
    public RectTransform textObjRect;
    public float stickStart = 0;
    public float stickEnd = 0;

    void Start() {
        UpdateUISortingOrder();
    }

    // Update UI sorting order to display correctly
    void UpdateUISortingOrder() {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas != null) {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
        }
    }
}
