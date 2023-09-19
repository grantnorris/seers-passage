using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool moving = false;
    public Animator[] anims;
    public Animator shadowAnim;
    public bool lowLight = true;
    [HideInInspector]
    public UnityEvent finishMoving = new UnityEvent();

    Player player;
    Vector3 targetPos;

    void Awake() {
        player = GetComponent<Player>();
    }

    void Start() {
        if (shadowAnim != null) {
            // Set initial state of shadow animation
            shadowAnim.SetBool("lowLight", true);
        }
    }
    
    void Update() {  
        UpdatePlayerIfMoving();
    }

    // Update player position
    void UpdatePlayerIfMoving() {
        if (moving && !GameManager.instance.Paused()) {
            if (transform.position != targetPos) {
                Logger.Send("Moving.", "player");
                MovePlayerTowardsTarget();
            } else {
                Logger.Send("Stop moving.", "player");
                ResetPlayerMovingState();
            }
        }
    }

    // Move player transform towards target
    void MovePlayerTowardsTarget() {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    // Reset player moving state, allow inputs and set animation triggers
    void ResetPlayerMovingState() {
        foreach (Animator anim in anims) {
            anim.SetTrigger("stopMoving");
        }

        player.control.AllowInput();
        moving = false;

        if (shadowAnim != null && !lowLight) {
            shadowAnim.SetBool("lowLight", false);
        }

        if (finishMoving != null) {
            finishMoving.Invoke();
        }
    }

    // Move in a given direction
    public void MoveInDirection(string direction) {
        Vector2 raycastDirection = Vector2.zero;
        Vector2 targetOffset = Vector2.zero;

        Logger.Send($"Move in {direction}.", "player");

        switch (direction) {
            case "left":
                raycastDirection = Vector2.left;
                targetOffset = Vector2.right;
                break;
            case "right":
                raycastDirection = Vector2.right;
                targetOffset = Vector2.left;
                break;
            case "up":
                raycastDirection = Vector2.up;
                targetOffset = Vector2.down;
                break;
            case "down":
                raycastDirection = Vector2.down;
                targetOffset = Vector2.up;
                break;
            default:
                Logger.Send("Moving in an invalid direction", "player");
                return;
        }

        // Cast a ray in the given direction to see how far to move until we hit a tile that would stop us
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection);
        Vector2 hitLocation = hit.transform.position;
        float hitDistance = Vector2.Distance(raycastOrigin, hitLocation);

        // If the hit is not an adjacent tile, then move towards it
        if (hitDistance > 1f) {
            Logger.Send("Hit a distant tile, so move towards it.", "player");
            MovePlayer(direction, hitLocation + targetOffset);
        } else {
            Logger.Send("Hit adjacent tile, so face that way and maybe interact with it.", "player");
            FaceDirection(direction);
            HitOrInteractWithAdjacentTile(hit);
        }
    }

    // Set player target position and trigger animations
    void MovePlayer(string direction, Vector2 pos) {
        int directionInt = DirectionToInt(direction);

        // Stop new player inputs
        player.control.DisallowInput();
        // Close any open interaction notices
        player.interaction.CloseInteractNotice();
        
        foreach (Animator anim in anims) {
            // Update player animators
            Logger.Send("Set player animation triggers.", "player");
            anim.SetInteger("directionFacing", directionInt);
            anim.SetTrigger("startMoving");
            player.visual.MoveAfterAnimation();
            anim.SetBool("moving", true);
        }

        Logger.Send("updated animations", "player");

        if (shadowAnim != null && !lowLight) {
            // Update shadow animations
            shadowAnim.SetBool("lowLight", true);
            Logger.Send("Updated shadow animations.", "player");
        }

        // Set the target position of the player
        targetPos = pos;

        Logger.Send($"Updated target position to {targetPos}.", "player");
    }

    // Does something with adjacent tile we're moving into
    void HitOrInteractWithAdjacentTile(RaycastHit2D hit) {
        Interactable hitInteractable = (hit.transform.tag == "Interactable" ? hit.transform.gameObject.GetComponent<Interactable>() : null);

        if (hitInteractable != null) {
            // If the hit tile is interactable
            hitInteractable.Interact();
        } else {
            // We are moving into a wall, shake the camera
            if (CameraShake.instance != null) {
                AudioManager.instance.PlayOneShot("Movement Failed");
                StartCoroutine(CameraShake.instance.Shake(.1f, .1f));
            }
        }
    }

    // Direction string to integer for animation triggers
    int DirectionToInt(string direction) {
        switch (direction) {
            case "up":
                return 1;
            case "right":
                return 2;
            case "down":
                return 3;
            case "left":
                return 4;
        }

        return 0;
    }

    // Face direction without moving
    public void FaceDirection(string direction) {
        int directionInt = DirectionToInt(direction);

        foreach (Animator anim in anims) {
            anim.SetInteger("directionFacing", directionInt);
        }
    }

    // Adds light to player
    public void AddLight() {
        lowLight = false;

        foreach (Animator anim in anims) {
            anim.SetBool("hasLight", true);
        }

        ExpandLightRadius();
    }

    // Reduce the light radius of the player
    public void ReduceLightRadius() {
        if (shadowAnim == null) {
            return;
        }

        shadowAnim.SetBool("lowLight", true);
    }

    // Expand the light radius of the player
    public void ExpandLightRadius() {
        if (shadowAnim == null || lowLight) {
            return;
        }

        shadowAnim.SetBool("lowLight", false);
    }

    // Current player target position
    public Vector3 TargetPos() {
        return targetPos;
    }
}
