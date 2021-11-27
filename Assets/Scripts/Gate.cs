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
    public float volume = 1f;

    Collider2D col;

    void Start() {
        col = GetComponent<Collider2D>();

        if (flipFlops) {
            GameManager.instance.player.steps.stepped.AddListener(Activate);
        }
    }

    // Update sound effects volume based on distance from player
    void UpdateVolume() {
        float playerDistance = Vector3.Distance(GameManager.instance.player.gameObject.transform.position, transform.position);
        volume = Mathf.Max(1 - ((playerDistance - 1f) * .1f), .25f);
    }
    
    // Toggle activated state of gate
    public void Activate() {
        if (!anim.isActiveAndEnabled) {
            anim.enabled = true;
        }

        UpdateVolume();

        if (!active) {
            if (anim != null) {
                anim.SetTrigger("activate");
            }

            if (col != null) {
                col.enabled = false;
            }

            AudioManager.instance.Play("Grinding", volume);
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
