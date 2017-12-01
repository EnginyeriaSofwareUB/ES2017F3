using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HowTo : MonoBehaviour {

	private GameObject controller;
	public GameObject ap;

	private IEnumerator coroutine;

	public GameObject[] texts;

	private bool move = false;
	private bool jump = false;

	private int cont = 0;
	private int tabs = 0;

	private Vector3 pos_ini;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("GM");

		coroutine = BlockWeapons();
		StartCoroutine (coroutine);

		foreach (GameObject g in texts) {
			g.SetActive (false);
		}
		texts[0].SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.GetComponent<GameController> ().turnRemainingTime < 1) {
			controller.GetComponent<GameController> ().turnRemainingTime = 20;
		}

		if (Input.GetKeyUp (KeyCode.Tab)) {
			tabs++;
		}

		if (cont>0 && ap.transform.position.y < -8f) {
			ap.transform.position = pos_ini;
		}

		switch (cont) {
		case 0:
			if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.W) || 
				Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)){

				move = true;
			}

			if (Input.GetKeyUp(KeyCode.Space)){
				jump = true;
			}

			if (jump && move) {				
				cont++;
			}
			break;

		case 1:
			coroutine = CameraInfo ();
			StartCoroutine (coroutine);
			cont++;
			break;

		case 2:
			if (Input.GetKeyUp (KeyCode.Tab)) {
				coroutine = TimeWind ();
				StartCoroutine (coroutine);
				cont++;
			}
			break;

		case 3:
			print(tabs);
			if (Input.GetKeyUp (KeyCode.Tab) && tabs % 2 == 0) {
				ap.GetComponent<PlayerShooting> ().enabled = true;
				coroutine = ShootTime ();
				StartCoroutine (coroutine);
				cont++;

			} else if (tabs == 0) {
				cont++;
			}
			break;

		case 4:
			if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) 
				|| Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6)){
				coroutine = Weapon ();
				StartCoroutine (coroutine);
				cont++;
			}
			break;

		case 5:
			if (Input.GetMouseButtonDown(0)){
				coroutine = FinalInfo ();
				StartCoroutine (coroutine);
			}
			break;
		}

	}

	IEnumerator BlockWeapons(){		
		yield return new WaitForSeconds (1);	
		ap = controller.GetComponent<GameController> ().activePlayer;
		ap.GetComponent<PlayerShooting> ().enabled = false;
		pos_ini = ap.transform.position;
	}

	IEnumerator CameraInfo(){		
		yield return new WaitForSeconds (1);	
		texts[0].SetActive (false);
		yield return new WaitForSeconds (1);
		texts [1].SetActive (true);
	}

	IEnumerator TimeWind(){		
		yield return new WaitForSeconds (1);	
		texts[1].SetActive (false);
		yield return new WaitForSeconds (1);
		texts [2].SetActive (true);
		yield return new WaitForSeconds (6);
		texts [2].SetActive (false);
		yield return new WaitForSeconds (1);
		texts [3].SetActive (true);
		yield return new WaitForSeconds (3);
		texts [3].SetActive (false);
		if (tabs % 2 == 1) {
			yield return new WaitForSeconds (1);
			texts [4].SetActive (true);
		}
	}

	IEnumerator ShootTime(){	
		yield return new WaitForSeconds (1);
		texts [4].SetActive (false);	
		yield return new WaitForSeconds (1);	
		texts [5].SetActive (true);
	}

	IEnumerator Weapon(){	
		yield return new WaitForSeconds (1);
		texts [5].SetActive (false);	
		yield return new WaitForSeconds (1);	
		texts [6].SetActive (true);
	}

	IEnumerator FinalInfo(){	
		yield return new WaitForSeconds (1);
		texts [6].SetActive (false);	
		yield return new WaitForSeconds (1);	
		texts [7].SetActive (true);
		yield return new WaitForSeconds (6);	
		SceneManager.LoadScene ("Main_Menu_Integrated", LoadSceneMode.Single);

	}



}
