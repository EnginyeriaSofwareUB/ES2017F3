using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;

    public float horizontal;
	public Transform model;
	public bool jump;
	public bool isGrounded;
	public bool backJump = false;
    private const float m_JumpPower = 5f; // The force added to the ball when it jumps.
	private const float m_BackJumpPower = 6f;
	private bool facingRight = false;
    private Animator anim;

	public Vector3 jump_force;
	public Vector3 backJump_force;

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

    void OnCollisionEnter(Collision collision) {
        if (jump == true)
        {
            jump = false;
			backJump = false;
            //print("Player Landed");
        }
    }

	// Update is called once per frame
	void Update () {

        // Check movement Inputs
		horizontal = Input.GetAxis ("Horizontal") * 5f;

        // If jump pressed and in ground and not jumping
	    if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !anim.GetBool("jump")) {
            // Perform jumping animation
            anim.SetBool("jump", true);
			jump = true;
		}

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

		// Udate horizontal movement
		//rb.velocity = new Vector3(horizontal, rb.velocity.y);

		// Update horizontal movement (but lock the horizontal movement while in air)
		if ((isGrounded && !jump) || !backJump) {
			rb.velocity = new Vector3(horizontal, rb.velocity.y);
		}

		if (Input.GetKeyDown (KeyCode.Space) && backJump) { //!anim.GetBool("backjump"))
			//anim.SetBool("backjump", true);
			//do BackJump()...
			BackJump ();
		}
	}


    public void Idle() {
        if (anim == null) return;

        // Return to Idle
        anim.SetBool("walking", false);
    }
   
    void BackJump() {
		//The addforce that makes the Player jump slightly higher.
		rb.AddForce(Vector3.up * m_BackJumpPower, ForceMode.Impulse);

		//Jump Done
		backJump = false;
		jump = true;

        GetComponent<PlayerSounds>().BackJump();
    }
    
    public void Jump() {
        if (anim == null) return;

        anim.SetBool("jump", false);

        Vector3 v;
        if (model.localScale.z > 0) v = Vector3.right;
        else v = Vector3.left;

        // ... add force in upwards.
        rb.AddForce(( (v + Vector3.up)/Mathf.Sqrt(2) )* m_JumpPower, ForceMode.Impulse);

        jump = true;
		backJump = true;

        GetComponent<PlayerSounds>().NormalJump();
    }

}
