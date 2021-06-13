using System.Collections;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public CanvasGroup headUI;
    public CanvasGroup footerUI;
    public TMP_Text playerStepCountTxt;

    void Start() {
        Init();
    }

    void Init() {
        if (headUI == null || footerUI == null) {
            return;
        }

        headUI.alpha = 0f;
        footerUI.alpha = 0f;
        GameManager.instance.levelStart.AddListener(StartTransitionInUi);
        GameManager.instance.stepped.AddListener(StartUpdateStepCountUI);
    }

    // Start the TransitionInUI coroutine
    void StartTransitionInUi() {
        StartCoroutine("TransitionInUI");
    }

    // Transition UI In
    public IEnumerator TransitionInUI() {
        if (headUI == null || footerUI == null) {
            yield break;
        }

        float progress = 0f;
        float speed = 2f;

        headUI.alpha = 0f;
        footerUI.alpha = 0f;

        while (progress < 1f) {
            progress += Time.deltaTime * speed;
            headUI.alpha = progress;
            footerUI.alpha = progress;
            yield return null;
        }

        headUI.alpha = 1f;
        footerUI.alpha = 1f;
    }

    // Start UpdateStepCountUI coroutine
    public void StartUpdateStepCountUI() {
        StartCoroutine("UpdateStepCountUI");
    }

    // Update step count UI with number roll
    IEnumerator UpdateStepCountUI() {
        if (playerStepCountTxt == null) {
            yield break;
        }

        string steps = GameManager.instance.StepCount().ToString();
        RectTransform txtRect = playerStepCountTxt.GetComponent<RectTransform>();
        Vector3 txtRectPos = txtRect.position;
        GameObject tempTxtObj = Instantiate(playerStepCountTxt.gameObject, footerUI.transform);
        RectTransform tempTxtRect = tempTxtObj.GetComponent<RectTransform>();
        TMP_Text tempTxt = tempTxtObj.GetComponent<TMP_Text>();
        tempTxt.text = steps;
        Color txtColor = playerStepCountTxt.color;
        float time = 0f;
        float seconds = .25f;

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float smoothTime = Mathf.SmoothStep(0f, 1f, time);
            txtRect.position = Vector3.Lerp(txtRectPos, txtRectPos - new Vector3(0, .5f, 0), smoothTime);
            tempTxtRect.position = Vector3.Lerp(txtRectPos + new Vector3(0, .5f, 0), txtRectPos, smoothTime);
            tempTxt.color = Color.Lerp(new Color(txtColor.r, txtColor.r, txtColor.g, 0), txtColor, smoothTime);
            playerStepCountTxt.color = Color.Lerp(txtColor, new Color(txtColor.r, txtColor.r, txtColor.g, 0), smoothTime);
            yield return null;
        }
        
        Destroy(tempTxt);
        playerStepCountTxt.text = steps;
        txtRect.position = txtRectPos;
        playerStepCountTxt.color = txtColor;
    }
}
