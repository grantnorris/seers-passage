using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [HideInInspector]
    public Level level = null;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SwitchScene() {

    }
}
