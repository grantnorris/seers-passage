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
    public void Update() {
        Image img = null;

        switch (GameManager.instance.playerHealth.Health())
        {
        case 2:
            img = transform.GetChild(2).GetComponent<Image>();
            break;
        case 1:
            img = transform.GetChild(1).GetComponent<Image>();
            break;
        case 0:
            img = transform.GetChild(0).GetComponent<Image>();
            break;
        }

        if (img == null) {
            return;
        }

        img.sprite = heartEmptySprite;
    }
}
