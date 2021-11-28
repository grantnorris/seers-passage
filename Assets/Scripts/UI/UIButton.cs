using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    Button btn;

    void Start() {
        btn = GetComponent<Button>();

        if (btn != null) {
            btn.onClick.AddListener(delegate {AudioManager.instance.PlayOneShot("Dialogue Open");});
        }
    }
}
