using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl control;
    public PlayerMove move;
    public PlayerInteraction interaction;
    public PlayerSteps steps;
    public PlayerHealth health;
    public PlayerVisual visual;

    // Set initial position of player transform
    public void SetInitialPosition(Vector3 pos) {
        transform.position = pos;
    }
}
