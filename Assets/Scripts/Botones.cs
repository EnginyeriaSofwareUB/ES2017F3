using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    public bool IS_MENU = false;

    public GameObject factions;
    //public GameObject suddenBool;
    //public GameObject suddenTurns;
    //public GameObject maxLife;
    public GameObject playersTeam;

    [Header("Preview Faction")]
    public Transform f1_place;
    public Transform f2_place;
    string lastf1 = "";
    string lastf2 = "";
	string sceneToLoad = "Test_Game";

    //to load prefabs for the preview
    GameObject vikingUI;
    GameObject pirateUI;
    GameObject knightUI;
    GameObject a;
    GameObject b;
    private void Start()
    {
        vikingUI = Resources.Load("Prefabs/UI_Characters/Viking_Show") as GameObject;
        pirateUI = Resources.Load("Prefabs/UI_Characters/Pirate_Show") as GameObject;
        knightUI = Resources.Load("Prefabs/UI_Characters/Knight_Show") as GameObject;

        IS_MENU = SceneManager.GetActiveScene().name.ToLower().Contains("menu"); //check if is menu scene by name

        if(IS_MENU)
            FirstSelected();
    }

    public void LoadScene()
    {
        if (IS_MENU)
            GetGamePreferences();
        GamePreferences.howTo = false;
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

	public void SetLevel1(){
		sceneToLoad = "Test_Game";
	}

	public void SetLevel2(){
		sceneToLoad = "Test_Game_Level2";
	}

	public void LoadScene2(){
		SceneManager.LoadScene("Test_Game_Level2", LoadSceneMode.Single);
	}

	public void setSudden(bool sudden){
		GamePreferences.sudden_death_activated = sudden;
	}

	public void setGameMode(bool gameMode){


		if (gameMode) {
			// Normal
			GamePreferences.sudden_death_turns = 12;
			GamePreferences.players_maxlife = 100;
			
		} else {
			//Fast
			GamePreferences.sudden_death_turns = 6;
			GamePreferences.players_maxlife = 75;
		}
	}

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main_Menu_Integrated", LoadSceneMode.Single);
    }

	public void LoadHowTo()
	{
		GamePreferences.howTo = true;
		SceneManager.LoadScene("Test_Game", LoadSceneMode.Single);
	}

    public void BotonSalir()
    {
        Application.Quit();
    }

    //to avoid execute all preference catching when updating the faction previewer
    void GetFactionPreferences()
    {
        foreach (Transform t in factions.GetComponentsInChildren<Transform>(true))
        {
            GameObject f = t.gameObject;
            if (f.name.Contains("p1RBon") && f.activeSelf)
            {
                if (f.name.Contains("Vikings"))
                {
                    GamePreferences.p1_faction = "vikings";
                }
                else if (f.name.Contains("Pirates"))
                {
                    GamePreferences.p1_faction = "pirates";
                }
                else if (f.name.Contains("Knights"))
                {
                    GamePreferences.p1_faction = "knights";
                }
            }

            if (f.name.Contains("p2RBon") && f.activeSelf)
            {
                if (f.name.Contains("Vikings"))
                {
                    GamePreferences.p2_faction = "vikings";
                }
                else if (f.name.Contains("Pirates"))
                {
                    GamePreferences.p2_faction = "pirates";
                }
                else if (f.name.Contains("Knights"))
                {
                    GamePreferences.p2_faction = "knights";
                }
            }
        }
    }

    // Gets all the Needed elems from the menu and saves it to 'GamePreferences' script, so GameController can access it.
    private void GetGamePreferences()
    {

        GetFactionPreferences();

        //changed input for text fields as no longer needed to edit it by the player, just use arrows
        
		//GamePreferences.sudden_death_activated = suddenBool.GetComponent<Toggle>().isOn;
        //GamePreferences.sudden_death_turns = int.Parse(suddenTurns.GetComponent<Text>().text);
        //GamePreferences.players_maxlife = int.Parse(maxLife.GetComponent<Text>().text);
        
		int nplayers = int.Parse(playersTeam.GetComponent<Text>().text);
        if (nplayers <= 4)
        {
            GamePreferences.number_players_team = nplayers;
        }
        else
        {
            GamePreferences.number_players_team = 4;
        }
    }

    public void CheckNPlayersPerTeam()
    {
        int nplayers = int.Parse(playersTeam.GetComponent<InputField>().text);
        if (nplayers > 4)
        {
            playersTeam.GetComponent<InputField>().text = 4.ToString();
        }
    }

    //for first timne menu is showed
    public void FirstSelected()
    {
        if (f1_place && f2_place)
        {
            a = Instantiate(pirateUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
			b = Instantiate(vikingUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
            a.transform.localScale = new Vector3(a.transform.localScale.x * 400f, a.transform.localScale.y * 400f, a.transform.localScale.z * -400f);
            b.transform.localScale = new Vector3(b.transform.localScale.x * 400f, b.transform.localScale.y * 400f, b.transform.localScale.z * -400f);
            
            a.GetComponent<ColorTeam>().color = 1;
            b.GetComponent<ColorTeam>().color = 2;
        }

        lastf1 = "pirates";
        lastf2 = "vikings";
    }

    //to update the preview box
    //NOTE: Cause problems due to faction selection system
    public void OnChangeFaction()
    {
        if (f1_place && f2_place)
        {

            lastf1 = GamePreferences.p1_faction;
            lastf2 = GamePreferences.p2_faction;

            GetFactionPreferences();

            if (!lastf1.Equals(GamePreferences.p1_faction))
            {
                print("different 1");
                //reset
                Destroy(a);
                a = new GameObject();

                //new model
                switch (GamePreferences.p1_faction)
                {
                    case "pirates":
                        a = Instantiate(pirateUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
                        break;
                    case "knights":
                        a = Instantiate(knightUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
                        break;
                    case "vikings":
                        a = Instantiate(vikingUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
                        break;
                }
            }

            if (!lastf2.Equals(GamePreferences.p2_faction))
            {
                print("different 2");
                //reset
                Destroy(b);
                b = new GameObject();

                //new model
                switch (GamePreferences.p2_faction)
                {
                    case "pirates":
                        b = Instantiate(pirateUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
                        break;
                    case "knights":
                        b = Instantiate(knightUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
                        break;
                    case "vikings":
                        b = Instantiate(vikingUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
                        break;
                }
            }

            a.transform.localScale = new Vector3(a.transform.localScale.x * 400f, a.transform.localScale.y * 400f, a.transform.localScale.z * -400f);
            b.transform.localScale = new Vector3(b.transform.localScale.x * 400f, b.transform.localScale.y * 400f, b.transform.localScale.z * -400f);


        }


    }

    public void OnChangeFaction1()
    {
        if (f1_place)
        {

            lastf1 = GamePreferences.p1_faction;

            GetFactionPreferences();

            if (!lastf1.Equals(GamePreferences.p1_faction))
            {
                print("different 1");
                //reset
                Destroy(a);
                a = new GameObject();

                //new model
                switch (GamePreferences.p1_faction)
                {
                    case "pirates":
                        a = Instantiate(pirateUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
                        break;
                    case "knights":
                        a = Instantiate(knightUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
                        break;
                    case "vikings":
                        a = Instantiate(vikingUI, f1_place.position, f1_place.rotation, f1_place) as GameObject;
                        break;
                }

                //sets the color for the team
                a.GetComponent<ColorTeam>().color = 1;
            }

            a.transform.localScale = new Vector3(a.transform.localScale.x * 400f, a.transform.localScale.y * 400f, a.transform.localScale.z * -400f);

        }
    }

    public void OnChangeFaction2()
    {
        if (f2_place)
        {

            lastf2 = GamePreferences.p2_faction;

            GetFactionPreferences();

            if (!lastf2.Equals(GamePreferences.p2_faction))
            {
                print("different 2");
                //reset
                Destroy(b);
                b = new GameObject();

                //new model
                switch (GamePreferences.p2_faction)
                {
                    case "pirates":
                        b = Instantiate(pirateUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
                        break;
                    case "knights":
                        b = Instantiate(knightUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
                        break;
                    case "vikings":
                        b = Instantiate(vikingUI, f2_place.position, f2_place.rotation, f2_place) as GameObject;
                        break;
                }

                //sets the color for the team
                b.GetComponent<ColorTeam>().color = 2;
            }

            b.transform.localScale = new Vector3(b.transform.localScale.x * 400f, b.transform.localScale.y * 400f, b.transform.localScale.z * -400f);

        }
    }
}

