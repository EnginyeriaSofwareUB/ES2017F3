using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBullet : ExplosiveBullet {

    private Animator anim;

    AudioSource audio;
    public AudioClip mecha;
    public AudioClip boom;

	// Use this for initialization
	new void Start () {
	    base.Start();
	    anim = GetComponent<Animator>();

        audio = GetComponent<AudioSource>();
        audio.clip = mecha;
        audio.Play();
        audio.loop = true;
    }

    protected override void DespawnBullet() {
        audio.Stop();
        audio.clip = boom;
        audio.loop = false;
        audio.PlayDelayed(1.5f); //1.5 is the explosive animation lenght

        anim.SetTrigger("explode");
    }

}
