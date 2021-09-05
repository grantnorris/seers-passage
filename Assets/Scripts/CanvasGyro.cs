using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGyro : MonoBehaviour
{
    Gyroscope gyro;
    RectTransform rect;
    float movement = 100f;

    // Start is called before the first frame update
    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rect == null) {
            return;
        }

        rect.offsetMin = new Vector2(gyro.attitude.y * movement, gyro.attitude.x * movement);

        Debug.Log("input.gyro.attitude: " + gyro.attitude);

    }
}
