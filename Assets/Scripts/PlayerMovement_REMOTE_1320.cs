﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;

    public float horizontal;
	public Transform model;
	public float timeInterval = 0.5f; // Default backJump time detect.
	private bool jump;
	private bool isGrounded;
	private bool firstPress = false;
	private float timePress;
	private bool reset = false;
	private bool backJump = false;
    private const float m_JumpPower = 5f; // The force added to the ball when it jumps.
	private bool facingRight = false;

	public Vector3 jump_force;
	public Vector3 backJump_force;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();

		jump_force = new Vector3 (1.0f, 1.0f, 0.0f);
		backJump_force = new Vector3 (-0.5f, 1.5f, 0.0f);
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
		if (isGrounded) {
			if (Input.GetKeyDown(KeyCode.Space) && firstPress) {
				if (Time.time - timePress <= timeInterval) {
					Debug.Log("BackJump");
					backJump = true;
				} else {
					Debug.Log("Jump");
				}

				jump = true;
				reset = true;
			}

			if (Time.time - timePress > timeInterval && firstPress) {
				jump = true;
				reset = true;
			}

			if (Input.GetKeyDown(KeyCode.Space) && !firstPress) {
				firstPress = true;
				timePress = Time.time;
			}

			if(reset) {
				firstPress = false;
				reset = false;
			} 
		}
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

		// Udate horizontal movement
		//rb.velocity = new Vector3(horizontal, rb.velocity.y);

		// Update horizontal movement (but lock the horizontal movement while in air)
		if (isGrounded) {
			rb.velocity = new Vector3(horizontal, rb.velocity.y);
		}

		// If on the ground and jump is pressed...
		if (jump && isGrounded) {

			if (backJump) {
				//do BackJump()...
				BackJump();
			} else {
				//do Jump()...
				Jump();
			}
		}
    }

	void Jump() {
		//The addforce that makes the Player jump.
		//rb.AddForce ((jump_force + m_JumpPower * model.localScale) * m_JumpPower, ForceMode.Impulse); NEEDS A LITTLE MORE WORK
		rb.AddForce(Vector3.up * m_JumpPower, ForceMode.Impulse);

		//Jump Done
		jump = false;
	}

	void BackJump() {
		//The addforce that makes the Player jump slightly higher.
		//rb.AddForce ((backJump_force + m_JumpPower * model.localScale) * m_JumpPower, ForceMode.Impulse); NEEDS A LITTLE MORE WORK
		rb.AddForce(Vector3.up * 1.5f * m_JumpPower, ForceMode.Impulse);

		//Jump Done
		backJump = false;
		jump = false;
	}
}
