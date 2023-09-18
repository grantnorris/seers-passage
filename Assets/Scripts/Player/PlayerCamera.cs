using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public float smoothSpeed = .15f;

    Transform player;
    Vector3 targetPos;
    Vector3 offset;
    Vector3 velocity = Vector3.zero;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        SetReferences();
    }

    // Set initial player and position references
    void SetReferences() {
        player = GameManager.instance.player.gameObject.transform;
        targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = targetPos;
    }

    // Smooth camera location in relation to player location/movement
    void LateUpdate() {
        if (player == null) {
            return;
        }
        
        targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset, ref velocity, smoothSpeed);
    }

    // Update camera offset
    public void UpdateOffset(Vector3 newOffset) {
        offset = newOffset;
    }
}
