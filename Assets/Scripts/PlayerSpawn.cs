using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn instance;
    public GameObject player;
    [Tooltip("up,down,left or right.\nDefaults to down.")]
    public string initialDirection;

    void Start() {
        if (instance != null) {
            return;
        }

        instance = this;

        CreatePlayer();
    }

    // Create player gameobject
    void CreatePlayer() {
        GameObject playerInstance = Instantiate(player, transform.position, Quaternion.identity);
        PlayerMove playerMove = playerInstance.GetComponent<PlayerMove>();
        PlayerControl playerControl = playerInstance.GetComponent<PlayerControl>();
        GameManager.instance.SetPlayer(playerInstance, playerMove, playerControl);

        if (initialDirection == "up" || initialDirection == "left" || initialDirection == "right") {
            playerMove.FaceDirection(initialDirection);
        }
    }
}
