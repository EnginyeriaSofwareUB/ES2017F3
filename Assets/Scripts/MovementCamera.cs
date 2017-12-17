using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour {

	public int MovementDelta = 25; // Dead zone near the edges
	public float MovementSpeed = 5.0f; // Speed of the movement

	private readonly Vector3 _mRightDirection = Vector3.right; // Direction the camera should move when on the right edge
	private readonly Vector3 _mUpDirection = Vector3.up; // Direction the camera should move when on the upper edge

	[Space(5)]
	[Header("Limits")]

	public float MinX = 0f;
	public float MaxX = 25f;

	private GameObject _minimapPoint;
	private float _minY = 1f;
	private float _maxY = 8f;

	// Use this for initialization
	void Start ()
	{
		_minimapPoint = GameObject.FindGameObjectWithTag("Minimap Point");
		_minY = _minimapPoint.transform.position.y;
		_maxY = _minY + 7;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		// Check if on the right edge
		if ((Input.mousePosition.x >= Screen.width - MovementDelta) && (transform.position.x<MaxX)) {
			// Move the camera
			transform.position += _mRightDirection * Time.deltaTime * MovementSpeed;

		// Check if on the left edge
		} else if ((Input.mousePosition.x <= 0 + MovementDelta) && (transform.position.x>MinX)) {
			// Move the camera
			transform.position -= _mRightDirection * Time.deltaTime * MovementSpeed;

		// Check if on the upper edge
		} else if ((Input.mousePosition.y >= Screen.height - MovementDelta) && (transform.position.y<_maxY)) {
			// Move the camera
			transform.position += _mUpDirection * Time.deltaTime * MovementSpeed;

		// Check if on the down edge
		} else if ((Input.mousePosition.y <= 0 + MovementDelta) && (transform.position.y>_minY)) {
			// Move the camera
			transform.position -= _mUpDirection * Time.deltaTime * MovementSpeed;
		}			

	}
}