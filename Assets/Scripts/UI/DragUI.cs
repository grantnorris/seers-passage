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
            case "up":
            rotation = 0;
            break;

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

        arrowImg.sprite = sprites[spriteIndex - 1];
        arrowImg.transform.rotation = Quaternion.Euler(0, 0, rotation);

        if (anim == null) {
            return;
        }

        if (spriteIndex == sprites.Length) {
            anim.SetBool("Locked", true);
        } else {
            anim.SetBool("Locked", false);
        }
    }

    public void ApplyDirection(string direction) {
        if (direction == null) {
            return;
        }

        string animationTrigger = "";

        switch (direction)
            {
            case "up":
            animationTrigger = "ApplyUp";
            break;

            case "right":
            animationTrigger = "ApplyRight";
            break;

            case "down":
            animationTrigger = "ApplyDown";
            break;

            case "left":
            animationTrigger = "ApplyLeft";
            break;
        }

        anim.SetTrigger(animationTrigger);
    }

    public void Reset() {
        ResetArrowSprite();
        transform.rotation = Quaternion.identity;
        anim.SetBool("Locked", false);
    }

    public void ResetArrowSprite() {
        arrowImg.sprite = defaultSprite;
    }
}
