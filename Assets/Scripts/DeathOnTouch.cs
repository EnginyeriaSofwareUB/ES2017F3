using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {

    public float turnTimeRemainingOnFall;

	private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		
	}

	void Update () {

	}

	void OnTriggerEnter(Collider col){

		if (col.gameObject.CompareTag("Player") && !col.gameObject.transform.GetComponent<Rigidbody> ().isKinematic) {
			GameObject g = col.gameObject;

			g.transform.GetComponent<Rigidbody> ().isKinematic = true;
			g.GetComponentInChildren<Animator> ().SetTrigger ("drown");

			//set a delay
			coroutine = DelayToDeath(g);
			StartCoroutine (coroutine);

            //play chof sound
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().FallToWater();

            //set remaining turn time
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().turnRemainingTime = turnTimeRemainingOnFall;

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
