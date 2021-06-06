using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractNotice : MonoBehaviour
{
    public GameObject visual;
    Animator anim;

    void Start() {
        if (visual != null) {
            anim = visual.GetComponent<Animator>();
        }
    }

    // Run open animation for visual
    public void Open() {
        if (visual != null) {
            anim.SetTrigger("open");
        }
    }

    // Run close animation for visual
    public void Close() {
        if (visual != null && anim != null) {
            anim.SetTrigger("close");
        }
    }
}
