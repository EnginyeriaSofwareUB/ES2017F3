using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {

	public GameObject target;

	[Header("Camera sets")]
	public float offset = 20.0f;
	public float height = 2.0f;
	public float speed = 3.0f;

	[Header("Smooth-Follow Camera")]
	public bool activateSmooth = false;

	// Use this for initialization
	void Start () {
        if(target)
		    transform.position = new Vector3 (target.transform.position.x, (target.transform.position.y + 2), transform.position.z);
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () {

		if (target) {
			// Camera Smooth Damping for a nicer follow.
			if (activateSmooth) {

				Vector3 pos = transform.position;
				pos.x = Mathf.Lerp(transform.position.x, target.transform.position.x, (speed * Time.deltaTime));
				pos.y = Mathf.Lerp(transform.position.y, (target.transform.position.y + height), (speed * Time.deltaTime));
				pos.z = target.transform.position.z - offset;

				transform.position = pos;

			// Camera basic follow.
			} else {
				transform.position = new Vector3(target.transform.position.x, target.transform.position.y + height, target.transform.position.z - offset);
			}
		}
	}
}
