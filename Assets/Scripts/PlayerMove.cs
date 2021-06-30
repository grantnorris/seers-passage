using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool moving     = false;
    public Animator anim;
    public Animator shadowAnim;
    public GameObject interactNotice;
    public bool lowLight = true;
    [HideInInspector]
    public UnityEvent finishMoving = new UnityEvent();

    PlayerControl playerControl;
    Vector3 targetPos;
    PlayerInteractNotice interactNoticeScript;

    void Start() {
        playerControl = GetComponent<PlayerControl>();
        
        if (GameManager.instance != null) {
            finishMoving.AddListener(GameManager.instance.IncrementStepCount);
        }

        if (DialogueTrigger.instance != null && DialogueTrigger.instance.stepTriggers.Length > 0) {
            finishMoving.AddListener(DialogueTrigger.instance.StepTriggers);
        }

        if (shadowAnim != null) {
            shadowAnim.SetBool("lowLight", true);
        }

        if (interactNotice != null) {
            interactNoticeScript = interactNotice.GetComponent<PlayerInteractNotice>();
        }
    }

    // Update is called once per frame
    void Update()
    {  
        if (moving) {
            if (transform.position != targetPos) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            } else {
                anim.SetTrigger("stopMoving");
                playerControl.AllowInput();
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
            return;
        }

        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y);

        //Cast a ray in the direction specified in the inspector.
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection);

        Vector2 hitLocation = hit.transform.position;
        float hitDistance = Vector2.Distance(raycastOrigin, hitLocation);

        // If the hit is not an adjacent tile, then move towards it
        if (hitDistance > 1f) {
            targetPos = new Vector2(hitLocation.x + xAdjustment, hitLocation.y + yAdjustment);

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

            playerControl.DisallowInput();
            anim.SetInteger("directionFacing", directionInt);
            anim.SetTrigger("startMoving");
            anim.SetBool("moving", true);

            if (shadowAnim != null && !lowLight) {
                shadowAnim.SetBool("lowLight", true);
            }
        } else {
            // Face the direction without moving
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

            anim.SetInteger("directionFacing", directionInt);

            // If the hit tile is interactable
            Interactable hitInteractable = hit.transform.gameObject.GetComponent<Interactable>();

            if (hitInteractable != null) {
                hitInteractable.Interact();
            } else {
                // We are moving into a wall, shake the camera
                if (CameraShake.instance != null) {
                    GameManager.instance.audioManager.Play("Movement Failed");
                    StartCoroutine(CameraShake.instance.Shake(.1f, .1f));
                }
            }
        }
    }

    // Expand the light radius of the player
    public void ExpandLightRadius() {
        lowLight = false;

        if (anim != null) {
            anim.SetBool("hasLight", true);
        }

        if (shadowAnim != null) {
            shadowAnim.SetBool("lowLight", false);
        }
    }

    // Display interation notice on trigger enter
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Interactable") {
            OpenInteractNotice();
        }
    }

    // Hide interation notice on trigger enter
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Interactable") {
            CloseInteractNotice();
        }
    }

    // Open interaction notice
    public void OpenInteractNotice() {
        if (interactNoticeScript != null) {
            interactNoticeScript.Open();
        }
    }

    // Close interaction notice
    public void CloseInteractNotice() {
        if (interactNoticeScript != null) {
            interactNoticeScript.Close();
        }
    }
}
