using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject renderTexObj;
    Camera playerCam;
    Camera mainCam;
    RenderTexture renderTex;

    void update() {
        Debug.Log("orth size main = " + mainCam.orthographicSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        playerCam = GameManager.instance.player.GetComponentInChildren<Camera>();

        if (playerCam != null) {
            renderTex = playerCam.targetTexture;
        }

        StartCoroutine("SetUpPlayerCam");
    }

    // Set up player cam and render texture
    IEnumerator SetUpPlayerCam() {
        Debug.Log("setup player cam");

        yield return null;

        if (mainCam == null || playerCam == null || renderTex == null || renderTexObj == null) {
            yield break;
        }

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        float aspectRatio = (float)screenWidth / (float)screenHeight;
        playerCam.orthographicSize = mainCam.orthographicSize * .75f;
        playerCam.aspect = aspectRatio;
        renderTex.width = screenWidth;
        renderTex.height = screenHeight;

        float worldUnitScreenHeight = mainCam.orthographicSize * 2.0f;
        float wordUnitScreenWidth = worldUnitScreenHeight * aspectRatio;

        renderTexObj.transform.localScale = new Vector3(wordUnitScreenWidth, worldUnitScreenHeight, 1);

        Debug.Log("orth size main = " + mainCam.orthographicSize);
    }
}
