using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// TODO: Medical will be a trigger
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			//TODO: Heal this player
			Debug.Log(col.gameObject.name +" healed");

			Destroy (this.gameObject);
		}
	}
}
