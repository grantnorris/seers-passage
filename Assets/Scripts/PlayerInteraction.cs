using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject interactNotice;
    [HideInInspector]
    public PlayerInteractNotice interactNoticeScript;

    bool watchingInteractableChanges = true;

    void Start() {
        if (interactNotice != null) {
            interactNoticeScript = interactNotice.GetComponent<PlayerInteractNotice>();
        }
    }

    // Display interation notice on trigger enter
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Interactable" && watchingInteractableChanges) {
            OpenInteractNotice();
            print("approached interactable");
        }
    }

    // Hide interation notice on trigger enter
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Interactable" && watchingInteractableChanges) {
            CloseInteractNotice();
            print("left interactable");
        }
    }

    // Open interaction notice
    public void OpenInteractNotice() {
        if (interactNoticeScript == null) {
            return;
        }

        interactNoticeScript.Open();
    }

    // Close interaction notice
    public void CloseInteractNotice() {
        if (interactNoticeScript == null) {
            return;
        }

        interactNoticeScript.Close();
    }

    public void StopWatchingInteractableChanges() {
        watchingInteractableChanges = false;
    }

    public void StartWatchingInteractableChanges() {
        watchingInteractableChanges = true;
    }
}
