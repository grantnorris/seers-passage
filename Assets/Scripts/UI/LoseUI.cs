using UnityEngine;
using TMPro;

public class LoseUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text subtitleText;

    void Start() {
        SetSubtitleText();
    }

    // Set subtitle UI text value to the current level name
    void SetSubtitleText() {
        if (subtitleText != null) {
            subtitleText.SetText(GameManager.instance.level.name);
        }
    }

    // Play death theme over UI
    // This is triggered by an animation
    public void PlayDeathSound() {
        AudioManager.instance.PlayTheme("Theme 3", 1f);
    }
}
