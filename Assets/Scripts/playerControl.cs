using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    bool allowInput;
    PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        allowInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput && GameManager.instance.playerControllable) {
            if (Input.GetKeyDown("left")) {
                playerMove.Move("left");
            } else if (Input.GetKeyDown("right")) {
                playerMove.Move("right");
            } else if (Input.GetKeyDown("up")) {
                playerMove.Move("up");
            } else if (Input.GetKeyDown("down")) {
                playerMove.Move("down");
            }
        }
    }

    public void AllowInput() {
        allowInput = true;
    }

    public void DisallowInput() {
        allowInput = false;
    }
}
