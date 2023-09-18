using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }
    
    // Set the animation trigger to transition out of the splash screen
    // This is triggered by an animation
    public void TransitionOut() {
        anim.SetTrigger("transitionOut");
    }

    // Change to the level select screen
    // This is triggered by an animation
    public void ChangeScene() {
        SceneManager.LoadScene("Level Select");
    }
}
