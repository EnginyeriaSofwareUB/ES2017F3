using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    //just for general sounds

    public AudioSource source;
    public AudioSource[] sourceAux;
    [Header("Audio Clips")]

    public AudioClip mainTheme;
    public AudioClip seaSound;

    [Header("Weather related Clips")]
    public AudioClip calm; //CALM
    public AudioClip windy; //CLOUDY
    public AudioClip rain; //STORMY
    public AudioClip storm; //STORMY
    

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        //sourceAux = GetComponentsInChildren<AudioSource>(); //For multiple "loop" audios at same time
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAmbientSound(AudioClip au)
    {
        sourceAux[0].Stop();
        sourceAux[0].clip = au;
        sourceAux[0].loop = true;
        sourceAux[0].Play();

        if (au.Equals(rain)) //if its rain, we add the storm audio too
        {
            sourceAux[1].Stop();
            sourceAux[1].clip = storm;
            sourceAux[1].loop = true;
            sourceAux[1].Play();
        }
    }


}
