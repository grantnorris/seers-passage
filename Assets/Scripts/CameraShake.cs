using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public Material lofiShader;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originPos = transform.position;
        float elapsed     = 0f;

        Vector2 chromaticAberration = lofiShader.GetVector("Vector2_6B378F0B");

        if (lofiShader != null) {
            lofiShader.SetVector("Vector2_6B378F0B", new Vector2(.0025f, 0));
        }

        while (duration > elapsed) {
            float xPos = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originPos.x + xPos, originPos.y, originPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        if (lofiShader != null) {
            lofiShader.SetVector("Vector2_6B378F0B", chromaticAberration);
        }

        transform.localPosition = originPos;
    }
}
