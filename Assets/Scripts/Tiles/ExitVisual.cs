using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitVisual : MonoBehaviour
{
    [SerializeField]
    [Tooltip("up, down, left or right.\nDefaults to up.")]
    public string direction = "up";

    public void Start() {
        Animator anim = GetComponent<Animator>();

        if (anim == null) {
            return;
        }

        SetAnimatorState();
    }

    // Set animator state based on direction set in editor
    void SetAnimatorState() {
        int directionInt = 0;

        switch (direction) {
            case "up":
                directionInt = 1;
                break;
            case "right":
                directionInt = 2;
                break;
            case "down":
                directionInt = 3;
                break;
            case "left":
                directionInt = 4;
                break;
        }

        anim.SetInteger("directionFacing", directionInt);
    }
}
