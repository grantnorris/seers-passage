using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originPos = transform.position;
        float elapsed     = 0f;

        while (duration > elapsed) {
            float xPos = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originPos.x + xPos, originPos.y, originPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originPos;
    }
}
