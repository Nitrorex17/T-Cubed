using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    private Animator anim;
    public Board board;
    public int index;
    public int change;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnMouseDown() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            anim.SetTrigger("Press");
            StartCoroutine(board.MoveMarker(index, change));
        }
    }
}
