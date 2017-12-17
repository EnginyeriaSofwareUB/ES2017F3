using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {
    GameController control;

	public GameObject target;
    public GameObject bullet_target;
    public Transform bullet_lastPos;

    [Header("Camera sets")]
	public float offset = 20.0f;
	public float height = 2.0f;
    public float offsetX = 1f;
	public float speed = 3.0f;

	[Header("Smooth-Follow Camera")]
	public bool activateSmooth = false;

	private CameraController _cameraController;
	
	// Use this for initialization
	void Start () {
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();

        if (target)
		    transform.position = new Vector3 (target.transform.position.x, (target.transform.position.y + 2), transform.position.z);

		_cameraController = GetComponent<CameraController>();
	}


    // LateUpdate is called once per frame
    void LateUpdate () {

		if (target && !control.shoot_ongoing) {
			// Camera Smooth Damping for a nicer follow.
			if (activateSmooth) {

				Vector3 pos = transform.position;
				pos.x = Mathf.Lerp(transform.position.x, target.transform.position.x + offsetX, (speed * Time.deltaTime));
				pos.y = Mathf.Lerp(transform.position.y, (target.transform.position.y + height), (speed * Time.deltaTime));
				pos.z = target.transform.position.z - offset;

				transform.position = pos;

			// Camera basic follow.
			} else {
				transform.position = new Vector3(target.transform.position.x, target.transform.position.y + height, target.transform.position.z - offset);
			}

			if (!_cameraController.IsMinimapEnabled())
			{
				Vector3 eulerRot;
				if (target.GetComponent<PlayerController>().TEAM == 1)
				{
					eulerRot = new Vector3(15f, 15f, 2.5f);
					offsetX = -1f;
				}
				else
				{
					eulerRot = new Vector3(15f, -15f, 2.5f);
					offsetX = 1f;
				}

				transform.rotation = Quaternion.Euler(eulerRot);
			}

		}

        if(control.shoot_ongoing && bullet_target)
        {
            if (activateSmooth)
            {

                Vector3 pos = transform.position;
                pos.x = Mathf.Lerp(transform.position.x, bullet_target.transform.position.x, (speed * Time.deltaTime));
                pos.y = Mathf.Lerp(transform.position.y, (bullet_target.transform.position.y + height), (speed * Time.deltaTime));
                pos.z = bullet_target.transform.position.z - offset;

                transform.position = pos;

                // Camera basic follow.
            }
            else
            {
                transform.position = new Vector3(bullet_target.transform.position.x, bullet_target.transform.position.y + height, bullet_target.transform.position.z - offset);
            }

            bullet_lastPos = bullet_target.transform;

        }else if(control.shoot_ongoing && !bullet_target)
        {
            if (activateSmooth)
            {

                Vector3 pos = transform.position;
                pos.x = Mathf.Lerp(transform.position.x, bullet_lastPos.transform.position.x, (speed * Time.deltaTime));
                pos.y = Mathf.Lerp(transform.position.y, (bullet_lastPos.transform.position.y + height), (speed * Time.deltaTime));
                pos.z = bullet_lastPos.transform.position.z - offset;

                transform.position = pos;

                // Camera basic follow.
            }
            else
            {
                transform.position = new Vector3(bullet_lastPos.transform.position.x, bullet_lastPos.transform.position.y + height, bullet_lastPos.transform.position.z - offset);
            }
        }
	}
}
