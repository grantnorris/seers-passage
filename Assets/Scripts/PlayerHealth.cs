using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    int health = 3;

    // The current health of the player
    public int Health() {
        return health;
    }

    // Reduce health by 1
    public void ReduceHealth() {
        health--;

        if (health <= 0) {
            Die();
        }
    }
    
    // End the game 
    void Die() {
        GameManager.instance.player.transform.Find("Visual").GetComponent<Animator>().SetBool("dead", true);
    }
}
