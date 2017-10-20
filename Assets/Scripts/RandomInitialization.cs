using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitialization : MonoBehaviour {

	public List<GameObject> destructiblePieces;
	public int numMedicalObjects;
	public GameObject medical;

	private List<GameObject> removedPiece;
	private List<GameObject> medicalAdded;

	// Use this for initialization
	void Start () {		

		removedPiece = new List<GameObject> ();
		medicalAdded = new List<GameObject> ();

		for (int i = 0; i < numMedicalObjects; i++) {
			
			var piece = destructiblePieces [Random.Range (0, destructiblePieces.Count)];
			Destroy (piece.gameObject);

			Debug.Log (piece.name);
			var med = (GameObject)Instantiate (
				         medical,
				         piece.transform.position,
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
