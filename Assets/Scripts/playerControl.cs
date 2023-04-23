using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    bool allowInput;
    bool dragging;
    Vector2 dragStart;
    string moveDirection;
    Player player;
    DragUI dragUI;

    void Awake() {
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        allowInput = false;
        dragUI = GameManager.instance.dragUI;
    }

    // Update is called once per frame
    void Update()
    {
        TranslateInputs();
    }

    void TranslateInputs() {
        if (allowInput && !GameManager.instance.Paused()) {            
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
                PlayerCamera.instance.UpdateOffset(Vector3.zero);
                StopDragging();
                Logger.Send("Stop Dragging", "player");
            } else if (Input.GetKeyDown("left")) {
                player.move.Move("left");
            } else if (Input.GetKeyDown("right")) {
                player.move.Move("right");
            } else if (Input.GetKeyDown("up")) {
                player.move.Move("up");
            } else if (Input.GetKeyDown("down")) {
                player.move.Move("down");
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

        float minDrag = .25f;
        float dragThreshold = 1.5f;
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
            float camOffset = Mathf.Clamp(xDif / dragThreshold * (direction == "left" ? -1 : 1), -.25f, .25f);
            PlayerCamera.instance.UpdateOffset(new Vector3(camOffset, 0, 0));

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
            float camOffset = Mathf.Clamp(yDif / dragThreshold * (direction == "down" ? -1 : 1), -.25f, .25f);
            PlayerCamera.instance.UpdateOffset(new Vector3(0, camOffset, 0));

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
        player.move.Move(moveDirection);
        moveDirection = null;
    }

    public void AllowInput() {
        Logger.Send("allowed input", "player");
        allowInput = true;
        dragging = false;
    }

    public void DisallowInput() {
        Logger.Send("disallowed input", "player");
        allowInput = false;
        dragging = false;
    }
}
