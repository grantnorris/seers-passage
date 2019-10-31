using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originPos = transform.position;

        float elapsed = 0f;

        while (duration > elapsed) {
            elapsed += Time.deltaTime;

            float xPos = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originPos.x + xPos, originPos.y, originPos.z);

            Debug.Log("shake camera");

            yield return null;
        }

        transform.localPosition = originPos;
    }
}
