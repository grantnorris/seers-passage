using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    Button btn;

    void Start() {
        btn = GetComponent<Button>();

        AddAudioOnClickListener();
    }

    void AddAudioOnClickListener() {
        if (btn != null) {
            btn.onClick.AddListener(delegate {AudioManager.instance.PlayOneShot("Dialogue Open");});
        }
    }
}
