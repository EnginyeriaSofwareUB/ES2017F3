using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSaberColor : MonoBehaviour {

    public Light saberLight;
    public Material[] lightsaberColors;
    public Renderer r;

    [Space(10)]
    [Header("Audio Stuff")]
    
    AudioSource source;
    public AudioClip on;
    public AudioClip off;
    public AudioClip loop;
    public AudioClip[] attacks;

    bool firstdisable = true; //to avoid bug of lightsaber off sound on start match

    private void Awake()
    {
        saberLight = GetComponentInChildren<Light>();
    }

    private void OnEnable()
    {
        //color order: red, blue, green, purple, yellow

        int rand = Random.Range(0, lightsaberColors.Length);
        r.material = lightsaberColors[rand];

        //set light color
        switch (rand)
        {
            case 0:
                saberLight.color = Color.red;
                break;
            case 1:
                saberLight.color = Color.blue;
                break;
            case 2:
                saberLight.color = Color.green;
                break;
            case 3:
                saberLight.color = Color.magenta;
                break;
            case 4:
                saberLight.color = Color.yellow;
                break;
        }

        source = GetComponent<AudioSource>();
        source.PlayOneShot(on);

        //Invoke("SaberSound", on.length);
        SaberSound();
    }

    void SaberSound()
    {
        source.clip = loop;
        source.loop = true;
        source.Play();
    }

    private void OnDisable()
    {
        if (!firstdisable)
        {
            GetComponentInParent<PlayerController>().GetComponent<AudioSource>().PlayOneShot(off);
            firstdisable = false;
        }
        //source.PlayOneShot(off);
    }

    public AudioClip RandomAttackSound()
    {
        return attacks[Random.Range(0, attacks.Length)];
    }
}
