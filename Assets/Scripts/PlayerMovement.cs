using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var horizontal = Input.GetAxis ("Horizontal") * 5f;

		rb.velocity = new Vector3 (horizontal, rb.velocity.y);

		if (Input.GetButtonDown ("Jump"))
			rb.velocity += new Vector3(0, 10f);
	}
}
