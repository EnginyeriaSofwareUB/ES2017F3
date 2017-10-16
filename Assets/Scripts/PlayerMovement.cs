using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;
    private float horizontal;
    private bool jump;
    private const float m_JumpPower = 10f; // The force added to the ball when it jumps.
    private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.


    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

        // Check movement Inputs
		horizontal = Input.GetAxis ("Horizontal") * 5f;
	    if (Input.GetKeyDown(KeyCode.Space))
	        jump = true;

	}

    void FixedUpdate()
    {
        // Update horizontal movement
        rb.velocity = new Vector3(horizontal, rb.velocity.y);

        // If on the ground and jump is pressed...
        if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength) && jump)
        {
            // ... add force in upwards.
            rb.AddForce(Vector3.up * m_JumpPower, ForceMode.Impulse);
            // Jump done
            jump = false;
        }
    }

}
