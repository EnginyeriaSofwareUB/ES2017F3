﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public enum gameStates {
		menu,
        startAnim,
		gameOn,
		pause,
		gameOver,
        delayed,
		none,
	};

	public GameObject completeLevelUI; //elemento para poder poner gameOver image
	public GameObject pauseScreenUI;
	public GameObject handS, lightSS, canonS, tntS, granadeS, arrowS;

    [Space(5)]
    public bool shoot_ongoing = false;

	public gameStates current = gameStates.none;
    [Header("Canvas Objects")]
    public Text turnTimerText;
    public GameObject[] UI; //for activating/deactivating UI purposes
    public GameObject UI_shoot_bar; //for players to retrieve it from here, no more findbytag, so we can deactivate this gameobjecte safefully
    public GameObject UI_shoot_text;

    [Header("Player/Prefab Variables")]
	public GameObject activePlayer = null;

    //public List<GameObject> testPlayerPrefabs; //seleccionar per script al carregar desde la escena anterior
    public GameObject vikingPlayer;
    public GameObject piratePlayer;
    public GameObject knightPlayer;

    public List<GameObject> players; //mirar de eliminar! 
	private GameObject PlayerPrefab;

    [Header("TEAM variables")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public float maxSpawnSpread = 1f;
    public static int nPlayersPerTeam = 2;
    public List<GameObject> team1;
    public List<GameObject> team2;
    int spawned1 = 0;
    int spawned2 = 0;

    [Header("Turns")]
	// points to the current active playe in the players index
	public int turnId;
    // in seconds
    public float turnTime = 10.0f;
    public float delayTime = 5f;
    public float afterShootTime = 3f;
	public float turnRemainingTime;
	public float delayRemainingTime;

	// Sudden Death (Reduces HP of all plyers to 1)
	public int turnsTillSudden = 10;
	private bool suddenDeath = false;
	private int turnCount;

	[Header("Guns")]
	public List<Gun> AvailableGuns;

	private int[][] _teamGunUses;
	public static readonly UnityEvent ChangedGunUsesEvent = new UnityEvent();

	// Use this for initialization
	void Start() {
        //TODO: Set the activePlayer to the Main Player.
        //activePlayer = GameObject.Find(testPlayerName);	

        //Start camera animation, deactivate UI
        Camera.main.GetComponent<Animator>().SetTrigger("start");
        current = gameStates.startAnim;
        SetUIActive(false);

        //TODO: LOAD DATA FROM GamePreferences INTO MATCH USING InitGame()
        Debug.Log("TEAM_1 FACTION:::"+ GamePreferences.p1_faction);
		Debug.Log("TEAM_2 FACTION:::"+ GamePreferences.p2_faction);
		Debug.Log("PLAYERS MAX_LIFE:::"+ GamePreferences.players_maxlife);
		Debug.Log("SUDDEN_DEATH ACTIVATED:::"+ GamePreferences.sudden_death_activated);
		Debug.Log("SUDDEN_DEATH TURNS:::"+ GamePreferences.sudden_death_turns);

		//Spawn players
        InitGame();
		// Create remaining gun uses
		_teamGunUses = new int[2][];
		for (int i = 0; i < 2; i++) {
			_teamGunUses[i] = new int[AvailableGuns.Count];
			for (int j = 0; j < AvailableGuns.Count; j++)
			{
				_teamGunUses[i][j] = AvailableGuns[j].InitialUsagesLeft;
			}
		}

		//Init shots left
				
        turnCount = 0;
        // retrieve players
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
        players.Sort((self, other) => self.GetComponent<PlayerController>().playerId - other.GetComponent<PlayerController>().playerId);

		// initiate
		foreach (GameObject player in players) {
            //Debug.Log(player);
			// disable movement
			player.GetComponent<PlayerMovement>().enabled = false;
			// disable firing shoots
			player.GetComponent<PlayerShooting>().enabled = false;
            // attach listener to shootEvent
            player.GetComponent<PlayerShooting>().shootEvent.AddListener(OnShoot);
            // attach listener to deathEvent
            player.GetComponent<PlayerController>().deathEvent.AddListener(OnDeath);


        }

        //turnId = -1;
        //changeTurn(); //moved to StartGame()
    }

    void InitGame() {
		//Getting Match Data from the Menu
		nPlayersPerTeam = GamePreferences.number_players_team;
		suddenDeath = GamePreferences.sudden_death_activated;
		turnsTillSudden = GamePreferences.sudden_death_turns;
        //testPlayerPrefab = testPlayerPrefabs.ToList()[0];

		// For now it only checks for VIKING or NON VIKING players
		//if (GamePreferences.p1_faction == "viking") {
		//	testPlayerPrefab = testPlayerPrefabs.ToList () [0];
		//}

        Debug.Log(" >>> Init game; Spawning " + nPlayersPerTeam + " per team.");
        //team1
        for (int i = 0; i < nPlayersPerTeam; i++) {

            PlayerPrefab = null;
            //ALERT: Check factions name
            switch (GamePreferences.p1_faction)
            {
                case "pirates":
                    PlayerPrefab = piratePlayer;
                    break;
                case "vikings":
                    PlayerPrefab = vikingPlayer;
                    break;
                case "knights":
                    PlayerPrefab = knightPlayer;
                    break;
            }

            //take spawnposition and add some variation on X value, to spread the spawn
            Vector3 where = new Vector3(spawnPoint1.position.x + Random.Range(-maxSpawnSpread, maxSpawnSpread), spawnPoint1.position.y, spawnPoint1.position.z);

            //GameObject p1 = Instantiate(Resources.Load("Prefabs/Characters/Animated Characters/" + testPlayerPrefabName), spawnPoint1.position, spawnPoint1.rotation, null) as GameObject;
            GameObject p1 = Instantiate(PlayerPrefab, where, spawnPoint1.rotation, null) as GameObject;
            p1.SetActive(true);
            p1.GetComponent<PlayerController>().TEAM = 1;
            p1.GetComponent<PlayerController>().playerId = 1 + 2*spawned1;
            p1.GetComponent<PlayerController>().last_dir = 1;
			p1.GetComponent<PlayerController>().maxHealth = GamePreferences.players_maxlife;
			p1.GetComponent<PlayerController>().health = GamePreferences.players_maxlife;
			p1.GetComponent<PlayerController> ().InitPlayerCanvas ();
            team1.Add(p1);
            spawned1++;

            p1.name = "Player_T1_" + spawned1.ToString();

            activePlayer = p1; //activem el player ultim del team 1 com a target 
        }

		// For now it only checks for VIKING or NON VIKING players
		//if (GamePreferences.p2_faction == "viking") {
		//	testPlayerPrefab = testPlayerPrefabs.ToList()[0];
		//}

        //TEAM2
        for (int i = 0; i < nPlayersPerTeam; i++) {
            PlayerPrefab = null;
            //ALERT: Check factions name
            switch (GamePreferences.p2_faction)
            {
                case "pirates":
                    PlayerPrefab = piratePlayer;
                    break;
                case "vikings":
                    PlayerPrefab = vikingPlayer;
                    break;
                case "knight":
                    PlayerPrefab = knightPlayer;
                    break;
            }
            //take spawnposition and add some variation on X value, to spread the spawn
            Vector3 where = new Vector3(spawnPoint2.position.x + Random.Range(-maxSpawnSpread, maxSpawnSpread), spawnPoint2.position.y, spawnPoint2.position.z);

            GameObject p2 = Instantiate(PlayerPrefab, where, spawnPoint2.rotation, null) as GameObject;
            p2.SetActive(true);
            p2.GetComponent<PlayerController>().TEAM = 2;
            p2.GetComponent<PlayerController>().playerId = 2 + 2*spawned2;
            p2.GetComponent<PlayerController>().last_dir = -1;
			p2.GetComponent<PlayerController>().maxHealth = GamePreferences.players_maxlife;
			p2.GetComponent<PlayerController>().health = GamePreferences.players_maxlife;
			p2.GetComponent<PlayerController> ().InitPlayerCanvas ();

            p2.GetComponent<PlayerMovement>().enabled = false;

            team2.Add(p2);
            spawned2++;

            p2.name = "Player_T2_" + spawned2.ToString();
        }
    }


    //this function will be called when the start camera animation ends
    public void StartGame()
    {
        Debug.Log("GAME BEGIN");
        Camera.main.GetComponent<CameraController>().SetPlayerTargetFirstTime();
        Destroy(Camera.main.GetComponent<Animator>());

        SetUIActive(true);
        current = gameStates.gameOn;
        turnId = -1;
        changeTurn();
    }


    void OnShoot() {
		// disable shooting
		activePlayer.GetComponent<PlayerShooting>().enabled = false;
        turnRemainingTime = afterShootTime;	
		updateUsages ();

        shoot_ongoing = true;
    }

	public void updateUsages(){
		int team = activePlayer.GetComponent<PlayerController> ().TEAM;

		GetComponent<InitUsages> ().SetBowUsages(team, _teamGunUses [team-1] [4]);
		GetComponent<InitUsages> ().SetGrenadeUsages(team, _teamGunUses [team-1] [3]);
	}

	public void addUsages(int team, int gun, int value){
		_teamGunUses[team-1][gun]+=value;
		updateUsages ();
	}

	void ChangeGun(){

		int wnum= activePlayer.GetComponent<PlayerShooting>().lastGunEquipped;

		switch (wnum) {
		default://hand active
			handS.SetActive(true);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
			break;
		case 0://light sable active
			handS.SetActive(false);
			lightSS.SetActive (true);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
			break;
		case 1://canon active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (true);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
			break;
		case 2://tnt active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (true);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
			break;
		case 3://granade active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (true);
			arrowS.SetActive (false);
			break;
		case 4://arrow active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (true);
			break;
		}
		
    }
		
    void OnDeath(int playerId) {
        bool isCurrentPlayer = activePlayer.GetComponent<PlayerController>().playerId == playerId; //ALERT: Si moren els dos alhora peta aqui
        // Debug.Log("suicide! " + isCurrentPlayer);

        // Delete from players dead player
        players.RemoveAll(player => player.GetComponent<PlayerController>().playerId == playerId);

        // suicide
        if (isCurrentPlayer) changeTurn();

        // Game over
        if (players.Count < 2) {
            Debug.Log("Game has ended!");

            current = gameStates.gameOver; //important

            // activar pantalla GameOver
			completeLevelUI.SetActive(true);

            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayGameOverSound();

            //Return to main menu
            //SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
        }
    }

	public int GetGunUsagesLeft(int team, int index) {
		return _teamGunUses[team - 1][index];
	}

	public IEnumerable<int> GetGunUsagesLeft(int team) {
		return _teamGunUses[team - 1];
	}

	public void AddGunUsages(int team, int index, int usages) {
		if (_teamGunUses[team - 1][index] < 0)
			return;
		_teamGunUses[team - 1][index] += usages;
		ChangedGunUsesEvent.Invoke();
	}

    public void disableActivePlayer() {
        // disable movement and firing to the previous player
        if (activePlayer) {
            activePlayer.GetComponent<PlayerShooting>().EmptyHands();
            activePlayer.GetComponent<PlayerMovement>().Idle();
            activePlayer.GetComponent<PlayerMovement>().enabled = false;
            activePlayer.GetComponent<PlayerShooting>().enabled = false;
	        activePlayer.GetComponent<PlayerController>().enableOutline(false);


			// disable flag
			if (activePlayer.GetComponentInChildren<FlagMainPlayer>() != null){
				activePlayer.GetComponentInChildren<FlagMainPlayer>().EnableMain(false);
			}
        }
    }

    public void startDelay() {
        current = gameStates.delayed;
        delayRemainingTime = delayTime;
        disableActivePlayer();
    }
	
    public void changeTurn() {
        if (current != gameStates.gameOver)
        {
            current = gameStates.gameOn;

            disableActivePlayer();

            // point to the next player
            turnId = (turnId + 1) % players.Count;
            // FIXME @rafa: this dummy assignment will lead weird bugs
            // TODO: pass to next plater with a better way

            // Game continues
            if (players.Count > 1)
            {
                // Sudden death
                if (!suddenDeath)
                {
                    if (turnCount >= turnsTillSudden)
                    {
                        SuddenDeath();
                        suddenDeath = true;
                    }
                    turnCount += 1;
                }

                activePlayer = players[turnId];
                //Debug.Log("Now active player is: " + activePlayer);
                // enable movement
                activePlayer.GetComponent<PlayerMovement>().enabled = true;
                // enable firing shoots
                activePlayer.GetComponent<PlayerShooting>().enabled = true;
	            // enable outline
	            activePlayer.GetComponent<PlayerController>().enableOutline(true);

                // enable flag
                if (activePlayer.GetComponentInChildren<FlagMainPlayer>() != null)
                {
                    activePlayer.GetComponentInChildren<FlagMainPlayer>().EnableMain(true);
                }

                // this turn expires in 10 seconds
                turnRemainingTime = turnTime;

                GetComponent<WindController>().ChangeWindRandom();

                //GetComponent<MatchProgressBar>().GetTeamHPs(); //recalculate HPs on change turn (probably not needed here)
            }

            int team = activePlayer.GetComponent<PlayerController>().TEAM;
            GetComponent<InitUsages>().SetPanel(team);
            GetComponent<InitUsages>().SetBowUsages(team, _teamGunUses[team - 1][4]);
            GetComponent<InitUsages>().SetGrenadeUsages(team, _teamGunUses[team - 1][3]);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if(current == gameStates.gameOn)
        {
            if (activePlayer)
                activePlayer.GetComponent<PlayerShooting>().ChangeGunEvent.AddListener(ChangeGun);

            turnRemainingTime -= Time.deltaTime;
            if (turnRemainingTime < 0)
            {
                changeTurn();
            }
        }
        

		if (Input.GetKey (KeyCode.Escape)) {
			if (current.Equals("pause")) {
				pauseScreenUI.SetActive(false);
				current = gameStates.gameOn;
				Time.timeScale = 1;
			} else {
				pauseScreenUI.SetActive(true);
				current = gameStates.pause;
				Time.timeScale = 0;
			}

		}

        UpdateCanvas();
	}

    void UpdateCanvas() {
        turnTimerText.color = (turnRemainingTime <= turnTime * 0.2f) ? Color.red : Color.blue;
        turnTimerText.text = turnRemainingTime.ToString("00"); //Remaining time 
    }

	void SuddenDeath() {
		Debug.Log("Sudden Death ON ::: All players at 1HP");
		foreach (GameObject player in players) {
			player.GetComponent<PlayerController> ().Damage ((player.GetComponent<PlayerController> ().health - 1));
		}
	}

	public void BotonResumPause() {
		pauseScreenUI.SetActive(false);
		current = gameStates.gameOn;
		Time.timeScale = 1;
	}

	public void PauseHowTo(bool pause){
		if (pause) {
			current = gameStates.pause;
			Time.timeScale = 0;
		} else {
			current = gameStates.gameOn;
			Time.timeScale = 1;
		}
	}

    public void SetUIActive(bool active)
    {
        foreach(GameObject e in UI)
        {
            e.SetActive(active);
        }
    }
}
