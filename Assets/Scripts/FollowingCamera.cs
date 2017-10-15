using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {

	private GameController controller;
	private Camera camera;

	public GameObject target;
	private Vector3 offset;

	private float dampTime = 0.1f;
	private Vector3 velocity = Vector3.zero;

	[Header("Smooth-Follow Camera")]
	public bool activateSmooth = false;

	// Use this for initialization
	void Start () {
		// Acceso al Controlador, Camara y guardamos la referencia al objeto Jugador.
		controller = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
		camera = GetComponent<Camera>();
			

	}
	
	// LateUpdate is called once per frame
	void LateUpdate () {
		// Initial position of the Camera based on activePlayer Position. Initial set of 'offset'.
        target = controller.activePlayer;
		if (target) {
			transform.position = new Vector3 (target.transform.position.x, (target.transform.position.y + 2), transform.position.z);
			offset = transform.position - target.transform.position;
		}

		if (target) {

			// Camera Smooth Damping for a nicer follow.
			if (activateSmooth) {
				Vector3 point = camera.WorldToViewportPoint (target.transform.position);
				Vector3 delta = target.transform.position - camera.ViewportToWorldPoint (new Vector3 (0.45f, 0.25f, point.z));

				transform.position = Vector3.SmoothDamp (transform.position, (transform.position + delta), ref velocity, dampTime);
			
			// Camera basic follow.
			} else {
				transform.position = target.transform.position + offset;
			}
		}
	}
}
