using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    bool allowInput;
    bool dragging;
    Touch dragStart;
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
        Vector2 startPos = dragStart.position;
        Vector2 dragPos = Input.touches[0].position;

        float xDif = Mathf.Abs(startPos.x - dragPos.x);
        float yDif = Mathf.Abs(startPos.y - dragPos.y);

        if (xDif > yDif) {
            if (startPos.x > dragPos.x) {
                Debug.Log("dragging left");
            } else {
                Debug.Log("dragging right");
            }
        } else {
            if (startPos.y > dragPos.y) {
                Debug.Log("dragging down");
            } else {
                Debug.Log("dragging up");
            }
        }
    }

    void StartDragging() {
        dragging = true;
        dragStart = Input.touches[0];
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
