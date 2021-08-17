using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUI : MonoBehaviour
{
    public void PlayDeathSound() {
        AudioManager.instance.Stop("Theme");
        AudioManager.instance.Play("Death");
    }

    public void PlayFlameSound() {
        AudioManager.instance.Play("Flame Ambience");
    }
}
