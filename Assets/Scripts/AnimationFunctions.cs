using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallDeath()
    {
        GetComponentInParent<PlayerController>().Death();
    }

    public void RandomizeIdle()
    {
        print("Idle randomized");
        anim.SetFloat("IdleBlend", Random.Range(0, 3));
    }

}
