using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }
    
    public void TransitionOut() {
        anim.SetTrigger("transitionOut");
    }

    public void ChangeScene() {
        SceneManager.LoadScene("Level Select");
    }
}
