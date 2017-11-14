using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

	public enum weatherState {
		CALM,
		CLOUDY,
		STORMY,
	}; 

	public weatherState current = weatherState.CALM;
	public float cloudSpawnTimeCloudy = 1.3f;
	public float cloudSpawnTimeStormy = 0.4f;

	private float cloudDelay = 0.0f;
	public List<GameObject> cloudModels;
	public List<GameObject> clouds;

	// Use this for initialization
	void Start () {
		clouds = new List<GameObject> ();

		// Spawn a Cloud (instantiate CloudController, rigidbody and GameObject).
		if (current != weatherState.CALM) {
			int c = Random.Range (0, cloudModels.Count);
			GameObject cloud = Instantiate<GameObject> (cloudModels.ToArray()[c]);
			cloud.AddComponent<Rigidbody> ();
			cloud.AddComponent<CloudController> ();

			clouds.Add (cloud);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (current) {

			case weatherState.CALM:
				// No need fore clouds if it's calm. 
				break;

			case weatherState.CLOUDY:
				if (clouds.Count <= 5) {

					cloudDelay += Time.deltaTime;
					if (cloudDelay >= cloudSpawnTimeCloudy) {
						cloudDelay = 0.0f;

						// Spawn a Cloud (instantiate CloudController, rigidbody and GameObject).
						int c = Random.Range (0, cloudModels.Count);
						GameObject cloud = Instantiate<GameObject> (cloudModels.ToArray()[c]);
						cloud.AddComponent<Rigidbody> ();
						cloud.AddComponent<CloudController> ();

						clouds.Add (cloud);
					}
				}

				break;

			case weatherState.STORMY:
				if (clouds.Count <= 25) {
					
					cloudDelay += Time.deltaTime;
					if (cloudDelay >= cloudSpawnTimeStormy) {
						cloudDelay = 0.0f;

						// Spawn a Cloud (instantiate CloudController, rigidbody and GameObject).
						int c = Random.Range (0, cloudModels.Count);
						GameObject cloud = Instantiate<GameObject> (cloudModels.ToArray()[c]);
						cloud.AddComponent<Rigidbody> ();
						cloud.AddComponent<CloudController> ();

						clouds.Add (cloud);
					}
				}

				break;
		};


	}
}
