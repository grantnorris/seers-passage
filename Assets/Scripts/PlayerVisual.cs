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
        Logger.Send("Reset player moving flag and end the moving animation", "player");

        if (PlayerMove == null) {
            return;
        }

        if (PlayerMove.TargetPos() != PlayerMove.transform.position) {
            Logger.Send("Tried stopping the moving animation too early.", "player", "assertion");
            return;
        }

        PlayerMove.moving = false;

        foreach (Animator anim in PlayerMove.anims) {
            anim.SetBool("moving", false);
        }
    }

    public void StartDie() {
        foreach (Animator anim in PlayerMove.anims) {
            anim.SetBool("dead", true);
        }
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

    public void MoveInSound() {
        AudioManager.instance.PlayOneShot("Player Move In");
    }

    public void MoveSound() {
        // AudioManager.instance.PlayOneShot("Player Move");
    }

    public void MoveOutSound() {
        AudioManager.instance.PlayOneShot("Player Move Out");
    }
}
