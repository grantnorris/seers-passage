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
        AudioManager.instance.PlayTheme("Theme 3", 1f);
    }

    public void PlayFlameSound() {
        AudioManager.instance.Play("Flame Ambience");
    }
}
