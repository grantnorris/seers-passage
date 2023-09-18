using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Slider slider;

    void Start() {
        slider = GetComponent<Slider>();
        SetHandlePosition();
        SetListener();
    }

    void SetHandlePosition() {
        if (AudioManager.instance != null || slider != null) {
            float volume = AudioManager.instance.masterVolume;

            slider.value = volume;
        }
    }

    void SetListener() {
        if (AudioManager.instance != null) {
            slider.onValueChanged.AddListener(delegate {AudioManager.instance.UpdateVolume(slider.value);});
        }
    }
}
