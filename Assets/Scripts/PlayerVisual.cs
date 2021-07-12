using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    PlayerMove PlayerMove;
    [SerializeField]
    Animator shadowAnim;

    void Start() {
        PlayerMove = GetComponentInParent<PlayerMove>();
    }

    // Move player after initial animation
    public void MoveAfterAnimation() {
        if (PlayerMove == null) {
            return;
        }

        PlayerMove.moving = true;
    }

    // Reset player moving flag and end the moving animation
    public void StopMovingAnimation() {
        if (PlayerMove == null) {
            return;
        }

        PlayerMove.moving = false;
        PlayerMove.anim.SetBool("moving", false);
    }

    public void Die() {
        GameManager.instance.EndDie();
    }

    public void LowLight() {
        if (shadowAnim == null) {
            return;
        }

        shadowAnim.SetBool("lowLight", true);
    }
}
