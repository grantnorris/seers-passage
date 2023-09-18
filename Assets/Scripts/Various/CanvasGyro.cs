using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGyro : MonoBehaviour
{
    Gyroscope gyro;
    RectTransform rect;
    float movement = 50f;
    float maxOffset = 5f;
    float restingAttitude = 0f;
    float velocity;
    float curOffset = 0f;

    void Start() {
        gyro = Input.gyro;
        gyro.enabled = true;
        rect = GetComponent<RectTransform>();
    }
    
    void Update() {
        Normalise();
        SetOffset();
    }

    // Gradually recenters the UI based on the current attitude
    void Normalise() {
        restingAttitude = Mathf.SmoothDamp(restingAttitude, gyro.attitude.y * movement, ref velocity, .05f);
    }

    // Offset rect transform based on current attitude
    void SetOffset() {
        if (rect == null) {
            return;
        }

        float offset = gyro.attitude.y * movement;

        if (offset < 0 && restingAttitude < 0) {
            offset -= restingAttitude;
        } else {
            offset += restingAttitude;
        }

        curOffset = Mathf.SmoothDamp(curOffset, Mathf.Clamp(offset, -maxOffset, maxOffset), ref velocity, .15f);

        rect.offsetMin = new Vector2(curOffset, 0);
        rect.offsetMax = new Vector2(curOffset, 0);
    }
}
