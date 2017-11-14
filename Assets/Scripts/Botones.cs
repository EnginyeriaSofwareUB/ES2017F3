using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Botones : MonoBehaviour {


	public void LoadScene() {
        SceneManager.LoadScene("Test_Game", LoadSceneMode.Single);
	}

	public void LoadMenu() {
		SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
	}
	

	public void BotonSalir() {
		Application.Quit();
	}
}

