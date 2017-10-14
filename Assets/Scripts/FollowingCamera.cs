using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {

	private GameController controller;
	public GameObject target;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		// Acceso al Controlador y guardamos la referencia al objeto Jugador.
		controller = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();

        target = controller.activePlayer;
        if (target)
            offset = transform.position - target.transform.position;
		
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () {
        if(target)
		    transform.position = target.transform.position + offset;
	}
}
