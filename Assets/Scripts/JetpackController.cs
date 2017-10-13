using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackController : MonoBehaviour {


	void FixedUpdate(){

		if (Input.GetMouseButtonDown (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			// create a plane at 0,0,0 whose normal points to Z:
			Plane hPlane = new Plane (Vector3.back, Vector3.zero);

			// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
			float distance = 0; 

			// if the ray hits the plane...
			if (hPlane.Raycast (ray, out distance)) {

				// get the hit point:
				var newPos = ray.GetPoint (distance);

				// Get the direction from the player to the mouse
				var d = (newPos - transform.position).normalized;

				// Speed is proportional to distance
				var speed = Vector3.Distance (newPos, transform.position);

				// Move the player 
				GetComponent<Rigidbody>().AddForce(d*speed*75, ForceMode.Impulse);
			}
		}
	}

}
