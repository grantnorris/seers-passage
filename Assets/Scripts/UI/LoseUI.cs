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

    // Play death theme over pause UI
    public void PlayDeathSound() {
        AudioManager.instance.PlayTheme("Theme 3", 1f);
    }
}
