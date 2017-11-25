using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    GameController control;
    AudioSource source;
    PlayerMovement move;

    public Gun selectedWeapon;
    [Space(5)]

    public AudioClip[] jump;
    public AudioClip[] jump2;

    [Space(5)]

    public AudioClip steps;

    [Space(5)]

    public AudioClip canon;

    // Use this for initialization
    void Start () {
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
        source = GetComponent<AudioSource>();
        move = GetComponent<PlayerMovement>();

        GetComponent<PlayerShooting>().shootEvent.AddListener(OnShoot);
    }
	
	// Update is called once per frame
	void Update () {
        selectedWeapon = GetComponent<PlayerShooting>()._currentGun;

        if (move.horizontal != 0 && move.isGrounded && !source.isPlaying) //is walking
        {
            source.volume = 0.3f;
            source.clip = steps;
            source.loop = true;
            source.Play();
        }

        if (GetComponent<Rigidbody>().velocity == Vector3.zero)//|| !move.isGrounded) // no walking
        {
            source.volume = 1f;
            source.loop = false;
            source.Stop();
        }

        /*
        if (move.jump && !source.isPlaying)
        {
            int rand = Random.Range(0, 2);
            print("jump"+rand);
            source.PlayOneShot(jump[rand]);
        }

        if (move.backJump && !source.isPlaying)
        {
            int rand = Random.Range(0, 2);
            source.PlayOneShot(jump2[rand]);
        }*/

        if (!control.activePlayer.Equals(this.gameObject)) //pasi el que pasi, si no som el actiu, parem els sonidos
        {
            source.Stop();
        }
	}


    public void NormalJump()
    {
        source.Stop();
        int rand = Random.Range(0, jump.Length);
        print("jump" + rand);
        source.volume = 0.5f;
        source.PlayOneShot(jump[rand]);
    }

    public void BackJump()
    {
        int rand = Random.Range(0, jump2.Length);
        print("jump" + rand);
        source.volume = 0.5f;
        source.PlayOneShot(jump2[rand]);
    }

    public void OnShoot()
    {
        print("shoooot on audio " + selectedWeapon.name);
        switch (selectedWeapon.name)
        {
            case "Cannon Base":
                source.volume = 0.5f;
                source.PlayOneShot(canon);
                break;
        }
        
    }
}
