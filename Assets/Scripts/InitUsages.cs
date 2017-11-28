using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitUsages : MonoBehaviour {

	public GameObject bowUsagesT1;
	public GameObject grenadeT1;
	public GameObject bowUsagesT2;
	public GameObject grenadeT2;

	// Use this for initialization
	void Start () {

		foreach (Gun g in GameObject.FindGameObjectWithTag ("GM").GetComponent<GameController> ().AvailableGuns) {
			switch (g.name) {
			case "Bow and Arrow":
				bowUsagesT1.GetComponent<UnityEngine.UI.Text> ().text = g.InitialUsagesLeft.ToString();
				bowUsagesT2.GetComponent<UnityEngine.UI.Text> ().text = g.InitialUsagesLeft.ToString();
				break;
			case "Grenade Base":
				grenadeT1.GetComponent<UnityEngine.UI.Text> ().text = g.InitialUsagesLeft.ToString();
				grenadeT2.GetComponent<UnityEngine.UI.Text> ().text = g.InitialUsagesLeft.ToString();
				break;
			}				
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetBowUsages(int team, int value){
		if (team == 1) {
			bowUsagesT1.GetComponent<UnityEngine.UI.Text> ().text = value.ToString ();
		} else {
			bowUsagesT2.GetComponent<UnityEngine.UI.Text> ().text = value.ToString ();
		}
	}

	public void SetGrenadeUsages(int team, int value){
		if (team == 1) {
			grenadeT1.GetComponent<UnityEngine.UI.Text> ().text = value.ToString ();
		} else {
			grenadeT2.GetComponent<UnityEngine.UI.Text> ().text = value.ToString ();
		}
	}
}
