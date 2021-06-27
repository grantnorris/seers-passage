using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // Run via animation event
    void UIVisible() {
        GameManager.instance.StartLevel();
    }
}
