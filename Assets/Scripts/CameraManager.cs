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
        GameManager.instance.playerSet.AddListener(Init);
    }

    public void Init()
    {
        mainCam = Camera.main;
        GameObject player = GameManager.instance.player;

        if (player == null) {
            Debug.LogWarning("No player set in gamemanager");
            return;
        }

        // playerCam = player.GetComponentInChildren<Camera>();

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
