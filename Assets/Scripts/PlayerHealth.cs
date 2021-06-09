using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject UI;

    int health = 3;

    // The current health of the player
    public int Health() {
        return health;
    }

    public void ReduceHealth() {
        health--;

        UpdateHealthUI();

        if (health <= 0) {
            Die();
        }
    }

    // Update health UI icon display
    void UpdateHealthUI() {
        if (UI == null) {
            return;
        }

        Image img = null;

        switch (health)
        {
        case 2:
            img = UI.transform.GetChild(0).GetComponent<Image>();
            break;
        case 1:
            img = UI.transform.GetChild(1).GetComponent<Image>();
            break;
        case 0:
            img = UI.transform.GetChild(2).GetComponent<Image>();
            break;
        }

        if (img == null) {
            return;
        }

        Color color = img.color;
        color.a = .5f;
        img.color = color;
    }
    
    // End the game 
    void Die() {
        Debug.Log("player dead");
    }
}
