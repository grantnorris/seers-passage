﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Animator anim;
    public bool active;

    Collider2D col;

    void Start() {
        col = GetComponent<Collider2D>();
    }
    
    // Toggle activated state of gate
    public void Activate() {
        if (!anim.isActiveAndEnabled) {
            anim.enabled = true;
        }

        if (!active) {
            if (anim != null) {
                anim.SetTrigger("activate");
            }

            if (col != null) {
                col.enabled = false;
            }

            active = true;
        } else {
            if (anim != null) {
                anim.SetTrigger("deactivate");
            }

            if (col != null) {
                col.enabled = true;
            }

            active = false;
        }
    }
}
