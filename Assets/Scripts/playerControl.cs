using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    bool allowInput;
    bool dragging;
    Vector2 dragStart;
    PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        allowInput = true;
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
            } else if (Input.GetKeyDown("left")) {
                playerMove.Move("left");
            } else if (Input.GetKeyDown("right")) {
                playerMove.Move("right");
            } else if (Input.GetKeyDown("up")) {
                playerMove.Move("up");
            } else if (Input.GetKeyDown("down")) {
                playerMove.Move("down");
            } else {
                dragging = false;
            }
        }
    }

    void Drag() {
        Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        float dragThreshold = 1f;

        float xDif = Mathf.Abs(dragStart.x - dragPos.x);
        float yDif = Mathf.Abs(dragStart.y - dragPos.y);
        string direction;

        if (xDif <= dragThreshold && yDif <= dragThreshold) {
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
        } else {
            if (dragStart.y > dragPos.y) {
                // Dragging down
                direction = "down";
            } else {
                // Dragging top
                direction = "up";
            }
        }

        playerMove.Move(direction);
    }

    void StartDragging() {
        dragging = true;
        dragStart = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
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
