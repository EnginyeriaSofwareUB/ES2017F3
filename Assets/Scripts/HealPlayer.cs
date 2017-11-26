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
		
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			Debug.Log(col.gameObject.name +" healed");

			//TODO: Heal the player correctly
			col.gameObject.GetComponent<PlayerController> ().Heal (10);

			Destroy (this.gameObject);
		}
	}
}
