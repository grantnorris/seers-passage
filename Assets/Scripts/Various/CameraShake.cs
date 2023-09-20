using System.Collections;
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

    void Start() {
        // Ensure chromatic aberration amount is reset to zero
        UpdateChromaticAberration(Vector2.zero);
    }

    // Update chromatic aberration value in render texture shader
    void UpdateChromaticAberration(Vector2 value) {
        if (lofiShader != null) {
            lofiShader.SetVector("ChromaticAberration", value);
        }
    }

    // Shake the camera by a given amount for a specified duration
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originPos = transform.position;
        float elapsed = 0f;

        UpdateChromaticAberration(new Vector2(.0025f, 0));

        while (duration > elapsed) {
            float xPos = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originPos.x + xPos, originPos.y, originPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        UpdateChromaticAberration(Vector2.zero);

        transform.localPosition = originPos;
    }
}
