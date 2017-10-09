using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check for collisions
    void OnCollisionEnter(Collision col)
    {
        // Check DesctructibleCube collision
        if (col.gameObject.tag == "DestructibleCube")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "character")
        {
            // Call the 'damage' function of the character collided

        }

        // Destroy the bullet
        Destroy(this.gameObject);
    }
}
