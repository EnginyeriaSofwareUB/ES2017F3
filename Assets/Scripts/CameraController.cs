using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameController _controller;
	private FollowingCamera _followingMode;
	private MovementCamera _mapMode;
	private Matrix4x4 _perspectiveMatrix;
	private float _transitionStartTime;
	private Vector3 _perspectivePoint;
	private IEnumerator _transitionCoroutine = TransitionCameraPerspective(Matrix4x4.identity, Vector3.back, 0, null, null);
	
	public bool Follow = true;
	public float PerspectiveTransitionTime = 0.4f;
	private Vector3 _minimapPoint;
	public float MinimapWidth;

    bool startdone = false;
    bool enddone = false;
    public Transform loserPosition;

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

        //_followingMode.target = _controller.activePlayer;
        //Invoke("SetPlayerTargetFirstTime", 0.75f); //Intencio: conseguir la camara fen un travelling del fondo al nivell jugable abans de comensar

		_minimapPoint = GameObject.FindGameObjectWithTag("Minimap Point").GetComponent<Transform>().position;
	}

    public void SetPlayerTargetFirstTime()
    {
        _followingMode.target = _controller.activePlayer;
        startdone = true;
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
		cameraMode.enabled = true;
		oldCameraMode.enabled = false;
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
			var t = (Time.time - startTime) / actualTransitionTime;
			var mat = new Matrix4x4();
			for (var i = 0; i < 16; i++)
			{
				mat[i] = Mathf.SmoothDamp(Camera.main.projectionMatrix[i], to[i], ref currentTransitionSpeed[i], transitionTime);
			}
			Camera.main.projectionMatrix = mat;
			Camera.main.transform.position = Vector3.Slerp(startPos, position, t);
			if (Camera.main.GetComponent<FollowingCamera>().enabled) {
				Camera.main.ResetProjectionMatrix();
			}
			yield return null;
		}
	}
	
    public void EndCameraStartAnimation()
    {
        _controller.StartGame();
    }

    public void EndCameraEndAnimation()
    {
        GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    private void Update () {
        // Update target
        if(startdone && !enddone)
            _followingMode.target = _controller.activePlayer; //TODO: Desde el controler, al canvi de torn es pot canviar el active player del following camera; evitem accedir a cada frame

        if (_controller.current.Equals(GameController.gameStates.gameOver) && !enddone) //si es gameover i no sa fet la animacio final, es fa
        {
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("end");

            _controller.SetLoserOnAnimation(loserPosition);

            enddone = true;
        }

        if (enddone)
            _followingMode.target = null;

        // Pressing Tab Key makes lock/unlock character
        if (Input.GetKeyDown(KeyCode.Tab))
		{
			StopCoroutine(_transitionCoroutine);
			if (_mapMode.enabled)
			{
				// We divide by 4 because the zoom back goes far quicker for some reason, so it takes less time
				_transitionCoroutine = TransitionCameraPerspective(_perspectiveMatrix, _perspectivePoint,
					PerspectiveTransitionTime / 4, _followingMode, _mapMode);
				StartCoroutine(_transitionCoroutine);

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
				_transitionCoroutine = TransitionCameraPerspective(GetOrtographicMatrix(), _minimapPoint, PerspectiveTransitionTime,
					_mapMode, _followingMode);
				StartCoroutine(_transitionCoroutine);

				if (activateFlags) {
					// Get all the players

					float pos = 0f;
					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {	
						
						GameObject flag;
						if (player.GetComponent<PlayerController> ().TEAM == 1) {						
							flag = (GameObject)Instantiate (
								flagTeam1,
								player.transform.position + new Vector3 (0, 1.5f, 0),
								Quaternion.identity);							
						
						} else {
							flag = (GameObject)Instantiate (
								flagTeam2,
								player.transform.position + new Vector3 (0, 1.5f, 0),
								Quaternion.identity);
							
						}

						flag.GetComponent<FlagMainPlayer> ().setZ (pos);
						flag.transform.parent = player.transform;
						flags.Add (flag);
						pos += 0.2f;


						if (player.Equals(_controller.activePlayer)) {
							player.GetComponentInChildren<FlagMainPlayer> ().EnableMain(true);
						} else {
							player.GetComponentInChildren<FlagMainPlayer> ().EnableMain(false);
						}
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


    public void CancelStartAnimation()
    {

    }
}
