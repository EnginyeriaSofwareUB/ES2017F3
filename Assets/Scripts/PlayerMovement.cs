using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;

    public float horizontal;
	public Transform model;
	private bool jump;
	private bool isGrounded;
    private const float m_JumpPower = 6f; // The force added to the ball when it jumps.
	private bool facingRight = false;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	// If any character is touching any "destructibleCube", it will allow them to jump.
	void OnCollisionStay() {
		isGrounded = true;
	}

	void OnCollisionExit() {
		isGrounded = false;
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
		// Fixing rotation of the player so it's always facing forward
		Vector3 scale = model.localScale;
		if (horizontal < 0 && !facingRight) {
			scale.x = scale.x * -1.0f;
			facingRight = true;
		} else if (horizontal > 0 && facingRight) {
			scale.x = scale.x * -1.0f;
			facingRight = false;
		}
		model.localScale = scale;

        // Update horizontal movement
       	rb.velocity = new Vector3(horizontal, rb.velocity.y);

		// If on the ground and jump is pressed...
		if (jump && isGrounded) {
			// ... add force in upwards.
			rb.AddForce(Vector3.up * m_JumpPower, ForceMode.Impulse);
			// Jump done
			jump = false;
		}
    }
}
