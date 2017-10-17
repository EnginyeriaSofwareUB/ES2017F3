using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Botones : MonoBehaviour {


	public void LoadScene() {
		Application.LoadLevel("Test_Game 1");
	}
	

	void BotonSalir () {
		Application.Quit();

	}
}

