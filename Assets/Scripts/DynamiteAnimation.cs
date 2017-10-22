using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteAnimation : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
	    anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.A))
	    {
	        anim.SetTrigger("explode");
	    }

	}
}
