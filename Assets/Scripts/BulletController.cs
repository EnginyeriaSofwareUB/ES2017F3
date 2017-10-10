using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private float bulletDamage;

	// Use this for initialization
	void Start () {

        // Init variables
        bulletDamage = 20f;

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

        // Check Player collision
        if (col.gameObject.tag == "Player")
        {
            // Call the 'damage' function of the character collided
            col.gameObject.GetComponent<PlayerController>().Damage(bulletDamage);
            Destroy(this.gameObject);
        }

        // Destroy the bullet
        // Destroy(this.gameObject);
    }
}
