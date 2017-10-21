using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject testPlayerPrefab; //seleccionar per script al carregar desde la escena anterior
    public List<GameObject> players; //mirar de eliminar! 


    [Header("TEAM variables")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public static int nPlayersPerTeam = 1;
    public List<GameObject> team1;
    public List<GameObject> team2;
    int spawned1 = 0;
    int spawned2 = 0;


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

        //Spawn players
        InitGame();

        // retrieve players
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
		// initiate
		foreach (GameObject player in players) {
            Debug.Log(player);
			// disable movement
			player.GetComponent<PlayerMovement>().enabled = false;
			// disable firing shoots
			player.GetComponent<PlayerShooting>().enabled = false;
            // attach listener to shootEvent
            player.GetComponent<PlayerShooting>().shootEvent.AddListener(OnShoot);
            // attach listener to deathEvent
            player.GetComponent<PlayerController>().deathEvent.AddListener(OnDeath);
        }

        turnId = -1;
		changeTurn();
	}

    void InitGame()
    {
        Debug.Log("Init game; Spawning " + nPlayersPerTeam + " per team.");
        //team1
        for (int i = 0; i < nPlayersPerTeam; i++)
        {
            //GameObject p1 = Instantiate(Resources.Load("Prefabs/Characters/Animated Characters/" + testPlayerPrefabName), spawnPoint1.position, spawnPoint1.rotation, null) as GameObject;
            GameObject p1 = Instantiate(testPlayerPrefab, spawnPoint1.position, spawnPoint1.rotation, null) as GameObject;
            p1.SetActive(true);
            p1.GetComponent<PlayerController>().TEAM = 1;
            p1.GetComponent<PlayerController>().playerId = 1 + spawned1;
            p1.GetComponent<PlayerController>().last_dir = 1;
            team1.Add(p1);
            spawned1++;

            p1.name = "Player_T1_" + spawned1.ToString();

            activePlayer = p1; //activem el player 1 com a target 
        }

        //TEAM2
        for (int i = 0; i < nPlayersPerTeam; i++)
        {
            GameObject p2 = Instantiate(testPlayerPrefab, spawnPoint2.position, spawnPoint2.rotation, null) as GameObject;
            p2.SetActive(true);
            p2.GetComponent<PlayerController>().TEAM = 2;
            p2.GetComponent<PlayerController>().playerId = 2 + spawned2;
            //p2.transform.localScale = new Vector3(p2.transform.localScale.x * (-1f), p2.transform.localScale.y, p2.transform.localScale.z);
            p2.GetComponent<PlayerController>().last_dir = -1;

            p2.GetComponent<PlayerMovement>().enabled = false;

            team2.Add(p2);
            spawned2++;

            p2.name = "Player_T2_" + spawned2.ToString();
        }
    }




    void OnShoot() {
		// disable shooting
		activePlayer.GetComponent<PlayerShooting>().enabled = false;
	}

    void OnDeath(int playerId) {
        // Delete from players dead player
        players.RemoveAll(player => player.GetComponent<PlayerController>().playerId == playerId);
    }


    void changeTurn() {
		// disable movement and firing to the previous player
		activePlayer.GetComponent<PlayerMovement>().enabled = false;
		activePlayer.GetComponent<PlayerShooting>().enabled = false;

		// point to the next player
		turnId = (turnId + 1) % players.Count;
		// FIXME @rafa: this dummy assignment will lead weird bugs
		// TODO: pass to next plater with a better way

	    if (players.Count < 2) {
            // Game finished

            Debug.Log("Game has ended!");

            current = gameStates.gameOver;

	        //Return to main menu
	        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
	    }

        else {
            // Game continues

            activePlayer = players[turnId];
            Debug.Log("Now active player is: " + activePlayer);
            // enable movement
            activePlayer.GetComponent<PlayerMovement>().enabled = true;
            // enable firing shoots
            activePlayer.GetComponent<PlayerShooting>().enabled = true;

            // this turn expires in 10 seconds
            turnRemainingTime = turnTime;
        }

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
