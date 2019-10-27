using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool moving     = false;
    public bool allowInput = false;
    public Animator anim;
    public Animator shadowAnim;
    public GameObject interactNotice;
    public bool lowLight = true;

    Vector3 targetPos;
    PlayerInteractNotice interactNoticeScript;

    void Start() {
        allowInput = true;

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
                allowInput = true;
                moving     = false;

                if (shadowAnim != null && !lowLight) {
                    shadowAnim.SetBool("lowLight", false);
                }
            }
        } else if (allowInput) {
            if (Input.GetKeyDown("left")) {
                Move("left");
            } else if (Input.GetKeyDown("right")) {
                Move("right");
            } else if (Input.GetKeyDown("up")) {
                Move("up");
            } else if (Input.GetKeyDown("down")) {
                Move("down");
            }
        }
    }

    void Move(string direction) {
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

            if (anim != null) {
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

                allowInput = false;
                anim.SetInteger("directionFacing", directionInt);
                anim.SetTrigger("startMoving");
                anim.SetBool("moving", true);

                if (shadowAnim != null && !lowLight) {
                    shadowAnim.SetBool("lowLight", true);
                }
            } else {
                // If no animator just start moving
                moving = true;
            }
        } else if (anim != null) {
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
            }
        }
    }

    public void ExpandLightRadius() {
        lowLight = false;

        if (anim != null) {
            anim.SetBool("hasLight", true);
        }

        if (shadowAnim != null) {
            shadowAnim.SetBool("lowLight", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Interactable") {
            if (interactNoticeScript != null) {
                interactNoticeScript.Open();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Interactable") {
            if (interactNoticeScript != null) {
                interactNoticeScript.Close();
            }
        }
    }
}
