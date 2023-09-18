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

    // Update the drag UI sprite based on the player's drag direction
    public void Display(float amount, Vector2 position, string direction) {
        if (arrowImg == null) {
            return;
        }

        int spriteIndex = Mathf.RoundToInt(sprites.Length * amount);
        int rotation = 0;

        if (spriteIndex <= 0) {
            spriteIndex = 0;
        } else if (spriteIndex > sprites.Length) {
            spriteIndex = sprites.Length;
        }

        switch (direction) {
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

        // Set the rotation of the sprite transform
        arrowImg.transform.rotation = Quaternion.Euler(0, 0, rotation);

        // Set the sprite and color based on drag distance
        arrowImg.sprite = sprites[spriteIndex - 1];
        arrowImg.color = new Color(arrowImg.color.r, arrowImg.color.g, arrowImg.color.b, amount);

        if (anim == null) {
            return;
        }

        // If the player has dragged enough to move the character lock the sprite so it doesn't change during animation
        if (spriteIndex == sprites.Length) {
            anim.SetBool("Locked", true);
        } else {
            anim.SetBool("Locked", false);
        }
    }

    // Trigger the animation based on the direction the player dragged
    public void ApplyDirection(string direction) {
        if (direction == null) {
            return;
        }

        string animationTrigger = "";

        switch (direction) {
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

    // Reset the drag UI sprite, color and rotation
    public void Reset() {
        ResetArrowSprite();
        transform.rotation = Quaternion.identity;
        anim.SetBool("Locked", false);
    }

    // Reset the drag UI arrow sprite
    // This is called by Reset() and triggered by an animation
    public void ResetArrowSprite() {
        arrowImg.sprite = defaultSprite;
    }
}
