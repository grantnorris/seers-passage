using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text playerStepCountTxt;
    public GameObject player;
    public PlayerControl playerControl;

    int playerStepCount = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void IncrementStepCount() {
        playerStepCount++;

        if (playerStepCountTxt != null) {
            playerStepCountTxt.text = playerStepCount.ToString();
        }
    }
}
