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
        howto,
        startAnim,
		gameOn,
		pause,
		gameOver,
        delayed,
		none,
	};

	public GameObject completeLevelUI; //elemento para poder poner gameOver image
	public GameObject pauseScreenUI;
    public GameObject skipStartAnimationMsg;
    public GameObject handS, lightSS, canonS, tntS, granadeS, arrowS;

    [Space(5)]
    public bool shoot_ongoing = false;
    public float cameraOnExplosionTime = 1f;
    bool shoot_dynamite = false;
    bool startAnimCancelled = false;

	public gameStates current = gameStates.none;
    public int winner = 0;
    [Header("Canvas Objects")]
    public Text turnTimerText;
    public GameObject[] UI; //for activating/deactivating UI purposes
    public GameObject UI_shoot_bar; //for players to retrieve it from here, no more findbytag, so we can deactivate this gameobjecte safefully
    public GameObject UI_shoot_text;
    public Text UI_winnerplayer;

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
	public int turnTeam1;
	public int turnTeam2;
    // in seconds
    public float turnTime = 10.0f;
    public float delayTime = 5f;
    public float afterShootTime = 3f;
	public float turnRemainingTime;
	public float delayRemainingTime;

	// Sudden Death (Reduces HP of all plyers to 1)
	public int turnsTillSudden = 10;
	public bool suddenDeathActivated;
	private bool suddenDeath = false;
	private int turnCount;

	[Header("Guns")]
	public List<Gun> AvailableGuns;

	private int[][] _teamGunUses;
	public static readonly UnityEvent ChangedGunUsesEvent = new UnityEvent();

	// Use this for initialization
	void Start() {

        GetComponent<HowTo>().enabled = GamePreferences.howTo;

        

        //If HowTo is activated, the game starts normally, otherwise it begins the start animation rutine 
        if (GamePreferences.howTo)
        {
            current = gameStates.howto;
            
            InitHowToGame();

            StartGame();
        }
        else
        {
            //Start camera animation, deactivate UI
            Camera.main.GetComponent<Animator>().SetTrigger("start");
            current = gameStates.startAnim;
            SetUIActive(false);

            //Spawn players
            InitGame();
        }


        //TODO: LOAD DATA FROM GamePreferences INTO MATCH USING InitGame()
        Debug.Log("TEAM_1 FACTION:::"+ GamePreferences.p1_faction);
		Debug.Log("TEAM_2 FACTION:::"+ GamePreferences.p2_faction);
		Debug.Log("PLAYERS MAX_LIFE:::"+ GamePreferences.players_maxlife);
		Debug.Log("SUDDEN_DEATH ACTIVATED:::"+ GamePreferences.sudden_death_activated);
		Debug.Log("SUDDEN_DEATH TURNS:::"+ GamePreferences.sudden_death_turns);

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

        if (!GamePreferences.howTo)
        {
            // initiate
            foreach (GameObject player in players)
            {
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
        }


        //set audio master volume
        AudioListener.volume = GamePreferences.audio_volume;
    }

    void InitHowToGame()
    {
        PlayerPrefab = vikingPlayer;
        Vector3 where = new Vector3(spawnPoint1.position.x + Random.Range(-maxSpawnSpread, maxSpawnSpread), spawnPoint1.position.y, spawnPoint1.position.z);
        GameObject p1 = Instantiate(PlayerPrefab, where, spawnPoint1.rotation, null) as GameObject;
        p1.SetActive(true);
        p1.GetComponent<PlayerController>().TEAM = 1;
        p1.GetComponent<PlayerController>().playerId = 1 + 2 * spawned1;
        p1.GetComponent<PlayerController>().last_dir = 1;
        p1.GetComponent<PlayerController>().maxHealth = GamePreferences.players_maxlife;
        p1.GetComponent<PlayerController>().health = GamePreferences.players_maxlife;
        p1.GetComponent<PlayerController>().InitPlayerCanvas();
        team1.Add(p1);
        spawned1++;
        p1.name = "Player_T1_" + spawned1.ToString();

        activePlayer = p1;


        PlayerPrefab = knightPlayer;
        where = new Vector3(spawnPoint2.position.x + Random.Range(-maxSpawnSpread, maxSpawnSpread), spawnPoint2.position.y, spawnPoint2.position.z);
        GameObject p2 = Instantiate(PlayerPrefab, where, spawnPoint2.rotation, null) as GameObject;
        p2.SetActive(true);
        p2.GetComponent<PlayerController>().TEAM = 2;
        p2.GetComponent<PlayerController>().playerId = 2 + 2 * spawned2;
        p2.GetComponent<PlayerController>().last_dir = -1;
        p2.GetComponent<PlayerController>().maxHealth = GamePreferences.players_maxlife;
        p2.GetComponent<PlayerController>().health = GamePreferences.players_maxlife;
        p2.GetComponent<PlayerController>().InitPlayerCanvas();
        team2.Add(p2);
        spawned2++;
        p2.name = "Player_T2_" + spawned2.ToString();
    }

    void InitGame() {
		//Getting Match Data from the Menu
		nPlayersPerTeam = GamePreferences.number_players_team;
		suddenDeathActivated = GamePreferences.sudden_death_activated;
		turnsTillSudden = GamePreferences.sudden_death_turns;

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

            
        }

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
                case "knights":
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

            activePlayer = p2; //activem el last player afegit, dakesta manera a changeTurn() es triara el primer torn el player 1 del team 1
        }
    }


    //this function will be called when the start camera animation ends or is skipped
    public void StartGame()
    {
        Debug.Log("GAME BEGIN");
        Camera.main.GetComponent<CameraController>().SetPlayerTargetFirstTime();
        //Destroy(Camera.main.GetComponent<Animator>());
        Camera.main.GetComponent<Animator>().SetTrigger("skipstart");
        Camera.main.GetComponent<Animator>().enabled = false;

        skipStartAnimationMsg.SetActive(false);
        SetUIActive(true);

        if(current != gameStates.howto)
        {
            current = gameStates.gameOn;
            turnTeam1 = -1;
            turnTeam2 = -1;
            changeTurn();
        }
        else
        {
            print("howto active, enabling " + activePlayer);
            activePlayer.GetComponent<PlayerMovement>().enabled = true;
            activePlayer.GetComponent<PlayerShooting>().enabled = true;
            activePlayer.GetComponent<PlayerController>().enableOutline(true);
        }
        
    }


    void OnShoot() {
		// disable shooting
		activePlayer.GetComponent<PlayerShooting>().enabled = false;
        if (shoot_dynamite)
            turnRemainingTime = afterShootTime + cameraOnExplosionTime; //+ 2f;	 //2f is da explosion animation lenght
        else
            turnRemainingTime = afterShootTime + cameraOnExplosionTime;

        updateUsages ();

        //shoot_ongoing = true; //now set from gun.cs
    }

    //called when projectile explodes
    public void OnEndShoot()
    {
        Invoke("ShootOnGoingEnd", cameraOnExplosionTime);
    }

    void ShootOnGoingEnd()
    {
        shoot_ongoing = false;
    }

	public void updateUsages(){
		int team = activePlayer.GetComponent<PlayerController> ().TEAM;

		GetComponent<InitUsages> ().SetBowUsages(team, _teamGunUses [team-1] [4]);
		GetComponent<InitUsages> ().SetDynamiteUsages(team, _teamGunUses [team-1] [2]);
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
            shoot_dynamite = false;
            break;
		case 0://light sable active
			handS.SetActive(false);
			lightSS.SetActive (true);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
            shoot_dynamite = false;
            break;
		case 1://canon active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (true);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
            shoot_dynamite = false;
            break;
		case 2://tnt active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (true);
			granadeS.SetActive (false);
			arrowS.SetActive (false);
            shoot_dynamite = true;
            break;
		case 3://granade active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (true);
			arrowS.SetActive (false);
            shoot_dynamite = true;
            break;
		case 4://arrow active
			handS.SetActive(false);
			lightSS.SetActive (false);
			canonS.SetActive (false);
			tntS.SetActive (false);
			granadeS.SetActive (false);
			arrowS.SetActive (true);
            shoot_dynamite = false;
            break;
		}
		
    }
		
    void OnDeath(int playerId) {
		if (gameStates.gameOver != current) {
			bool isCurrentPlayer = activePlayer.GetComponent<PlayerController> ().playerId == playerId; //ALERT: Si moren els dos alhora peta aqui
			// Debug.Log("suicide! " + isCurrentPlayer);

			// Delete from players dead player
			players.RemoveAll (player => player.GetComponent<PlayerController> ().playerId == playerId);
			team1.RemoveAll (player => player.GetComponent<PlayerController> ().playerId == playerId);
			team2.RemoveAll (player => player.GetComponent<PlayerController> ().playerId == playerId);

			// Game over
			bool isTeam1Alive = team1.Count > 0;
			bool isTeam2Alive = team2.Count > 0;
			if (!isTeam1Alive || !isTeam2Alive) {
				Debug.Log ("Game has ended!");

				current = gameStates.gameOver; //important
				winner = isTeam1Alive ? 1 : 2;

				// activar pantalla GameOver
				completeLevelUI.SetActive (true);
				foreach (GameObject g in UI) {
					g.SetActive (false);
				}

				//set player who wins
				UI_winnerplayer.text = "Player " + winner;

				GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ().PlayGameOverSound ();

				//Return to main menu
				//SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
			} else {
				// suicide
				if (isCurrentPlayer)
					changeTurn ();
			}
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

            //to avoid out of control bugs
            shoot_ongoing = false;

            disableActivePlayer();

            // point to the next player
            int prevTeam = activePlayer.GetComponent<PlayerController>().playerId % 2;
            if (prevTeam == 0) {
                // next team 1
                turnTeam1 = (turnTeam1 + 1) % team1.Count;
                activePlayer = team1[turnTeam1];
            } else {
                // next team 2
                turnTeam2 = (turnTeam2 + 1) % team2.Count;
                activePlayer = team2[turnTeam2];
            }
            

            // Game continues
            if (players.Count > 1)
            {
				if (suddenDeathActivated) {
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
				}

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
            GetComponent<InitUsages>().SetDynamiteUsages(team, _teamGunUses[team - 1][2]);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if(current == gameStates.startAnim)
        {
            if (Input.GetKeyUp(KeyCode.Space) && !startAnimCancelled)
            {
                StartGame();
                startAnimCancelled = true;
                Camera.main.GetComponent<Animator>().enabled = false; //stop travelling animation

                GetComponent<MatchProgressBar>().SetBackIconsDefaultPosition();
            }
        }


        if(current == gameStates.gameOn)
        {
            if (activePlayer)
                activePlayer.GetComponent<PlayerShooting>().ChangeGunEvent.AddListener(ChangeGun);

            if(!shoot_ongoing)
                turnRemainingTime -= Time.deltaTime;

            if (turnRemainingTime < 0)
            {
                changeTurn();
            }
        }


        if (current == gameStates.gameOver)
            turnRemainingTime = 0;



		if (Input.GetKeyDown (KeyCode.Escape) && current!=gameStates.gameOver) {
			if (current == gameStates.pause) {
				pauseScreenUI.SetActive(false);
				foreach (GameObject g in UI) {
					g.SetActive (true);
				}
				current = gameStates.gameOn;
				Time.timeScale = 1;
			} else {
				pauseScreenUI.SetActive(true);
				foreach (GameObject g in UI) {
					g.SetActive (false);
				}
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
		this.GetComponent<ShowSuddenDeath>().SuddenDeath();
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

    public void SetLoserOnAnimation(Transform pos) {
        MatchProgressBar m = GetComponent<MatchProgressBar>(); //sorry i put it there, lazy to change it
        GameObject L = new GameObject();

        //si ha guanyat team 1, el loser sera del team2
        string loserFaction = (winner == 1) ? GamePreferences.p2_faction : GamePreferences.p1_faction;
        switch (loserFaction) {
            case "pirates":
                L = Instantiate(m.pirate, pos);
                break;
            case "vikings":
                L = Instantiate(m.viking, pos);
                break;
            case "knight":
                L = Instantiate(m.knight, pos);
                break;
        }

        L.transform.position = pos.position;
        L.transform.rotation = pos.rotation;
        L.transform.localScale = pos.localScale;
    }
}
