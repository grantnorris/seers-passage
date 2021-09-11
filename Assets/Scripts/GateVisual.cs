using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateVisual : MonoBehaviour
{
    Gate gate;

    void Start() {
        gate = GetComponentInParent<Gate>();
    }

    public void StartSound() {
        AudioManager.instance.PlayOneShot("Grinding Start", gate.volume);
        AudioManager.instance.PlayOneShot("Grinding", gate.volume);
    }

    public void StopSound() {
        AudioManager.instance.Stop("Grinding");
        AudioManager.instance.PlayOneShot("Grinding End", gate.volume);
    }
}
