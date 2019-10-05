using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public PlayerControl playerControl;

    public void MoveAfterAnimation() {
        if (playerControl != null) {
            playerControl.moving = true;
            Debug.Log("start moving");
        }
    }

    public void StopMovingAnimation() {
        if (playerControl != null) {
            playerControl.moving = false;
            playerControl.anim.SetBool("moving", false);
            Debug.Log("start moving");
        }
    }
}
