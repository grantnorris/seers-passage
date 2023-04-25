using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn instance;
    public GameObject player;
    [Tooltip("up, down, left or right.\nDefaults to down.")]
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
        Player player = GameManager.instance.player;
        player.SetInitialPosition(transform.position);

        if (initialDirection == "up" || initialDirection == "left" || initialDirection == "right") {
            player.move.FaceDirection(initialDirection);
        }
    }
}
