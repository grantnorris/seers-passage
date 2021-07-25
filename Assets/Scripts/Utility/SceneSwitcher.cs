using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    [HideInInspector]
    public Level level = null;

    void Awake() {
        if (instance == null) {
            SceneSwitcher.instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevel(Level lvl) {
        level = lvl;

        SceneManager.LoadScene("Level");
    }

    public void ReloadLevel() {
        SceneManager.LoadScene("Level");
    }

    public void LoadLevelSelect() {
        SceneManager.LoadScene("Level Select");
    }
}
