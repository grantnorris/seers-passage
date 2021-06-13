using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject renderTexObj;
    Camera playerCam;
    Camera mainCam;
    RenderTexture renderTex;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        playerCam = GameManager.instance.player.GetComponentInChildren<Camera>();

        if (playerCam != null) {
            renderTex = playerCam.targetTexture;
        }

        SetUpPlayerCam();
    }

    // Set up player cam and render texture
    void SetUpPlayerCam() {
        if (mainCam == null || playerCam == null || renderTex == null || renderTexObj == null) {
            return;
        }

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        float aspectRatio = (float)screenWidth / (float)screenHeight;
        float worldUnitScreenHeight = mainCam.orthographicSize * 2.0f;
        float wordUnitScreenWidth = worldUnitScreenHeight * aspectRatio;
        playerCam.orthographicSize = mainCam.orthographicSize * .75f;
        playerCam.aspect = aspectRatio;
        renderTex.width = screenWidth;
        renderTex.height = screenHeight;
        renderTexObj.transform.localScale = new Vector3(wordUnitScreenWidth, worldUnitScreenHeight, 1);
    }
}
