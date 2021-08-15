using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    bool allowInput;
    bool dragging;
    Vector2 dragStart;
    string moveDirection;
    PlayerMove playerMove;
    DragUI dragUI;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        allowInput = false;
        dragUI = GameManager.instance.dragUI;
    }

    // Update is called once per frame
    void Update()
    {
        TranslateInputs();
    }

    void TranslateInputs() {
        if (allowInput) {
            if (Input.touchCount > 0) {
                if (dragging) {
                    Drag();
                } else {
                    StartDragging();
                }
            } else if (Input.GetMouseButton(0)) {
                if (dragging) {
                    Drag();
                } else {
                    StartDragging();
                }
            } else if (dragging) {
                StopDragging();
            } else if (Input.GetKeyDown("left")) {
                playerMove.Move("left");
            } else if (Input.GetKeyDown("right")) {
                playerMove.Move("right");
            } else if (Input.GetKeyDown("up")) {
                playerMove.Move("up");
            } else if (Input.GetKeyDown("down")) {
                playerMove.Move("down");
            }
        }
    }

    void Drag() {
        Vector2 dragPos = Vector2.zero;

        if (Input.touchCount > 0) {
            dragPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        } else {
            dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (dragPos == Vector2.zero) {
            return;
        }

        float minDrag = .5f;
        float dragThreshold = 3f;
        float xDif = Mathf.Abs(dragStart.x - dragPos.x);
        float yDif = Mathf.Abs(dragStart.y - dragPos.y);
        string direction;

        if (xDif < minDrag && yDif < minDrag) {
            dragUI.Reset();
            return;
        }

        if (xDif > yDif) {
            if (dragStart.x > dragPos.x) {
                // Dragging left
                direction = "left";
            } else {
                // Dragging right
                direction = "right";
            }

            dragUI.Display(xDif / dragThreshold, dragStart, direction);

            if (xDif <= dragThreshold) {
                moveDirection = null;
                return;
            }
        } else {
            if (dragStart.y > dragPos.y) {
                // Dragging down
                direction = "down";
            } else {
                // Dragging top
                direction = "up";
            }

            dragUI.Display(yDif / dragThreshold, dragStart, direction);

            if (yDif <= dragThreshold) {
                moveDirection = null;
                return;
            }
        }

        moveDirection = direction;
    }

    void StartDragging() {
        dragging = true;

        if (Input.touchCount > 0) {
            dragStart = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        } else {
            dragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void StopDragging() {
        dragging = false;

        if (moveDirection == null) {
            GameManager.instance.dragUI.Reset();
            return;
        }

        dragUI.ApplyDirection(moveDirection);
        playerMove.Move(moveDirection);
        moveDirection = null;
    }

    public void AllowInput() {
        allowInput = true;
        dragging = false;
    }

    public void DisallowInput() {
        allowInput = false;
        dragging = false;
    }
}
