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

    // Vibrate the mobile device
    public void Vibrate() {
        Logger.Send("Vibrate.");

        // Currently disabled as this doesn't feel great and we aren't offered much/any control over duration and intensity
        // This should be an option in the settings if vibrations/haptics are kept
        // Handheld.Vibrate();
    }
}
