using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour {

	public int mDelta = 25; // Dead zone near the edges
	public float mSpeed = 5.0f; // Speed of the movement

	private Vector3 mRightDirection = Vector3.right; // Direction the camera should move when on the right edge
	private Vector3 mUpDirection = Vector3.up; // Direction the camera should move when on the upper edge

	[Space(5)]
	[Header("Limits")]

	public float min_X = 0f;
	public float max_X = 25f;
	public float min_Y = -0.5f;
	public float max_Y = 8f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		// Check if on the right edge
		if ((Input.mousePosition.x >= Screen.width - mDelta) && (transform.position.x<max_X)) {
			// Move the camera
			transform.position += mRightDirection * Time.deltaTime * mSpeed;

		// Check if on the left edge
		} else if ((Input.mousePosition.x <= 0 + mDelta) && (transform.position.x>min_X)) {
			// Move the camera
			transform.position -= mRightDirection * Time.deltaTime * mSpeed;

		// Check if on the upper edge
		} else if ((Input.mousePosition.y >= Screen.height - mDelta) && (transform.position.y<max_Y)) {
			// Move the camera
			transform.position += mUpDirection * Time.deltaTime * mSpeed;

		// Check if on the down edge
		} else if ((Input.mousePosition.y <= 0 + mDelta) && (transform.position.y>min_X)) {
			// Move the camera
			transform.position -= mUpDirection * Time.deltaTime * mSpeed;
		}			

	}
}