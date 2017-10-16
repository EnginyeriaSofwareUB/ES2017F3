using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public enum gameStates {
		menu,
		gameOn,
		pause,
		gameOver,
		none,
	};

	public gameStates current = gameStates.none;
    [Header("Canvas Objects")]
    public Text turnTimerText;


    [Header("Testing Variables Here")]
    public GameObject activePlayer = null;
	public GameObject[] players;

    [Header("Turns")]
	// points to the current active playe in the players index
	public int turnId;
    // in seconds
    public float turnTime = 10.0f;
	public float turnRemainingTime;

	// Use this for initialization
	void Start () {
		//TODO: Set the activePlayer to the Main Player.
		//activePlayer = GameObject.Find(testPlayerName);	

		// retrieve players
		players = GameObject.FindGameObjectsWithTag("Player");
		// initiate
		foreach (GameObject player in players) {
            Debug.Log(player);
			// disable movement
			player.GetComponent<PlayerMovement>().enabled = false;
			// disable firing shoots
			player.GetComponent<PlayerShooting>().enabled = false;
            // attach listener to shootEvent
            player.GetComponent<PlayerShooting>().shootEvent.AddListener(OnShoot);
        }

        turnId = -1;
		changeTurn();
	}

	void OnShoot() {
		// disable shooting
		activePlayer.GetComponent<PlayerShooting>().enabled = false;
	}

	void changeTurn() {
		// disable movement and firing to the previous player
		activePlayer.GetComponent<PlayerMovement>().enabled = false;
		activePlayer.GetComponent<PlayerShooting>().enabled = false;

		// point to the next player
		turnId = (turnId + 1) % players.Length;
		// FIXME @rafa: this dummy assignment will lead weird bugs
		// TODO: pass to next plater with a better way

		if (players.Length < 2) {
			current = gameStates.gameOver;
		}

		activePlayer = players[turnId];
        Debug.Log("Now active player is: " + activePlayer);
		// enable movement
		activePlayer.GetComponent<PlayerMovement>().enabled = true;
		// enable firing shoots
		activePlayer.GetComponent<PlayerShooting>().enabled = true;

		// this turn expires in 10 seconds
		turnRemainingTime = turnTime;
	}
	
	// Update is called once per frame
	void Update () {
		turnRemainingTime -= Time.deltaTime;
		if(turnRemainingTime < 0) {
			changeTurn();
		}


        UpdateCanvas();
	}

    void UpdateCanvas()
    {
        if(turnRemainingTime <= turnTime * 0.2f)
        {
            turnTimerText.color = Color.red;
        }
        else
        {
            turnTimerText.color = Color.blue;
        }
        turnTimerText.text = "Remaining time: "+turnRemainingTime.ToString();
    }
}
