using UnityEngine;

public class DoorInteractable : Interactable
{
    [Tooltip("up, down, left or right - this affects the tile the player will appear on when returning through this door.")]
    public string direction;
    public DoorInteractable siblingDoor;

    public override void Interact() {
        if (siblingDoor == null || siblingDoor.direction == null) {
            Logger.Send($"No sibling door found for {gameObject.name}.", "general", "assertion");
            return;
        }

        StartCoroutine(GameManager.instance.screenTransitions.TransitionDoor(this));
    }

    // Retrieve the direction that the door faces
    public string Direction() {
        return direction;
    }

    // Retrieve a vector containing the offset related to the door direction
    public Vector3 DirectionVector() {
        switch (direction) {
            case "up":
                return new Vector3(0, 1, 0);
            case "right":
                return new Vector3(1, 0, 0);
            case "down":
                return new Vector3(0, -1, 0);
            case "left":
                return new Vector3(-1, 0, 0);
            default: 
                return Vector2.zero;
        }
    }

    // Move the player and camera to the other side of the connected door and update the player's facing direction
    public void MovePlayer() {
        Vector3 targetPos = siblingDoor.transform.position + siblingDoor.DirectionVector();
        Transform playerTransform = GameManager.instance.player.gameObject.transform;
        Transform camTransform = PlayerCamera.instance.transform;

        playerTransform.position = new Vector3(targetPos.x, targetPos.y, playerTransform.position.z);
        camTransform.position = new Vector3(targetPos.x, targetPos.y, camTransform.position.z);
        GameManager.instance.player.move.FaceDirection(siblingDoor.Direction());
    }
}
