using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitialization : MonoBehaviour {

	// List of pieces that can be destroyed
	public List<GameObject> destructiblePieces;

	[Space(5)]
	[Header("Medical")]
	// Num of medical objects in the scenario
	public int numMedical =5;

	// Game object to replace pieces
	public GameObject medical;

	[Space(5)]
	[Header("Ammunition")]
	// Num of ammunition objects in the scenario
	public int numAmmunition =5;

	// Game object to replace pieces
	public GameObject ammunition;

	void Start () {		

		for (int i = 0; i < numMedical; i++) {

			// Pick a random piece and destoy it
			var piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];
			Destroy (piece.gameObject);

			// Bring the medical to the front
			Vector3 pos = piece.transform.position;
			pos.z -= 0.2f;

			var med = (GameObject)Instantiate (
				         medical,
				         pos,
				Quaternion.identity);
		
			destructiblePieces.Remove (piece);
		}

		for (int i = 0; i < numAmmunition; i++) {

			// Pick a random piece and destoy it
			var piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];
			Destroy (piece.gameObject);

			var amm = (GameObject)Instantiate (
				ammunition,
				piece.transform.position,
				Quaternion.identity);

			destructiblePieces.Remove (piece);
		}
	}

//	void Start(){
//		GameObject[] lst = GameObject.FindGameObjectsWithTag ("DestructibleCube");
//		foreach (GameObject g in lst) {
//			var pos = g.transform.position.z;
//			if (pos < -0.6f) {
//				Destroy (g);
//			}
//		}
//	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
