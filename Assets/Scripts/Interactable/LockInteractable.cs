using UnityEngine;

public class LockInteractable : Interactable
{
    public override void Interact() {
        // Inventory.instance.Remove(new InventoryItem("Key"));

        Gate gate = GetComponent<Gate>();

        if (!gate) {
            return;
        }

        float animationDuration = .8f;

        // Run animation
        gate.Activate();

        StartCoroutine(GameManager.instance.DisablePlayerMoveForDuration(animationDuration));
        
        // Shake the camera
        if (CameraShake.instance != null) {
            StartCoroutine(CameraShake.instance.Shake(animationDuration, .05f));
        }
    }
}
