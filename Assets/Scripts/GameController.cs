using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public enum gameStates {
		menu,
		gameOn,
		pause,
		gameOver,
		none,
	};

	public gameStates current = gameStates.none;

    [Header("Testing Variables Here")]
    public GameObject activePlayer = null;
	public GameObject[] players;

	// points to the current active playe in the players index
	public int turnId;
	// in seconds
	public float turnRemainingTime;

	// Use this for initialization
	void Start () {
		//TODO: Set the activePlayer to the Main Player.
		//activePlayer = GameObject.Find(testPlayerName);	

		// retrieve players
		players = GameObject.FindGameObjectsWithTag("Player");
		// initiate
		foreach (GameObject player in players) {
			// disable movement
			player.GetComponent<PlayerMovement>().enabled = false;
			// disable firing shoots
			player.GetComponent<PlayerShooting>().enabled = false;
		}

		turnId = -1;
		changeTurn();
	}

	void changeTurn() {
		// disable movement and firing to the previous player
		activePlayer.GetComponent<PlayerMovement>().enabled = false;
		activePlayer.GetComponent<PlayerShooting>().enabled = false;

		// point to the next player
		turnId = (turnId + 1) % players.Length;

		if (players.Length < 2) {
			current = gameStates.gameOver;
		}

		activePlayer = players[turnId];
		// enable movement
		activePlayer.GetComponent<PlayerMovement>().enabled = true;
		// enable firing shoots
		activePlayer.GetComponent<PlayerShooting>().enabled = true;

		// this turn expires in 10 seconds
		turnRemainingTime = 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		turnRemainingTime -= Time.deltaTime;
		if(turnRemainingTime < 0) {
			changeTurn();
		}
	}
}
