using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{

    private Animator anim;
    private PlayerMovement pm;
    private PlayerShooting ps;
    private PlayerController pc;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();
        ps = GetComponentInParent<PlayerShooting>();
        pc = GetComponentInParent<PlayerController>();
    }

    public void CallDeath() {
        pc.Death();
    }

    public void RandomizeIdle() {
        anim.SetFloat("IdleBlend", Random.Range(0, 3));
    }

    public void PerformJump() {
        pm.Jump();
    }

    public void FireCannon() {
        anim.SetTrigger("fireCannon");
    }

    public void EndFire() {
        ps.EmptyHands();
    }

}
