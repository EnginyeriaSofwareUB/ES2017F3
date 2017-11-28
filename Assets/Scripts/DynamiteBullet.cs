using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBullet : ExplosiveBullet {

    public bool isDinamite = true; //if is dinamite we play the dinamite sound, otherwise must be a granade
    private Animator anim;

    AudioSource audio;
    public AudioClip mecha;
    public AudioClip boom;
    [Space(5)]
    public AudioClip grenadeBoom;

	// Use this for initialization
	new void Start () {
	    base.Start();
	    anim = GetComponent<Animator>();

        audio = GetComponent<AudioSource>();

        if (isDinamite)
        {
            audio.clip = mecha;
            audio.Play();
            audio.loop = true;
        }
        else
        {
            audio.clip = grenadeBoom;
        }
    }

    protected override void DespawnBullet() {

        TriggerSound();

        anim.SetTrigger("explode");
    }

    public void TriggerSound()
    {
        audio.Stop();
        audio.loop = false;

        if (isDinamite) { 
            audio.clip = boom;
            audio.PlayDelayed(1.5f);
        }
        else { 
            //audio.clip = grenadeBoom;
            audio.PlayOneShot(grenadeBoom);
        }
        
         //1.5 is the explosive animation lenght
    }

}
