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

	void OnTriggerEnter(Collider col){

		Debug.Log (col.tag + col.name);

		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerController> ().Damage(100f);

			//TODO: Set a delay
			GameObject.FindGameObjectWithTag ("GM").GetComponent<GameController> ().changeTurn ();
		}
	}
}
