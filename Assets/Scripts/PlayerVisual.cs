using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    PlayerMove PlayerMove;

    void Start() {
        PlayerMove = GetComponentInParent<PlayerMove>();
    }

    // Move player after initial animation
    public void MoveAfterAnimation() {
        if (PlayerMove != null) {
            PlayerMove.moving = true;
        }
    }

    // Reset player moving flag and end the moving animation
    public void StopMovingAnimation() {
        if (PlayerMove != null) {
            PlayerMove.moving = false;
            PlayerMove.anim.SetBool("moving", false);
        }
    }
}
