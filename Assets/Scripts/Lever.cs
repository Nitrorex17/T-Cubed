using UnityEngine;

public class Lever : MonoBehaviour {
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Pull() {
        anim.SetTrigger("Pull");
    }
}