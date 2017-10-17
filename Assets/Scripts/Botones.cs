using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Botones : MonoBehaviour {


	public void LoadScene() {
        SceneManager.LoadScene("Test_Game", LoadSceneMode.Single);
	}
	

	void BotonSalir () {
		Application.Quit();

	}
}

