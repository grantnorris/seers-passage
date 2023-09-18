using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // Start the current level once the UI is visible
    // Triggered via animation event
    void UIVisible() {
        GameManager.instance.StartLevel();
    }
}
