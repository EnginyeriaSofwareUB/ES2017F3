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
        StartCoroutine("DynamiteExplosion");
    }

    IEnumerator DynamiteExplosion() {
        anim.SetTrigger("explode");

        //AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        //while (animState.normalizedTime < 1) {
        //    yield return null;
        //}

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1.4f);
        
        TriggerExplosion();

    }

}
