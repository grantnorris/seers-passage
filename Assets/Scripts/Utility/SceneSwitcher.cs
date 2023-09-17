using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    [HideInInspector]
    public Level level = null;
    [HideInInspector]
    public Level prevLevel = null;

    void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
    }

    public void LoadLevel(Level newLevel) {
        level = newLevel;
        SceneManager.LoadScene("Level");
    }

    public void ReloadLevel() {
        SceneManager.LoadScene("Level");
    }

    public void LoadLevelSelect() {
        prevLevel = level;
        level = null;
        SceneManager.LoadScene("Level Select");
    }
}
