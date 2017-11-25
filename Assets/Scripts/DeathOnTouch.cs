using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {

	private bool isDrown;
	private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		isDrown = false;
	}

	void Update () {

	}

	void OnTriggerEnter(Collider col){

		if (col.gameObject.CompareTag("Player") && !isDrown) {
			Debug.Log ("A");
			isDrown = true;
			GameObject g = col.gameObject;

			g.transform.GetComponent<Rigidbody> ().isKinematic = true;
			g.GetComponentInChildren<Animator> ().SetTrigger ("drown");

			//set a delay
			coroutine = DelayToDeath(g);
			StartCoroutine (coroutine);

		}
	}

	IEnumerator DelayToDeath(GameObject g){
		print ("COR" + g);
		yield return new WaitForSeconds (2);
		if (g != null) {
			g.GetComponent<PlayerController> ().Damage (100f);
		}
		//		yield return new WaitForSeconds (2);
		//		GameObject.FindGameObjectWithTag ("GM").GetComponent<GameController> ().changeTurn ();
	}


}
