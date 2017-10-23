using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBullet : ExplosiveBullet {

    private Animator anim;

	// Use this for initialization
	new void Start () {
	    base.Start();
	    anim = GetComponent<Animator>();
	}

    protected override void DespawnBullet() {
        anim.SetTrigger("explode");
    }

}
