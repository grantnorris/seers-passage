using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject UI;
    public GameObject gameUI;
    public GameObject outroUI;
    public TMP_Text playerStepCountTxt;
    public GameObject heartbreakPrefab;
    public GameObject healthUI;
    public GameObject torchUI;
    
    GameObject heartbreakUI;

    void Start() {
        Init();
    }

    void Init() {
        playerStepCountTxt.text = "0";
        GameManager.instance.stepped.AddListener(StartUpdateStepCountUI);
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
        GameObject tempTxtObj = Instantiate(playerStepCountTxt.gameObject, playerStepCountTxt.transform.parent);
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
        HealthUI.instance.Update();
    }

    public void StepCountDialogue() {
        int perfectSteps = GameManager.instance.perfectSteps;
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

    // Highlight torch ui and enable animation
    public void ActivateTorchUI() {
        if (torchUI == null) {
            return;
        }

        Image torchImage = torchUI.GetComponent<Image>();
        Color torchColor = torchImage.color;
        torchColor.a = 1;
        torchImage.color = torchColor;
        torchUI.GetComponent<Animator>().enabled = true;
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
        yield return new WaitForSeconds(2.2f);
        gameUI.GetComponent<Animator>().SetTrigger("TransitionOut");
    }
}
