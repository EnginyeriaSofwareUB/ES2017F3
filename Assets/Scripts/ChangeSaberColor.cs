using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSaberColor : MonoBehaviour {

    public Material[] lightsaberColors;
    public Renderer r;

    [Space(10)]
    [Header("Audio Stuff")]
    
    AudioSource source;
    public AudioClip on;
    public AudioClip off;
    public AudioClip loop;
    public AudioClip[] attacks;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        int rand = Random.Range(0, lightsaberColors.Length);
        r.material = lightsaberColors[rand];

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
        GetComponentInParent<PlayerController>().GetComponent<AudioSource>().PlayOneShot(off);
        //source.PlayOneShot(off);
    }

    public AudioClip RandomAttackSound()
    {
        return attacks[Random.Range(0, attacks.Length)];
    }
}
