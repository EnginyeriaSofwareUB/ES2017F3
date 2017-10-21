using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletDamage;

	// It is initialized here in order to prevent a trigger before Start function
	private List<GameObject> listTrigger = new List<GameObject> ();

	// It is initialized here in order to prevent a trigger before Start function
	private List<GameObject> playersPushback = new List<GameObject> ();

	// Use this for initialization
	void Start () {

        // Init variables

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check for collisions
    void OnCollisionEnter(Collision col)
    {
        // Check DesctructibleCube collision

		// 
		if (col.gameObject.tag == "DestructibleCube"){

			// Destroy all the blocks inside the trigger
			foreach (GameObject g in listTrigger) {
				Destroy (g);
			}

			// Pushback all the players inside the destructible zone
			foreach (GameObject play in playersPushback) {
				play.GetComponent<Rigidbody> ().AddForce (new Vector3(100,100,0));
			}
        }

        // Check Player collision
        if (col.gameObject.tag == "Player")
        {
            // Call the 'damage' function of the character collided
            col.gameObject.GetComponent<PlayerController>().Damage(bulletDamage);            
        }

        // Destroy the bullet
        Destroy(this.gameObject);
    }

	// Check for blocks to destroy
	void OnTriggerEnter(Collider col){		

		// Add all the blocks inside the trigger
		if (col.gameObject.tag=="DestructibleCube" && !listTrigger.Contains (col.gameObject)) {
			listTrigger.Add (col.gameObject);
		}

		// Add all the players inside the trigger to pushback
		if (col.gameObject.tag=="Player" && !playersPushback.Contains (col.gameObject)) {
			playersPushback.Add (col.gameObject);
		}
	}

	// Check for blocks to not destroy
	void OnTriggerExit(Collider col){		

		// Remove all the blocks that exit the trigger
		if (col.gameObject.tag=="DestructibleCube" && listTrigger.Contains (col.gameObject)) {
			listTrigger.Remove (col.gameObject);
		}

		// Remove all the players inside the trigger to pushback
		if (col.gameObject.tag=="Player" && playersPushback.Contains (col.gameObject)) {
			playersPushback.Remove (col.gameObject);
		}
	}
}
