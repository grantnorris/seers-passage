using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Player player;
    public GameObject interactNotice;
    [HideInInspector]
    public PlayerInteractNotice interactNoticeScript;
    [SerializeField]
    List<Collider2D> interactionColliders = new List<Collider2D>();

    bool watchingInteractableChanges = true;

    void Awake() {
        player = GetComponent<Player>();
    }

    void Start() {
        if (interactNotice != null) {
            interactNoticeScript = interactNotice.GetComponent<PlayerInteractNotice>();
        }

        player.move.finishMoving.AddListener(UpdateInteractionNotice);
    }

    public void UpdateInteractionNotice() {
        CheckForDestroyedColliders();
        
        if (interactionColliders.Count > 0) {
            OpenInteractNotice();
        } else {
            CloseInteractNotice();
        }
    }

    void CheckForDestroyedColliders() {
        if (interactionColliders.Count == 0) {
            return;
        }

        List<Collider2D> colsToRemove = new List<Collider2D>();

        foreach (Collider2D col in interactionColliders) {
            if (col != null && col.gameObject && col.tag == "Interactable" && Vector3.Distance(col.transform.position, player.transform.position) < 2) {
                continue;
            }

            colsToRemove.Add(col);
        }

        foreach (Collider2D col in colsToRemove) {
            interactionColliders.Remove(col);
        }
    }

    // Display interation notice on trigger enter
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Interactable" && watchingInteractableChanges) {
            interactionColliders.Add(other);
        }
    }

    // Hide interation notice on trigger enter
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Interactable" && watchingInteractableChanges) {
            interactionColliders.Remove(other);
        }
    }

    // Open interaction notice
    public void OpenInteractNotice() {
        if (interactNoticeScript == null) {
            return;
        }

        interactNoticeScript.Open();
        TipManager.DisplayTip("Interactables");
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
