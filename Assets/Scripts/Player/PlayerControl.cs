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

    void Start() {
        allowInput = false;
        dragUI = GameManager.instance.dragUI;
    }

    void Update() {
        TranslateInputs();
    }

    // Translate player inputs
    // Keycode inputs are also supported for dev
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
                Logger.Send("Stop Dragging.", "player");
            } else if (Input.GetKeyDown("left")) {
                player.move.MoveInDirection("left");
            } else if (Input.GetKeyDown("right")) {
                player.move.MoveInDirection("right");
            } else if (Input.GetKeyDown("up")) {
                player.move.MoveInDirection("up");
            } else if (Input.GetKeyDown("down")) {
                player.move.MoveInDirection("down");
            }
        }
    }

    // Update UI, player sprite and direction references based on player drag input
    void Drag() {
        Vector2 dragPos = CurrentDragLocation();

        if (dragPos == Vector2.zero) {
            return;
        }

        float minDrag = .25f;
        float dragThreshold = 1.5f;
        float xDistance = Mathf.Abs(dragStart.x - dragPos.x);
        float yDistance = Mathf.Abs(dragStart.y - dragPos.y);
        string direction;
        Vector3 cameraOffset;
        float maxCameraOffset = .25f;
        float distance = xDistance > yDistance ? xDistance : yDistance;

        if (xDistance < minDrag && yDistance < minDrag) {
            // Reset drag UI if dragging a minimal amount
            dragUI.Reset();
            return;
        }

        if (xDistance > yDistance) {
            direction = dragStart.x > dragPos.x ? "left" : "right";
            cameraOffset = new Vector3(
                Mathf.Clamp(distance / dragThreshold * (direction == "left" ? -1 : 1), -maxCameraOffset, maxCameraOffset),
                0,
                0
            );
        } else {
            direction = dragStart.y > dragPos.y ? "down" : "up";
            cameraOffset = new Vector3(
                0,
                Mathf.Clamp(distance / dragThreshold * (direction == "down" ? -1 : 1), -maxCameraOffset, maxCameraOffset),
                0
            );
        }

        player.move.FaceDirection(direction);
        dragUI.Display(distance / dragThreshold, dragStart, direction);
        PlayerCamera.instance.UpdateOffset(cameraOffset);

        if (distance <= dragThreshold) {
            moveDirection = null;
            return;
        }

        // Set the move direction that should be applied if the player releases the drag
        moveDirection = direction;
    }

    // Set beginning drag location reference
    void StartDragging() {
        dragging = true;
        dragStart = CurrentDragLocation();
    }

    // Get the current location of the drag
    Vector2 CurrentDragLocation() {
        if (Input.touchCount > 0) {
            // Is touch
            return Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        } else {
            // Is mouse (dev)
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    // Reset dragging UI and references, move the player if the input dragged far enough
    void StopDragging() {
        dragging = false;

        if (moveDirection == null) {
            GameManager.instance.dragUI.Reset();
            return;
        }

        dragUI.ApplyDirection(moveDirection);
        player.move.MoveInDirection(moveDirection);
        moveDirection = null;
    }

    // Allow new inputs to be made
    public void AllowInput() {
        Logger.Send("Allowed input.", "player");
        allowInput = true;
        dragging = false;
    }

    // Stop allowing new inputs
    public void DisallowInput() {
        Logger.Send("Disallowed input.", "player");
        allowInput = false;
        dragging = false;
    }
}
