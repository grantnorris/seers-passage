﻿using System.Collections;
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
        PlayerMove playerMove = playerInstance.GetComponent<PlayerMove>();
        PlayerControl playerControl = playerInstance.GetComponent<PlayerControl>();

        GameManager.instance.SetPlayer(playerInstance, playerMove, playerControl);
    }
}
