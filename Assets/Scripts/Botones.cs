using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Botones : MonoBehaviour {


	public void LoadScene() {
		Application.LoadLevel("BasicShooting_Test");
	}
	

	void BotonSalir () {
		Application.Quit();
	}
}

