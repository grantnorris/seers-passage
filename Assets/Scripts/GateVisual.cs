using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateVisual : MonoBehaviour
{
    public void StartSound() {
        AudioManager.instance.Play("Grinding Start");
        AudioManager.instance.Play("Grinding");
    }

    public void StopSound() {
        AudioManager.instance.Stop("Grinding");
        AudioManager.instance.Play("Grinding End");
    }
}
