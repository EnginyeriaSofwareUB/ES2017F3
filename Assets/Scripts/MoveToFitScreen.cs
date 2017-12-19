using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFitScreen : MonoBehaviour
{	
	// Use this for initialization
	void Awake ()
	{
		var minimapHeight = Camera.main.GetComponent<CameraController>().MinimapWidth / Camera.main.aspect;
		Debug.Log("Calculated height for minimap = " + minimapHeight);
		var newPosition = transform.position;
		var distanceToSea = Mathf.Abs(GameObject.FindWithTag("Sea").transform.position.y - newPosition.y);
		Debug.Log("Distance from Minimap to Sea in height = " + distanceToSea);
		newPosition.y += minimapHeight - distanceToSea + 0.5f; // Plus a bit to avoid the animation showing emptiness
		transform.position = newPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
