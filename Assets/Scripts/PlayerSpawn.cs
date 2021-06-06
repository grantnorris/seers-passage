using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn instance;
    public GameObject player;

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
        GameManager.instance.player = playerInstance;

        PlayerControl playerControl = playerInstance.GetComponent<PlayerControl>();

        if (playerControl != null) {
            GameManager.instance.playerControl = playerControl;
        }
    }
}
