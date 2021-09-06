using UnityEngine;
using UnityEngine.UI;

public class Haptics : MonoBehaviour
{
    Button btn;

    void Start() {
        btn = GetComponent<Button>();

        if (btn != null) {
            btn.onClick.AddListener(delegate {Vibrate();});
        }
    }

    public void Vibrate() {
        Debug.Log("Vibrate");
        // Handheld.Vibrate();
    }
}
