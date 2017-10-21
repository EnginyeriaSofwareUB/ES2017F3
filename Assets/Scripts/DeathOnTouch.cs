using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {

			// TODO: Dying should be public
			col.gameObject.GetComponent<PlayerController> ().Damage (col.gameObject.GetComponent<PlayerController> ().maxHealth);
		}
	}
}
