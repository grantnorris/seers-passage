using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [Tooltip("up, down, left or right – this affects the tile the player will appear on when returning through this door.")]
    public string direction;
    public DoorInteractable siblingDoor;

    public override void Interact() {
        Debug.Log("Interacted with door");

        if (siblingDoor == null || siblingDoor.direction == null) {
            Debug.LogWarning("No sibling door found for " + gameObject.name);
            return;
        }

        StartCoroutine(GameManager.instance.screenTransitions.TransitionDoor(this));
    }

    public string Direction() {
        return direction;
    }

    public Vector3 DirectionVector() {
        switch (direction)
        {
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

    public void MovePlayer() {
        Vector3 targetPos = siblingDoor.transform.position + siblingDoor.DirectionVector();
        Transform playerTransform = GameManager.instance.player.gameObject.transform;
        Transform camTransform = PlayerCamera.instance.transform;

        playerTransform.position = new Vector3(targetPos.x, targetPos.y, playerTransform.position.z);
        camTransform.position = new Vector3(targetPos.x, targetPos.y, camTransform.position.z);
        GameManager.instance.player.move.FaceDirection(siblingDoor.Direction());
    }
}
