using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitUsages : MonoBehaviour {

	public GameObject bowUsagesT1;
	public GameObject bowUsagesT2;

	public GameObject grenadeT1;
	public GameObject grenadeT2;

	public GameObject panelT1;
	public GameObject panelT2;

	public GameObject panel;

	private IEnumerator coroutine;
	private int ang=0;

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

		panelT1.SetActive (true);
		panelT2.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		float speed = 100F;
		panel.transform.rotation = Quaternion.RotateTowards (
			panel.transform.rotation, Quaternion.Euler (ang, 0, 0), Time.deltaTime * speed);	

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


		
	public void SetPanel(int team){
		if (team == 1) {
			ang = 0;
		} else {
			ang = -180;
		}

		coroutine = WaitForIt(ang);
		StartCoroutine (coroutine);

	}

	IEnumerator WaitForIt(int angle){
		yield return new WaitForSecondsRealtime (1f);
		if (angle == -180) {			
			panelT2.SetActive (true);
			panelT1.SetActive (false);
		} else {
			panelT1.SetActive (true);
			panelT2.SetActive (false);
		}
	}
}
