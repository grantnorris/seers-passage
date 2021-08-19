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
        AudioManager.instance.Play("Grinding Start", gate.volume);
        AudioManager.instance.Play("Grinding", gate.volume);
    }

    public void StopSound() {
        AudioManager.instance.Stop("Grinding");
        AudioManager.instance.Play("Grinding End", gate.volume);
    }
}
