using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour {

	public float minFov = 15f;
	public float maxFov = 90f;
	public float sensitivity = 10f;

	private float fov;

	// Use this for initialization
	void Start() {
		fov = Camera.main.fieldOfView;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov);

		Camera.main.fieldOfView = fov;
	}
}