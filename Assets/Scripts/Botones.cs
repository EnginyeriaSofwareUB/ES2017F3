using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour {

	public GameObject factions;
	public GameObject suddenBool;
	public GameObject suddenTurns;
	public GameObject maxLife;
    public GameObject playersTeam;

	public void LoadScene() {
		GetGamePreferences();
        SceneManager.LoadScene("Test_Game", LoadSceneMode.Single);
	}

	public void LoadMenu() {
		SceneManager.LoadScene("Main_Menu_Integrated", LoadSceneMode.Single);
	}

	public void BotonSalir() {
		Application.Quit();
	}

	// Gets all the Needed elems from the menu and saves it to 'GamePreferences' script, so GameController can access it.
	private void GetGamePreferences() {
		foreach (Transform t in factions.GetComponentsInChildren<Transform>(true)) {
			GameObject f = t.gameObject;
			if (f.name.Contains ("p1RBon") && f.activeSelf) {
				if (f.name.Contains("Vikings")) {
					GamePreferences.p1_faction = "vikings"; 
				} else if (f.name.Contains("Pirates")) {
					GamePreferences.p1_faction = "pirates";
				} else if (f.name.Contains("Knights")){
					GamePreferences.p1_faction = "knights";
				}
			}

			if (f.name.Contains ("p2RBon") && f.activeSelf) {
				if (f.name.Contains("Vikings")) {
					GamePreferences.p2_faction = "vikings"; 
				} else if (f.name.Contains("Pirates")) {
					GamePreferences.p2_faction = "pirates";
				} else if (f.name.Contains("Knights")){
					GamePreferences.p2_faction = "knights";
				}
			}
		}

		GamePreferences.sudden_death_activated = suddenBool.GetComponent<Toggle>().isOn; 
		GamePreferences.sudden_death_turns = int.Parse(suddenTurns.GetComponent<InputField>().text);
		GamePreferences.players_maxlife = int.Parse(maxLife.GetComponent<InputField>().text);
        int nplayers = int.Parse(playersTeam.GetComponent<InputField>().text);
        if (nplayers <= 4)
        {
            GamePreferences.number_players_team = nplayers;
        } else
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

        public void SetSuddenTurnsInteractable()
    {
        suddenTurns.GetComponent<InputField>().interactable = suddenBool.GetComponent<Toggle>().isOn;
    }
}

