using System.Collections.Generic;
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

    // Check to see if there are still nearby interactables and update the notice accordingly
    public void UpdateInteractionNotice() {
        CheckForDestroyedColliders();
        
        if (interactionColliders.Count > 0) {
            OpenInteractNotice();
        } else {
            CloseInteractNotice();
        }
    }

    // Check to see if any references to nearby interactable colliders have since been destroyed (ie interacted with)
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

    // Stop watching for interactable changes (ie when moving)
    public void StopWatchingInteractableChanges() {
        watchingInteractableChanges = false;
    }

    // Start watching for interactable changes (ie when stationery)
    public void StartWatchingInteractableChanges() {
        watchingInteractableChanges = true;
    }
}
