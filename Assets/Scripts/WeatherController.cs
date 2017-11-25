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
	public float cloudSpawnTimeCloudy = 1.5f;
	public float cloudSpawnTimeStormy = 0.4f;
    public float cloudVelocity = 7f;
    public float cloudVelocityVariance = 0.2f; //for randomizing a bit the velocity
    public float windForceMultiplier = 1.2f; //relation between windForce and clouds velocity

	private float cloudDelay = 0.0f;
	public List<GameObject> cloudModels;
	public List<GameObject> clouds = new List<GameObject>();

    [Header("Spawn Points")]
    public Transform leftSpawn;
    public Transform rightSpawn;

	// Use this for initialization
	void Start () {
		//clouds = 

		// Spawn a Cloud (instantiate CloudController, rigidbody and GameObject).
        /*
		if (current != weatherState.CALM) {
			int c = Random.Range (0, cloudModels.Count);
			GameObject cloud = Instantiate<GameObject> (cloudModels.ToArray()[c]);
			cloud.AddComponent<Rigidbody> ();
			cloud.AddComponent<CloudController> ();

			clouds.Add (cloud);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (current) {

			case weatherState.CALM:
				// No need fore clouds if it's calm. 
				break;

			case weatherState.CLOUDY:
				if (clouds.Count <= 6) {

					cloudDelay += Time.deltaTime;
					if (cloudDelay >= cloudSpawnTimeCloudy) {
						cloudDelay = 0.0f;

						// Spawn a Cloud (instantiate CloudController, rigidbody and GameObject).
						int c = Random.Range (0, cloudModels.Count);
						GameObject cloud = Instantiate<GameObject> (cloudModels.ToArray()[c]);
						cloud.AddComponent<Rigidbody> ();
						//cloud.AddComponent<CloudController> ();

                        CloudController cc = cloud.GetComponent<CloudController>();
                        Rigidbody rig = cloud.GetComponent<Rigidbody>();

                        // Set the speed of the new cloud, with a lil variance
                        float vel = Random.Range(cloudVelocity - cloudVelocity * cloudVelocityVariance, cloudVelocity + cloudVelocity * cloudVelocityVariance);
                        cc.velocity = vel;

                        //Debug.Log(" WIND DIRECTION IS : " + GetComponent<WindController>().direction);
                        //Choose where to spawn the cloud, based on wind direction
                        if(GetComponent<WindController>().direction == 1)
                        {
                            //cc.startPosition = new Vector3(leftSpawn.position.x, Random.Range(leftSpawn.position.y- 1.2f, leftSpawn.position.y + 1.2f), leftSpawn.position.z);
                            cloud.transform.position = new Vector3(leftSpawn.position.x, Random.Range(leftSpawn.position.y - 1.2f, leftSpawn.position.y + 1.2f), leftSpawn.position.z); 
                            cc.dir = 1;
                        }
                        else
                        {
                            //cc.startPosition = new Vector3(rightSpawn.position.x, Random.Range(rightSpawn.position.y - 1.2f, rightSpawn.position.y+ 1.2f), rightSpawn.position.z); 
                            cloud.transform.position = new Vector3(rightSpawn.position.x, Random.Range(rightSpawn.position.y - 1.2f, rightSpawn.position.y + 1.2f), rightSpawn.position.z);
                            cc.dir = -1;
                        }
                        

                        rig.useGravity = false;

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
