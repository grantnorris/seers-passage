using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI instance;

    [SerializeField]
    Sprite heartFullSprite;
    [SerializeField]
    Sprite heartEmptySprite;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        Init();
    }

    // Initialise UI by filling all hearts
    void Init() {
        foreach (Transform heart in transform) {
            heart.GetComponent<Image>().sprite = heartFullSprite;
        }
    }

    // Update UI to reflect health
    public void UpdateUI() {
        Animator anim = null;

        switch (GameManager.instance.player.health.Health())
        {
        case 2:
            anim = transform.GetChild(2).GetComponent<Animator>();
            anim.SetTrigger("Remove");
            break;
        case 1:
            anim = transform.GetChild(1).GetComponent<Animator>();
            transform.GetChild(0).GetComponent<Animator>().SetBool("Flash", true);
            anim.SetTrigger("Remove");
            break;
        case 0:
            anim = transform.GetChild(0).GetComponent<Animator>();
            transform.GetChild(0).GetComponent<Animator>().SetBool("Flash", false);
            anim.SetTrigger("Remove");
            break;
        }
    }
}
