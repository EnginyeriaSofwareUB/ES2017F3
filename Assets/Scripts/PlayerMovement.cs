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
    private Animator anim;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
        anim = GetComponentInChildren<Animator>();
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

        // If jump pressed and in ground and not jumping
	    if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !anim.GetBool("jump"))
            // Perform jumping animation
            anim.SetBool("jump", true);

        if (horizontal != 0) {
            // Perform walking animation
            anim.SetBool("walking", true);
        }
        else {
            // Return to Idle
            anim.SetBool("walking", false);
        }

    }

    void FixedUpdate()
    {
		// Fixing rotation of the player so it's always facing forward
		Vector3 scale = model.localScale;
		if (horizontal < 0 && !facingRight) {
			scale.z = scale.z * -1.0f;
			facingRight = true;
		} else if (horizontal > 0 && facingRight) {
			scale.z = scale.z * -1.0f;
			facingRight = false;
		}
		model.localScale = scale;

        // Update horizontal movement
       	rb.velocity = new Vector3(horizontal, rb.velocity.y);

    }

    public void Idle() {
        if (anim == null) return;

        // Return to Idle
        anim.SetBool("walking", false);
    }

    public void Jump() {
        if (anim == null) return;

        anim.SetBool("jump", false);

        // ... add force in upwards.
        rb.AddForce(Vector3.up * m_JumpPower, ForceMode.Impulse);
    }

}
