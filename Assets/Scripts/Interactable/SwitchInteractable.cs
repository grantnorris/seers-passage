using UnityEngine;

public class SwitchInteractable : Interactable
{
    public Animator anim;
    public GameObject target;

    public override void Interact() {
        if (anim != null) {
            anim.SetTrigger("interact");
        }

        if (target != null) {
            // Activate target gate
            if (target.GetComponent<Gate>()) {
                target.GetComponent<Gate>().Activate();                
            }

            // Other switch interactions to come in the future
        }
    }
}
