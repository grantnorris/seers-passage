using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField]
    GameObject hearts;
    [SerializeField]
    Sprite heartSolid;
    [SerializeField]
    Sprite heartOutline;
    [SerializeField]
    TMP_Text scoreTxt;
    [SerializeField]
    GameObject Torches;
    List<Animator> torchAnimators = new List<Animator>();
    [SerializeField]
    CanvasGroup continueUI;
    int playerHealth;

    void Awake() {
        playerHealth = GameManager.instance.playerHealth.Health();
        SetHearts();
        SetScoreText();
        SetTorchAnimators();
        continueUI.alpha = 0f;
        StartCoroutine("AnimateObjects");
    }
    
    void SetHearts() {
        if (hearts == null || heartSolid == null || heartOutline == null) {
            return;
        }

        int counter = 0;

        foreach (Image img in hearts.GetComponentsInChildren<Image>()) {
            counter++;

            if (playerHealth < counter) {
                img.sprite = heartOutline;
            }

            img.sprite = heartSolid;
        }

        hearts.SetActive(false);
    }

    void SetScoreText() {
        if (scoreTxt == null) {
            return;
        }

        string txt = null;
        switch (playerHealth)
        {
        case 3:
            txt = "Perfect";
            break;
        case 2:
            txt = "Great";
            break;
        case 1:
            txt = "Close call";
            break;
        }

        scoreTxt.SetText(txt.ToUpper());
        scoreTxt.gameObject.SetActive(false);
    }

    void SetTorchAnimators() {
        foreach (Animator anim in Torches.GetComponentsInChildren<Animator>()) {
            torchAnimators.Add(anim);
            anim.enabled = false;
        }
    }

    IEnumerator AnimateObjects() {
        // Wait for frame animation to happen
        yield return new WaitForSeconds(1.25f);
        yield return StartCoroutine("AnimateHearts");
        yield return StartCoroutine("AnimateScoreText");
        EnableTorches();
        yield return new WaitForSeconds(.5f);
        StartCoroutine("AnimateContinue");
    }

    IEnumerator AnimateHearts() {
        hearts.SetActive(true);
        List<GameObject> heartObjs = new List<GameObject>();

        foreach (Transform heart in hearts.transform) {
            heartObjs.Add(heart.gameObject);
            heart.gameObject.SetActive(false);
        }

        foreach (GameObject heart in heartObjs) {
            heart.SetActive(true);   

            Vector3 finishScale = heart.transform.localScale;
            Vector3 startScale = finishScale * 1.5f;

            float time = 0f;
            float seconds = .25f;

            while (time <= 1f) {
                time += Time.deltaTime / seconds;
                heart.transform.localScale = Vector3.Lerp(startScale, finishScale, time);
                yield return null;
            }

            heart.transform.localScale = finishScale;
        }
    }

    IEnumerator AnimateScoreText() {
        scoreTxt.gameObject.SetActive(true);   

        Vector3 finishScale = scoreTxt.transform.localScale;
        Vector3 startScale = finishScale * 1.5f;

        float time = 0f;
        float seconds = .25f;

        while (time <= 1f) {
            time += Time.deltaTime / seconds;
            scoreTxt.transform.localScale = Vector3.Lerp(startScale, finishScale, time);
            yield return null;
        }

        scoreTxt.transform.localScale = finishScale;
    }
    
    void EnableTorches() {
        foreach (Animator anim in torchAnimators) {
            anim.enabled = true;
        }
    }

    IEnumerator AnimateContinue() {
        float time = 0f;
        float seconds = 1f;

        while (time <= 1f) {
            time += Time.deltaTime / seconds;
            continueUI.alpha = Mathf.Lerp(0f, 1f, time);
            yield return null;
        }

        continueUI.alpha = 1f;
    }
}
