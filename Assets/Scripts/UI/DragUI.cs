using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragUI : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    Sprite defaultSprite;
    [SerializeField]
    Image arrowImg;
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();

        if (arrowImg == null) {
            return;
        }
        
        defaultSprite = arrowImg.sprite;
    }

    public void Display(float amount, Vector2 position, string direction) {
        if (arrowImg == null) {
            return;
        }

        int spriteIndex = Mathf.RoundToInt(sprites.Length * amount);
        int rotation = 0;

        if (spriteIndex <= 0) {
            spriteIndex = 1;
        } else if (spriteIndex > sprites.Length) {
            spriteIndex = sprites.Length;
        }

        switch (direction)
        {
            case "right":
            rotation = 270;
            break;

            case "down":
            rotation = 180;
            break;

            case "left":
            rotation = 90;
            break;
        }

        arrowImg.transform.rotation = Quaternion.Euler(0, 0, rotation);
        arrowImg.sprite = sprites[spriteIndex - 1];

        if (anim == null) {
            return;
        }

        if (spriteIndex == sprites.Length) {
            anim.SetBool("Locked", true);
        } else {
            anim.SetBool("Locked", false);
        }
    }

    public void Reset() {
        if (arrowImg == null) {
            return;
        }

        arrowImg.sprite = defaultSprite;
        transform.rotation = Quaternion.identity;

        if (anim != null) {
            anim.SetBool("Locked", false);
        }
    }
}
