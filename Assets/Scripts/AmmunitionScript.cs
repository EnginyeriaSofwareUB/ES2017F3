﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionScript : MonoBehaviour {

	private GameObject GM;

	private int BOW = 4;
	private int GRENADE = 3;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GM");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		//print ("coll");
		if (col.gameObject.tag == "Player") {			
			Destroy (this.gameObject);
			int team = col.GetComponent<PlayerController>().TEAM;
            Debug.Log(GM);
			if (Random.value > 0.5) {				
				GM.GetComponent<GameController> ().addUsages (team, BOW, 1);
			} else {
				GM.GetComponent<GameController> ().addUsages (team, GRENADE, 3);
			}
		}
	}
}
