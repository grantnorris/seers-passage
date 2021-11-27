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

    void UpdateInteractionNotice() {
        print("update interaction notice");

        if (interactionColliders.Count > 0) {
            OpenInteractNotice();
        } else {
            CloseInteractNotice();
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
