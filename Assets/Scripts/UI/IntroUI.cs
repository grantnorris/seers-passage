using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUI : MonoBehaviour
{
    public void AnimationComplete() {
        GameManager.instance.screenTransitions.StartTransitionViewIn();
        gameObject.SetActive(false);
    }
}
