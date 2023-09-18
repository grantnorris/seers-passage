using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public GameObject renderTexObj;
    public Camera playerCam;
    Camera mainCam;
    RenderTexture renderTex;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        Initialise();
    }

    public void Initialise() {
        mainCam = Camera.main;
        GameObject player = GameManager.instance.player.gameObject;

        if (player == null) {
            Logger.Send("No player set in game manager.", "general", "assertion");
            return;
        }

        if (playerCam != null) {
            renderTex = playerCam.targetTexture;
        }

        SetUpPlayerCam();
    }

    // Set up player camera and render texture
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
