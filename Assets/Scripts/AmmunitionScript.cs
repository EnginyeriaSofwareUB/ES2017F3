using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {			

			//TODO: Update ammunition

			Destroy (this.gameObject);
		}
	}
}
