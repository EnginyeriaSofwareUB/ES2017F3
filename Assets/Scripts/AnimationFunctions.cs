using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{

    private Animator anim;
    private PlayerMovement pm;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();
    }

    public void CallDeath() {
        GetComponentInParent<PlayerController>().Death();
    }

    public void RandomizeIdle() {
        //print("Idle randomized");
        anim.SetFloat("IdleBlend", Random.Range(0, 3));
    }

    public void PerformJump() {
        pm.Jump();
    }

}
