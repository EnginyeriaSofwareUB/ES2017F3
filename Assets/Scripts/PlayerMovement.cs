using System.Collections;
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
    private Animator anim;

	public Vector3 jump_force;
	public Vector3 backJump_force;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
        anim = GetComponentInChildren<Animator>();
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

    void OnCollisionEnter(Collision collision) {
        if (jump == true)
        {
            jump = false;
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
        }

        if (horizontal != 0) {
            // Perform walking animation
            anim.SetBool("walking", true);
        }
        else {
            // Return to Idle
            anim.SetBool("walking", false);
        }
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
			}
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
		if (isGrounded && !jump) {
			rb.velocity = new Vector3(horizontal, rb.velocity.y);
		}


			if (backJump) {
				//do BackJump()...
				BackJump();
			}
	}


    public void Idle() {
        if (anim == null) return;

        // Return to Idle
        anim.SetBool("walking", false);
    }
   
    void BackJump() {
		//The addforce that makes the Player jump slightly higher.
		//rb.AddForce ((backJump_force + m_JumpPower * model.localScale) * m_JumpPower, ForceMode.Impulse); NEEDS A LITTLE MORE WORK
		rb.AddForce(Vector3.up * 1.5f * m_JumpPower, ForceMode.Impulse);

		//Jump Done
		backJump = false;
		jump = false;
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
    }

}
