using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragUI : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    Sprite defaultSprite;
    Image img;

    void Start() {
        img = GetComponent<Image>();

        if (img == null) {
            return;
        }
        
        defaultSprite = img.sprite;
    }

    public void Display(float amount, Vector2 position, string direction) {
        if (img == null) {
            return;
        }

        amount = amount > 1 ? 1 : amount;

        int spriteIndex = Mathf.RoundToInt(sprites.Length * amount);
        spriteIndex = spriteIndex == 0 ? 1 : spriteIndex;
        int rotation = 0;

        switch (direction)
        {
            case "up":
            rotation = 90;
            break;

            case "down":
            rotation = 270;
            break;

            case "left":
            rotation = 180;
            break;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotation);
        img.sprite = sprites[spriteIndex - 1];
    }

    public void Reset() {
        if (img == null) {
            return;
        }

        img.sprite = defaultSprite;
        transform.rotation = Quaternion.identity;
    }
}
