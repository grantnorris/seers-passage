using UnityEngine;
using TMPro;

public class IntroUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text titletxt;

    void Start() {
        SetTitleText();
    }

    // Set intro title UI text value to level name
    void SetTitleText() {
        if (titletxt == null || SceneSwitcher.instance == null) {
            return;
        }

        titletxt.text = SceneSwitcher.instance.level.name;
    }

    // Transition the game view in and hide the intro UI gameobject once the animation is complete
    // This is triggered by an animation
    public void AnimationComplete() {
        GameManager.instance.screenTransitions.StartTransitionViewIn();
        gameObject.SetActive(false);
    }
}
