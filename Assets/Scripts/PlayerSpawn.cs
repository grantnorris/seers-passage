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
                Instantiate(player, transform.position, Quaternion.identity);
            }
        }
    }
}
