using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMainPlayer : MonoBehaviour {

	private float index;
	public float speed = 2.8f;
	public float ampli = 0.75f;

	private float pos_z = 0;

	// Use this for initialization
	void Start () {
		
	}

	public void EnableMain(bool enable){		
		if (enable) {
			transform.position = new Vector3(transform.position.x,transform.position.y, -0.5f);
		} else {
			transform.position = new Vector3(transform.position.x,transform.position.y, pos_z);
		}
		this.enabled = enable;
	}

	public void Update(){
		index += Time.deltaTime;
		float y = transform.parent.position.y + ampli*2 + Mathf.Abs (ampli*Mathf.Sin (speed*index));
		transform.position = new Vector3(transform.parent.position.x,y,transform.position.z);

		//print (transform.position);
	}

	public void setZ(float value){
		pos_z = value;
	}
}
