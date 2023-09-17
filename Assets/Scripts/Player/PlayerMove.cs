using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool moving     = false;
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
        if (DialogueTrigger.instance != null && DialogueTrigger.instance.stepTriggers.Length > 0) {
            finishMoving.AddListener(DialogueTrigger.instance.StepTriggers);
        }

        if (shadowAnim != null) {
            shadowAnim.SetBool("lowLight", true);
        }
    }

    // Update is called once per frame
    void Update()
    {  
        if (moving && !GameManager.instance.Paused()) {
            Logger.Send("moving", "player");

            if (transform.position != targetPos) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            } else {
                Logger.Send("Stop moving", "player");

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
        }
    }

    // Move in a given direction
    public void Move(string direction) {
        Vector2 raycastDirection = Vector2.zero;

        float xAdjustment = 0;
        float yAdjustment = 0;

        Logger.Send("move " + direction, "player");
        
        if (direction == "left") {
            raycastDirection = Vector2.left;
            xAdjustment = 1f;
        } else if (direction == "right") {
            raycastDirection = Vector2.right;
            xAdjustment = -1f;
        } else if (direction == "up") {
            raycastDirection = Vector2.up;
            yAdjustment = -1f;
        } else if (direction == "down") {
            raycastDirection = Vector2.down;
            yAdjustment = 1f;
        }

        // Nowhere to go
        if (raycastDirection == Vector2.zero) {
            Logger.Send("nowhere to go", "player");
            return;
        }

        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y);

        //Cast a ray in the direction specified in the inspector.
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection);

        Vector2 hitLocation = hit.transform.position;
        float hitDistance = Vector2.Distance(raycastOrigin, hitLocation);

        // If the hit is not an adjacent tile, then move towards it
        if (hitDistance > 1f) {
            Logger.Send("hit a distant tile, so move towards it", "player");

            targetPos = new Vector2(hitLocation.x + xAdjustment, hitLocation.y + yAdjustment);

            Logger.Send("updated target position to " + targetPos, "player");

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

            player.control.DisallowInput();
            player.interaction.CloseInteractNotice();
            
            foreach (Animator anim in anims) {
                Logger.Send("set player animation triggers", "player");
                anim.SetInteger("directionFacing", directionInt);
                anim.SetTrigger("startMoving");
                player.visual.MoveAfterAnimation();
                anim.SetBool("moving", true);
            }

            Logger.Send("updated animations", "player");

            if (shadowAnim != null && !lowLight) {
                Logger.Send("updated shadow animations", "player");
                shadowAnim.SetBool("lowLight", true);
            }
        } else {
            Logger.Send("hit adjacent tile, so just face that way", "player");
            FaceDirection(direction);

            // If the hit tile is interactable
            Interactable hitInteractable = (hit.transform.tag == "Interactable" ? hit.transform.gameObject.GetComponent<Interactable>() : null);

            if (hitInteractable != null) {
                hitInteractable.Interact();
            } else {
                // We are moving into a wall, shake the camera
                if (CameraShake.instance != null) {
                    AudioManager.instance.PlayOneShot("Movement Failed");
                    StartCoroutine(CameraShake.instance.Shake(.1f, .1f));
                }
            }
        }
    }

    // Face direction without moving
    public void FaceDirection(string direction) {
        int directionInt = 0;

        if (direction == "up") {
            directionInt = 1;
        } else if (direction == "right") {
            directionInt = 2;
        } else if (direction == "down") {
            directionInt = 3;
        } else if (direction == "left") {
            directionInt = 4;
        }

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

    public Vector3 TargetPos() {
        return targetPos;
    }
}
