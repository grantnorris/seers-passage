using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    BoxCollider2D col;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Interactable ready");

            PlayerControl playerScript = other.GetComponent<PlayerControl>();

            if (playerScript != null) {
                GameObject interactNotice = playerScript.interactNotice;

                if (interactNotice != null) {
                    Animator interactNoticeAnim = interactNotice.GetComponent<Animator>();

                    interactNotice.SetActive(true);

                    if (interactNoticeAnim != null) {
                        interactNoticeAnim.SetTrigger("open");
                        Debug.Log("Open interact notice");
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            PlayerControl playerScript = other.GetComponent<PlayerControl>();

            if (playerScript != null) {
                GameObject interactNotice = playerScript.interactNotice;

                if (interactNotice != null) {
                    interactNotice.SetActive(false);
                }
            }
        }
    }

    public virtual void Interact() {
        Debug.Log("interact");
    }
}
