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
        Debug.Log("transition splash out");
    }

    public void ChangeScene() {
        SceneManager.LoadScene("Level Select");
        Debug.Log("change scene to level select");
    }
}
