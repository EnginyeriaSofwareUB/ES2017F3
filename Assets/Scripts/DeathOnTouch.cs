using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {

	private GameObject g;


	// Use this for initialization
	void Start () {

	}

	void Update () {

	}

	void OnTriggerEnter(Collider col){

		Debug.Log (col.tag + col.name);

		if (col.gameObject.CompareTag("Player")) {

			g = col.gameObject;

			g.transform.GetComponent<Rigidbody> ().isKinematic = true;

			//g.transform.position -= new Vector3 (0, 0.5f, 0);

			col.gameObject.GetComponent<PlayerController> ().Damage(100f);

			//TODO: Set a delay
			//GameObject.FindGameObjectWithTag ("GM").GetComponent<GameController> ().changeTurn ();
		}
	}


}
