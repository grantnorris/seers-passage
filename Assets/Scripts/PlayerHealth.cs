using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int health = 3;

    // The current health of the player
    public int Health() {
        return health;
    }

    public void ReduceHealth() {
        health--;

        Debug.Log("player health = " + health);

        if (health <= 0) {
            Die();
        }
    }
    
    // End the game 
    void Die() {
        Debug.Log("player dead");
    }
}
