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
    public GameObject heartbreakUI;
    [SerializeField]
    GameObject loseUI;
    [SerializeField]
    Color perfectStepColor;
    [SerializeField]
    Color goodStepColor;
    [SerializeField]
    Color badStepColor;
    [SerializeField]
    PauseUI pauseUI;

    void Start() {
        Initialise();
    }

    // Initialise by setting player step count and UI active states
    void Initialise() {
        playerStepCountTxt.text = GameManager.instance.player.steps.StepCount().ToString();

        if (introUI != null) {
            introUI.SetActive(true);

            if (gameUI != null) {
                gameUI.SetActive(false);
            }
        }

        SetFloorNameUI();
    }

    // Display game UI object
    public void DisplayGameUI() {
        if (gameUI != null) {
            gameUI.SetActive(true);
        }
    }

    // Set floor name text in game UI
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

        int steps = GameManager.instance.player.steps.StepCount();
        RectTransform txtRect = playerStepCountTxt.GetComponent<RectTransform>();
        Vector3 txtRectPos = txtRect.position;
        GameObject tempTxtObj = Instantiate(playerStepCountTxt.gameObject, playerStepCountTxt.transform.parent);
        RectTransform tempTxtRect = tempTxtObj.GetComponent<RectTransform>();
        TMP_Text tempTxt = tempTxtObj.GetComponent<TMP_Text>();
        tempTxt.text = steps.ToString();
        Color txtColor = perfectStepColor;
        float time = 0f;
        float seconds = .25f;
        string stepScore = GameManager.instance.player.steps.StepScore();

        if (stepScore == "Good") {
            // Orange
            txtColor = goodStepColor;
        } else if (stepScore == "Bad") {
            // Red
            txtColor = badStepColor;
        }

        // Animate number roll over 1 second
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

    // Display the beginning of the heart break animation
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

        // Animate UI over 1 second
        while (time <= 1f) {
            time += Time.deltaTime / seconds;
            uiRect.sizeDelta = Vector2.Lerp(Vector2.zero, uiRectSize, time);
            yield return null;
        }

        uiRect.sizeDelta = uiRectSize;
        DialogueManager.instance.dialogueEnded.AddListener(EndBreakHeart);
    }

    // Display the end of heart break animation
    public void EndBreakHeart() {
        DialogueManager.instance.dialogueEnded.RemoveListener(EndBreakHeart);
        StartCoroutine("EndBreakHeartAnimation");
    }

    IEnumerator EndBreakHeartAnimation() {
        if (heartbreakUI != null) {
            heartbreakUI.GetComponent<Animator>().SetTrigger("Break");

            yield return new WaitForSeconds(.2f);

            AudioManager.instance.PlayOneShot("Heart Break");

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
        
        GameManager.instance.player.health.ReduceHealth();
        HealthUI.instance.UpdateUI();

        if (GameManager.instance.player.health.Health() == 1) {
            playerStepCountTxt.GetComponent<Animator>().SetBool("Flash", true);
        }

        if (GameManager.instance.player.health.Health() > 0) {
            GameManager.instance.EnablePlayerMove();
        }
    }

    // Display a dialogue popup based on the current number of lives/steps
    public void StepCountDialogue() {
        Dialogue dialogue = null;

        switch (GameManager.instance.player.steps.currentStepThreshold()){
            case 1:
                dialogue = new Dialogue(new string[] {"An uneasy presence washes over you."});
                break;
            case 2:
                dialogue = new Dialogue(new string[] {"A pressuring presence weighs on you."});
                break;
            case 3:
                dialogue = new Dialogue(new string[] {"A suffocating presence consumes you."});
                break;
        }

        DialogueManager.instance.StartDialogue(dialogue);
    }

    // Display the win state UI after level completion
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

    // Display the lose state after level loss
    public void DisplayLoseUI() {
        StartCoroutine("TransitionLoseUI");
    }

    IEnumerator TransitionLoseUI() {
        gameUI.GetComponent<Animator>().SetTrigger("TransitionOut");
        yield return new WaitForSeconds(.5f);
        loseUI.SetActive(true);
    }

    // Display pause UI
    public void DisplayPauseUI() {
        if (pauseUI == null) {
            return;
        }

        pauseUI.TransitionIn();
    }

    // Hide pause UI
    public void HidePauseUI() {
        if (pauseUI == null) {
            return;
        }

        pauseUI.TransitionOut();
    }
}
