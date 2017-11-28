using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitialization : MonoBehaviour {

	// List of pieces that can be destroyed
	public List<GameObject> destructiblePieces;

	[Range(0,3)]
	public float minDistance =1f;

	private List<GameObject> piecesAdded;


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

	private bool checkNear(GameObject piece){
		foreach(GameObject p in piecesAdded){
			if (Vector3.Distance (piece.transform.position, p.transform.position) < 1f) {
				return false;
			}
		}
		return true;
	}

	void Start () {		

		piecesAdded = new List<GameObject> ();

		for (int i = 0; i < numMedical; i++) {

			// Pick a random piece and destoy it
			var piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];

			while (!checkNear (piece)) {
				piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];
			}

			Destroy (piece.gameObject);

			// Bring the medical to the front
			Vector3 pos = piece.transform.position;
			pos.z -= 0.2f;

			Instantiate (
				         medical,
				         pos,
				Quaternion.identity);
		
			destructiblePieces.Remove (piece);
			piecesAdded.Add (piece);
		}

		for (int i = 0; i < numAmmunition; i++) {

			// Pick a random piece and destoy it
			var piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];

			while (!checkNear (piece)) {
				piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];
			}

			Destroy (piece.gameObject);

			Instantiate (
				ammunition,
				piece.transform.position,
				Quaternion.identity);

			destructiblePieces.Remove (piece);
			piecesAdded.Add (piece);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
