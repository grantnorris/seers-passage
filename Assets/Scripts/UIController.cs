using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject UI;
    public CanvasGroup headUI;
    public CanvasGroup footerUI;
    public TMP_Text playerStepCountTxt;
    public GameObject heartbreakPrefab;
    public GameObject healthUI;
    public GameObject torchUI;
    
    GameObject heartbreakUI;

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
        float speed = 4f;

        // Animate ui
        while (time <= 1f) {
            time += Time.deltaTime * speed;
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
            yield return new WaitForSeconds(1.1f);

            RectTransform uiRect = heartbreakUI.GetComponent<RectTransform>();
            float time = 0f;
            float speed = 4f;

            // Animate ui
            while (time <= 1f) {
                time += Time.deltaTime * speed;
                uiRect.sizeDelta = Vector2.Lerp(uiRect.sizeDelta, Vector2.zero, time);
                yield return null;
            }

            Destroy(heartbreakUI);
        }
        
        UpdateHealthUI();
    }

    public void StepCountDialogue() {
        int perfectSteps = 7;
        int goodSteps = perfectSteps * 2;
        int badSteps = perfectSteps * 3;
        int stepCount = GameManager.instance.StepCount();
        
        if (stepCount == perfectSteps) {
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

    // Update health UI icon display
    void UpdateHealthUI() {
        if (healthUI == null) {
            return;
        }

        Image img = null;

        switch (GameManager.instance.playerHealth.Health())
        {
        case 2:
            img = healthUI.transform.GetChild(0).GetComponent<Image>();
            break;
        case 1:
            img = healthUI.transform.GetChild(1).GetComponent<Image>();
            break;
        case 0:
            img = healthUI.transform.GetChild(2).GetComponent<Image>();
            break;
        }

        if (img == null) {
            return;
        }

        Color color = img.color;
        color.a = .5f;
        img.color = color;
    }

    // Highlight torch ui and enable animation
    public void activateTorchUI() {
        if (torchUI == null) {
            return;
        }

        Image torchImage = torchUI.GetComponent<Image>();
        Color torchColor = torchImage.color;
        torchColor.a = 1;
        torchImage.color = torchColor;
        torchUI.GetComponent<Animator>().enabled = true;
    }
}
