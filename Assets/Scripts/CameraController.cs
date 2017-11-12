﻿using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameController _controller;
	private FollowingCamera _f;
	private MovementCamera _m;
	private Matrix4x4 _perspectiveMatrix, _orthographicMatrix;
	private float _transitionStartTime;
	private Vector3 _perspectivePoint;
	
	public bool Follow = true;
	public float PerspectiveTransitionTime = 0.4f;
	private Vector3 _minimapPoint;
	public float MinimapXSize, MinimapYSize;

	private void Awake () {
		// Acceso al Controlador, Camara y guardamos la referencia al objeto Jugador.
		_controller = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
		_f = Camera.main.GetComponent<FollowingCamera> ();
		_m = Camera.main.GetComponent<MovementCamera> ();

		_f.target = _controller.activePlayer;
		_orthographicMatrix = Matrix4x4.Ortho(
			-MinimapXSize, MinimapXSize, -MinimapYSize, MinimapYSize,
			Camera.main.nearClipPlane, Camera.main.farClipPlane);
		_minimapPoint = GameObject.FindGameObjectWithTag("Minimap Point").GetComponent<Transform>().position;
	}

	// Use this for initialization
	void Start () {
		if (Follow) {
			_m.enabled = false;
			_f.enabled = true;
		} else {
			_m.enabled = true;
			_f.enabled = false;
		}
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
        _f.target = _controller.activePlayer; //TODO: Desde el controler, al canvi de torn es pot canviar el active player del following camera; evitem accedir a cada frame

		// Pressing Tab Key makes lock/unlock character
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (_m.enabled)
				StartCoroutine(TransitionCameraPerspective(_perspectiveMatrix, _perspectivePoint, PerspectiveTransitionTime/4, _f, _m));
			else
			{
				_perspectiveMatrix = Camera.main.projectionMatrix;
				_perspectivePoint = Camera.main.transform.position;
				_f.enabled = false; // foloowing camera is always updating the position, impedes the lerp to minimap point
				Camera.main.ResetProjectionMatrix();
				StartCoroutine(TransitionCameraPerspective(_orthographicMatrix, _minimapPoint, PerspectiveTransitionTime, _m, _f));

			}
		}
	}
}
