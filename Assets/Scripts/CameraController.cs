﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameController _controller;
	private FollowingCamera _followingMode;
	private MovementCamera _mapMode;
	private Matrix4x4 _perspectiveMatrix;
	private float _transitionStartTime;
	private Vector3 _perspectivePoint;
	
	public bool Follow = true;
	public float PerspectiveTransitionTime = 0.4f;
	private Vector3 _minimapPoint;
	public float MinimapWidth;

	[Space(5)]
	[Header("Flags")]
	public bool activateFlags = true;
	public GameObject flagTeam1;
	public GameObject flagTeam2;
	private List<GameObject> flags;


	private void Awake () {
		// Acceso al Controlador, Camara y guardamos la referencia al objeto Jugador.
		_controller = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
		_followingMode = Camera.main.GetComponent<FollowingCamera> ();
		_mapMode = Camera.main.GetComponent<MovementCamera> ();

		_followingMode.target = _controller.activePlayer;
		_minimapPoint = GameObject.FindGameObjectWithTag("Minimap Point").GetComponent<Transform>().position;
	}

	// Use this for initialization
	void Start () {
		if (Follow) {
			_mapMode.enabled = false;
			_followingMode.enabled = true;
		} else {
			_mapMode.enabled = true;
			_followingMode.enabled = false;
		}

		flags = new List<GameObject> ();
	}

	private static IEnumerator TransitionCameraPerspective(Matrix4x4 to, Vector3 position, float transitionTime, Behaviour cameraMode, Behaviour oldCameraMode)
	{
		var startTime = Time.time;
		float[] currentTransitionSpeed = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		var startPos = Camera.main.transform.position;
		// 4 is here because it takes around 4 * T seconds for the result to get where we want
		// even though the official documentation specifies that SmoothDamp will take
		// approximately T time to get there
		var actualTransitionTime = 4 * transitionTime;
		// Essentially matrix interpolation until we reach the target
		while (Time.time < startTime + actualTransitionTime)
		{
			var mat = new Matrix4x4();
			for (var i = 0; i < 16; i++)
			{
				mat[i] = Mathf.SmoothDamp(Camera.main.projectionMatrix[i], to[i], ref currentTransitionSpeed[i], transitionTime);
			}
			Camera.main.projectionMatrix = mat;
			Camera.main.transform.position = Vector3.Slerp(startPos, position, (Time.time - startTime)/actualTransitionTime);
			yield return null;
		}
		cameraMode.enabled = true;
		oldCameraMode.enabled = false;
	}
	
	// Update is called once per frame
	private void Update () {
        // Update target
        _followingMode.target = _controller.activePlayer; //TODO: Desde el controler, al canvi de torn es pot canviar el active player del following camera; evitem accedir a cada frame

		// Pressing Tab Key makes lock/unlock character
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (_mapMode.enabled)
			{
				// We divide by 4 because the zoom back goes far quicker for some reason, so it takes less time
				StartCoroutine(TransitionCameraPerspective(_perspectiveMatrix, _perspectivePoint, PerspectiveTransitionTime / 4,
					_followingMode, _mapMode));

				if (activateFlags) {					
					foreach (GameObject flag in flags) {
						Destroy (flag);
					}
				}
			}
			else
			{
				_perspectiveMatrix = Camera.main.projectionMatrix;
				_perspectivePoint = Camera.main.transform.position;
				_followingMode.enabled =
					false; // following camera is always updating the position, impedes the lerp to minimap point
				Camera.main.ResetProjectionMatrix();
				StartCoroutine(TransitionCameraPerspective(GetOrtographicMatrix(), _minimapPoint, PerspectiveTransitionTime, _mapMode,
					_followingMode));

				if (activateFlags) {
					// Get all the players
					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {	

						GameObject flag;

						// We 
						if (player.GetComponent<PlayerController> ().TEAM == 1) {						
							flag = (GameObject)Instantiate (
								flagTeam1,
								player.transform.position + new Vector3 (0, 1, 0),
								player.transform.rotation);
						
						} else {
							flag = (GameObject)Instantiate (
								flagTeam2,
								player.transform.position + new Vector3 (0, 1, 0),
								player.transform.rotation);
						}

						flag.transform.parent = player.transform;
						flags.Add (flag);
					}
				}
			}
		}
	}

	private Matrix4x4 GetOrtographicMatrix()
	{
		var aspectRatio = Camera.main.aspect;
		return Matrix4x4.Ortho(
			-MinimapWidth, MinimapWidth, -MinimapWidth / aspectRatio, MinimapWidth / aspectRatio,
			Camera.main.nearClipPlane, Camera.main.farClipPlane);
	}
}
