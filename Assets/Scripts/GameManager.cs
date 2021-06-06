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
    public bool playerControllable = false;

    int playerStepCount = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    // Increase the player's step count by 1
    public void IncrementStepCount() {
        playerStepCount++;

        if (playerStepCountTxt != null) {
            playerStepCountTxt.text = playerStepCount.ToString();
        }
    }

    // Disable player inputs for a given durations
    public IEnumerator DisablePlayerControlForDuration(float duration) {
        if (duration > 0f) {
            float elapsed = 0f;

            while (duration > elapsed) { 
                elapsed += Time.deltaTime;
                DisablePlayerControl();

                yield return null;
            }

            EnablePlayerControl();
        }
    }

    // Disable player inputs
    public void DisablePlayerControl() {
        playerControllable = false;
    }

    // Enable player inputs
    public void EnablePlayerControl() {
        playerControllable = true;
    }
}
