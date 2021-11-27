using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl control;
    public PlayerMove move;
    public PlayerSteps steps;

    public void SetInitialPosition(Vector3 pos) {
        transform.position = pos;
    }
}
