using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Animator anim;
    public bool active;
    [SerializeField]
    [Tooltip("Will cause the gate to activate after each player movement.")]
    bool flipFlops;

    Collider2D col;

    void Start() {
        col = GetComponent<Collider2D>();

        if (flipFlops) {
            GameManager.instance.stepped.AddListener(Activate);
        }
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
