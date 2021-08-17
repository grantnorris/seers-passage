using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float smoothSpeed = .15f;

    Transform player;
    Vector3 targetPos;
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.playerSet.AddListener(Init);
    }

    void Init() {
        player = GameManager.instance.player.transform;
        targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = targetPos;
    }

    void LateUpdate()
    {
        if (player == null) {
            return;
        }
        
        targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothSpeed);
    }
}
