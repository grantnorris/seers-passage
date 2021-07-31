using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect instance;

    public Level[] levels;
    public GameObject content;
    public GameObject itemPrefab;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        Init();
    }

    void Init() {
        if (levels.Length < 1 || content == null || itemPrefab == null) {
            return;
        }

        foreach (Level level in levels) {
            GameObject item = Instantiate(itemPrefab, content.transform);
            item.GetComponent<LevelSelectItem>().Setup(level);
        }
    }

    public void SelectLevel(Level level) {
        Debug.Log("Select level - " + level.name);
        SceneSwitcher.instance.LoadLevel(level);
    }
}
