using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitVisual : MonoBehaviour
{
    [SerializeField]
    [Tooltip("up, down, left or right.\nDefaults to up.")]
    string direction = "up";

    public void Start() {
        Animator anim = GetComponent<Animator>();

        if (anim == null) {
            return;
        }

        int directionInt = 0;

        switch (direction)
            {
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
