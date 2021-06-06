using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public PlayerControl playerControl;

    // Move player after initial animation
    public void MoveAfterAnimation() {
        if (playerControl != null) {
            playerControl.moving = true;
        }
    }

    // Reset player moving flag and end the moving animation
    public void StopMovingAnimation() {
        if (playerControl != null) {
            playerControl.moving = false;
            playerControl.anim.SetBool("moving", false);
        }
    }
}
