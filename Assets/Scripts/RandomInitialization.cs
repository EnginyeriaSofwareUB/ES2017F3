using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitialization : MonoBehaviour {

	// List of pieces that can be destroyed
	public List<GameObject> destructiblePieces;

	// Num of mdeical objects in the scenario
	public int numMedicalObjects;

	// Game object to replace pieces
	public GameObject medical;

	private List<GameObject> removedPiece;
	private List<GameObject> medicalAdded;

	void Start () {		

		removedPiece = new List<GameObject> ();
		medicalAdded = new List<GameObject> ();

		for (int i = 0; i < numMedicalObjects; i++) {

			// Pick a random piece and destoy it
			var piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];
			Destroy (piece.gameObject);

			// Bring the medical to the front
			Vector3 pos = piece.transform.position;
			pos.z -= 0.5f;

			var med = (GameObject)Instantiate (
				         medical,
				         pos,
				         piece.transform.rotation);
		
			destructiblePieces.Remove (piece);
			removedPiece.Add (piece);
			medicalAdded.Add (med);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
