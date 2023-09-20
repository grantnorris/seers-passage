using UnityEngine;

public class GameUI : MonoBehaviour
{
    // Start the current level once the UI is visible
    // This is triggered by an animation
    void UIVisible() {
        GameManager.instance.StartLevel();
    }
}
