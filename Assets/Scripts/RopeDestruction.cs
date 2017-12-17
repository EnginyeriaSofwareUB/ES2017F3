using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeDestruction : MonoBehaviour {

	public GameObject[] destructibles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		foreach (GameObject g in destructibles) {
			try{
				var t = g.transform.position.x;
			}
			catch{
				print ("AAAAAAAA");
				Destroy (this.gameObject);
			}
		}
	}
}
