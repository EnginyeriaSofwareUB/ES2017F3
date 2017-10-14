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

	

	// Use this for initialization
	void Start () {
		//TODO: Set the activePlayer to the Main Player.
		//activePlayer = GameObject.Find(testPlayerName);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
