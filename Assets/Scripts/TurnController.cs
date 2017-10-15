using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

	private GameController gameController;

	



	// Use this for initialization
	void Start () {
		// chapuza
		gameController = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();

        // foreach (GameObject player in players) {
        //     // Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
        // }
		// gameController.players;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
