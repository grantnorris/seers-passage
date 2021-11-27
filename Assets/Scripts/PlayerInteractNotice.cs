using UnityEngine;

public class PlayerInteractNotice : MonoBehaviour
{
    public GameObject visual;
    Animator anim;

    void Start() {
        if (visual != null) {
            anim = visual.GetComponent<Animator>();
        }
    }

    // Run open animation for visual
    public void Open() {
        if (visual != null && anim != null) {
            anim.SetBool("open", true);
        }
    }

    // Run close animation for visual
    public void Close() {
        if (visual != null && anim != null) {
            anim.SetBool("open", false);
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}
