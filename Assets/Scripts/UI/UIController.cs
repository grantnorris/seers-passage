using System.Collections;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject UI;
    [SerializeField]
    GameObject introUI;
    [SerializeField]
    GameObject gameUI;
    [SerializeField]
    GameObject outroUI;
    [SerializeField]
    TMP_Text playerStepCountTxt;
    [SerializeField]
    GameObject heartbreakPrefab;
    [SerializeField]
    TMP_Text floorNameUI;
    [SerializeField]
    GameObject healthUI;
    GameObject heartbreakUI;
    [SerializeField]
    GameObject loseUI;
    [SerializeField]
    Color perfectStepColor;
    [SerializeField]
    Color goodStepColor;
    [SerializeField]
    Color badStepColor;
    [SerializeField]
    GameObject PauseUI;

    void Start() {
        Init();
    }

    void Init() {
        playerStepCountTxt.text = "0";
        GameManager.instance.stepped.AddListener(StartUpdateStepCountUI);

        if (introUI != null) {
            introUI.SetActive(true);

            if (gameUI != null) {
                gameUI.SetActive(false);
            }
        }

        SetFloorNameUI();
    }

    public void DisplayGameUI() {
        if (gameUI != null) {
            gameUI.SetActive(true);
        }
    }

    void SetFloorNameUI() {
        if (floorNameUI == null || SceneSwitcher.instance == null) {
            return;
        }

        floorNameUI.SetText(SceneSwitcher.instance.level.name);
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

        int steps = GameManager.instance.StepCount();
        RectTransform txtRect = playerStepCountTxt.GetComponent<RectTransform>();
        Vector3 txtRectPos = txtRect.position;
        GameObject tempTxtObj = Instantiate(playerStepCountTxt.gameObject, playerStepCountTxt.transform.parent);
        RectTransform tempTxtRect = tempTxtObj.GetComponent<RectTransform>();
        TMP_Text tempTxt = tempTxtObj.GetComponent<TMP_Text>();
        tempTxt.text = steps.ToString();
        Color txtColor = perfectStepColor;
        float time = 0f;
        float seconds = .25f;
        string stepScore = GameManager.instance.StepScore();

        if (stepScore == "Good") {
            // Orange
            txtColor = goodStepColor;
        } else if (stepScore == "Bad") {
            // Red
            txtColor = badStepColor;
        }

        while (time <= 1f) {
            time += Time.unscaledDeltaTime / seconds;
            float smoothTime = Mathf.SmoothStep(0f, 1f, time);
            txtRect.position = Vector3.Lerp(txtRectPos, txtRectPos - new Vector3(0, .5f, 0), smoothTime);
            tempTxtRect.position = Vector3.Lerp(txtRectPos + new Vector3(0, .5f, 0), txtRectPos, smoothTime);
            tempTxt.color = Color.Lerp(new Color(txtColor.r, txtColor.g, txtColor.b, 0), txtColor, smoothTime);
            playerStepCountTxt.color = Color.Lerp(playerStepCountTxt.color, new Color(playerStepCountTxt.color.r, playerStepCountTxt.color.g, playerStepCountTxt.color.b, 0), smoothTime);
            yield return null;
        }
        
        Destroy(tempTxt.gameObject);
        playerStepCountTxt.text = steps.ToString();
        txtRect.position = txtRectPos;
        playerStepCountTxt.color = txtColor;
    }

    // Display heartbreak animation
    public void StartBreakHeart() {
        if (heartbreakPrefab == null) {
            return;
        }

        StartCoroutine("StartBreakHeartAnimation");
    }

    IEnumerator StartBreakHeartAnimation() {
        StepCountDialogue();
        heartbreakUI = Instantiate(heartbreakPrefab, UI.transform);

        RectTransform uiRect = heartbreakUI.GetComponent<RectTransform>();
        Vector2 uiRectSize = uiRect.sizeDelta;
        uiRect.sizeDelta = Vector2.zero;

        float time = 0f;
        float seconds = .25f;

        // Animate ui
        while (time <= 1f) {
            time += Time.deltaTime / seconds;
            uiRect.sizeDelta = Vector2.Lerp(Vector2.zero, uiRectSize, time);
            yield return null;
        }

        uiRect.sizeDelta = uiRectSize;
        DialogueManager.instance.dialogueEnded.AddListener(EndBreakHeart);
    }

    public void EndBreakHeart() {
        DialogueManager.instance.dialogueEnded.RemoveListener(EndBreakHeart);
        StartCoroutine("EndBreakHeartAnimation");
    }

    IEnumerator EndBreakHeartAnimation() {
        if (heartbreakUI != null) {
            heartbreakUI.GetComponent<Animator>().SetTrigger("Break");

            yield return new WaitForSeconds(.2f);

            GameManager.instance.audioManager.Play("Player Hurt");

            yield return new WaitForSeconds(.9f);

            RectTransform uiRect = heartbreakUI.GetComponent<RectTransform>();
            float time = 0f;
            float seconds = .25f;

            // Animate ui
            while (time <= 1f) {
                time += Time.deltaTime / seconds;
                uiRect.sizeDelta = Vector2.Lerp(uiRect.sizeDelta, Vector2.zero, time);
                yield return null;
            }

            Destroy(heartbreakUI);
        }
        
        GameManager.instance.playerHealth.ReduceHealth();
        HealthUI.instance.UpdateUI();
    }

    public void StepCountDialogue() {
        int stepThreshold = GameManager.instance.stepThreshold;
        int goodSteps = stepThreshold * 2;
        int badSteps = stepThreshold * 3;
        int stepCount = GameManager.instance.StepCount();
        
        if (stepCount == stepThreshold) {
            Dialogue dialogue = new Dialogue(new string[] {"An uneasy presence washes over you."});
            DialogueManager.instance.StartDialogue(dialogue);
        } else if (stepCount == goodSteps) {
            Dialogue dialogue = new Dialogue(new string[] {"A pressuring presence weighs on you."});
            DialogueManager.instance.StartDialogue(dialogue);
        } else if (stepCount == badSteps) {
            Dialogue dialogue = new Dialogue(new string[] {"A suffocating presence consumes you."});
            DialogueManager.instance.StartDialogue(dialogue);
        }
    }

    public void DisplayOutroCard() {
        StartCoroutine("TransitionToOutro");
    }

    IEnumerator TransitionToOutro() {
        yield return new WaitForSeconds(2.2f);
        gameUI.GetComponent<Animator>().SetTrigger("TransitionOut");
        yield return new WaitForSeconds(.35f);
        gameUI.SetActive(false);
        outroUI.SetActive(true);
    }

    public void DisplayLoseUI() {
        StartCoroutine("TransitionLoseUI");
    }

    IEnumerator TransitionLoseUI() {
        gameUI.GetComponent<Animator>().SetTrigger("TransitionOut");
        yield return new WaitForSeconds(.5f);
        loseUI.SetActive(true);
    }

    public void DisplayPauseUI() {
        PauseUI.SetActive(true);
    }

    public void HidePauseUI() {
        PauseUI.SetActive(false);
    }
}
