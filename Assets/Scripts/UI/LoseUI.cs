using UnityEngine;
using TMPro;

public class LoseUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text subtitleText;

    void Start() {
        if (subtitleText != null) {
            subtitleText.SetText(GameManager.instance.level.name);
        }
    }

    public void PlayDeathSound() {
        AudioManager.instance.Stop("Theme");
        AudioManager.instance.Play("Theme 2");
    }

    public void PlayFlameSound() {
        // AudioManager.instance.Play("Flame Ambience");
    }
}
