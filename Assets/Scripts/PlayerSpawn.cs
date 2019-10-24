using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn instance;

    public GameObject player;

    void Awake() {
        if (instance == null) {
            instance = this;

            if (player != null) {
                GameObject playerInstance = Instantiate(player, transform.position, Quaternion.identity);
                GameManager.instance.player = playerInstance;

                PlayerControl playerControl = playerInstance.GetComponent<PlayerControl>();

                if (playerControl != null) {
                    GameManager.instance.playerControl = playerControl;
                }
            }
        }
    }
}
