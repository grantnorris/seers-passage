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
        Logger.Send("Reset player moving flag and end the moving animation.", "player");

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

    // Trigger the die animation
    public void StartDie() {
        foreach (Animator anim in PlayerMove.anims) {
            anim.SetBool("dead", true);
        }
    }

    // Trigger the lose UI via the GameManager script
    // This is triggered by an animation
    public void Die() {
        GameManager.instance.EndDie();
    }

    // Set the shadow animator trigger to be in low light
    // This is triggered by an animation
    public void LowLight() {
        if (shadowAnim == null) {
            return;
        }

        shadowAnim.SetBool("lowLight", true);
    }

    // Play the sound for when the player begins moving
    // This is triggered by an animation
    public void MoveInSound() {
        AudioManager.instance.PlayOneShot("Player Move In");
    }

    // Play the looping sound for while the player is moving
    // This is triggered by an animation
    public void MoveSound() {
        // Currently disabled as the sound isn't quite right and it becomes grating
        // AudioManager.instance.PlayOneShot("Player Move");
    }

    // Play the sound for when the player finishes moving
    // This is triggered by an animation
    public void MoveOutSound() {
        AudioManager.instance.PlayOneShot("Player Move Out");
    }
}
