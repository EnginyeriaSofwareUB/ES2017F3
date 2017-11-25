using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    //just for general sounds

    AudioSource source;

    public AudioClip mainTheme;
    public AudioClip seaSound;
    public AudioClip rain;
    public AudioClip storm;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
