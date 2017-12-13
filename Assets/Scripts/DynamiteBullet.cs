using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamiteBullet : ExplosiveBullet {

    public bool isDinamite = true; //if is dinamite we play the dinamite sound, otherwise must be a granade
    private Animator anim;

    //to show countdown
    public Text countdown;
    float timeSinceDrop;
    Quaternion initRot;
    Vector3 initPos;

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

        //countdown = GetComponentInChildren<Text>();
        initRot = transform.rotation;
        initPos = transform.position;

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

        Invoke("EndShoot", 2.2f);
    }

    public void TriggerSound()
    {
        audio.Stop();
        audio.loop = false;

        if (isDinamite) { 
            audio.clip = boom;
            audio.PlayDelayed(1.8f);
        }
        else { 
            //audio.clip = grenadeBoom;
            audio.PlayOneShot(grenadeBoom);
        }
        
         //1.5 is the explosive animation lenght
    }

    void EndShoot()
    {
        //tell controler that we finished attacking
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().shoot_ongoing = false;
    }

    private void Update()
    {
        timeSinceDrop += Time.deltaTime;
        float timeToExplode;
        if (isDinamite)
            timeToExplode = TimeUntilDespawn - timeSinceDrop;
        else
            timeToExplode = TimeUntilDespawn - timeSinceDrop -2f; //to fix some bug with grande, explodes 2 sec before 'TimeUntilDespawn'

        if (timeToExplode > 1f)
            countdown.color = Color.yellow;
        else if (timeToExplode < 1.5f)
            countdown.color = Color.red;


        if (countdown && timeToExplode >= 0f)
            countdown.text = timeToExplode.ToString("0");


        GetComponentInChildren<Canvas>().transform.rotation = initRot;
        //GetComponentInChildren<Canvas>().transform.position = initPos;
    }
}
