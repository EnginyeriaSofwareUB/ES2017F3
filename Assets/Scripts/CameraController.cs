using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameController controller;
	private FollowingCamera f;
	private MovementCamera m;

	public bool follow = true;

	void Awake () {
		// Acceso al Controlador, Camara y guardamos la referencia al objeto Jugador.
		controller = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
		f = Camera.main.GetComponent<FollowingCamera> ();
		m = Camera.main.GetComponent<MovementCamera> ();

		f.target = controller.activePlayer;
	}

	// Use this for initialization
	void Start () {
		if (follow) {
			m.enabled = false;
			f.enabled = true;
		} else {
			m.enabled = true;
			f.enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {
        // Update target
        f.target = controller.activePlayer; //TODO: Desde el controler, al canvi de torn es pot canviar el active player del following camera; evitem accedir a cada frame

		// Pressing Tab Key makes lock/unlock character
		if (Input.GetKeyDown(KeyCode.Tab)) {
			follow = !follow;
		}

		if (follow) {
			m.enabled = false;
			f.enabled = true;
		} else {
			m.enabled = true;
			f.enabled = false;
		}
	}
}
