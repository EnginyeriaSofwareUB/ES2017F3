using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private GameObject target;
	private Rigidbody rb;
	private WeatherController wc;

	public int velocity = 12;
	public Vector3 startPosition = new Vector3(-40, 10, 0);

	public int minZ = -10;
	public int maxZ = 10;
	public int endX = 40;

	// Awake call
	void Awake () {
		startPosition = new Vector3(startPosition.x, startPosition.y, Random.Range(minZ, maxZ));
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		wc = GameObject.FindGameObjectWithTag("GM").GetComponent<WeatherController>();

		transform.position = startPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x <= endX) {
			rb.velocity = new Vector3(velocity, 0, 0);
		} else {
			wc.clouds.Remove (this.gameObject);
			Destroy (this.gameObject);
		}
	}
}
