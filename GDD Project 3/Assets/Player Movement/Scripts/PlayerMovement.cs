using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region aspdasd
	Vector3 scale;
	private SpriteRenderer sr;
	#endregion

	#region animator
	public Animator animator;
	#endregion

	#region Movement variables

	[SerializeField]
	[Tooltip("Movement speed")]
	public float movementSpeed;

	[SerializeField]
	[Tooltip("Can you move mid-air?")]
	public bool touchingFloor;

	[SerializeField]
	[Tooltip("Can you wall jump?")]
	public bool canWallJump;

	[SerializeField]
	[Tooltip("Jump speed")]
	public float jumpSpeed;

	[SerializeField]
	[Tooltip("Can move")]
	public bool isActive = true;

	[SerializeField]
	[Tooltip("Can Double Jump?")]
	private bool canDoubleJump;

	#endregion

	#region Jumping variables

	private float jumpTimer;

	[SerializeField]
	[Tooltip("Max jump rate in seconds")]
	private float maxJumpRate = 0.3f;

	/**If you're touching a wall, you can't propulse*/
	private bool touchingWall;

	private Vector2 jumpVector;
	private Vector2 pushVector;

	private Vector2 vel;

	private bool hasDoubleJumped;

	#endregion

	#region Propulsion variables

	private bool m_canPropulse = true;
	public bool canPropulse
	{
		get { return m_canPropulse; }
		set { m_canPropulse = value; }
	}

	/* Player controller. */
	private PlayerController pc;

	/* Whether the player is touching a wall on their left. */
	private bool left;
	public bool leftContact
	{
		get { return left; }
		set {
			left = value;
		}
	}

	/* Whether the player is touching a wall on their right. */
	private bool right;
	public bool rightContact
	{
		get { return right; }
		set {
			right = value;
		}
	}

	#endregion

	#region Player Components

	private Rigidbody2D rb;

	private AudioSource asrc;

	#endregion

	#region Unity methods

	void Start()
    {
		scale = transform.localScale;
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		rb.interpolation = RigidbodyInterpolation2D.Interpolate;
		pc = GetComponent<PlayerController>();
		asrc = GetComponent<AudioSource>();
		asrc.playOnAwake = false;
		jumpTimer = 0;
		jumpVector = new Vector2(0, jumpSpeed * rb.gravityScale);
		pushVector = new Vector2(movementSpeed / 5, 0);
		vel = Vector2.zero;
		left = false;
		right = false;
		hasDoubleJumped = false;
	}

	// Update is called once per frame
	void FixedUpdate()
    {

		if (!isActive)
		{
			rb.gravityScale = 0;
		}
		else
		{
			rb.gravityScale = 6.5f;
		}
	}

	void Update()
    {
		if (!isActive)
        {
			return;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			Jump();
		}
		if (m_canPropulse || touchingFloor)
		{

			float xDir = 0.2f * Input.GetAxisRaw("Horizontal");
			if (xDir > 0) {
				this.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
				//sr.flipX = true;
			} else if (xDir < 0) {
				this.transform.localScale = scale;
				//sr.flipX = false;
			}
			//if facing right, flip (sr flip x = true)
			// rb.AddForce(Vector2.right * (xDir * movementSpeed * Time.deltaTime), ForceMode2D.Force);
			if (vel.x > 10 && xDir < 0 || vel.x < -10 && xDir > 0)
			{
				xDir *= 2;
			}
			/*if (!touchingFloor)
            {
				xDir = xDir * 0.75f;
				
            } */
			vel = rb.velocity;
			//Sliding on wall
			if ((xDir < 0 && left) || (xDir > 0 && right))
			{
				if (left && right && touchingFloor)
                {
					Debug.Log("asd");
					pc.Die();
                }
				xDir *= 0.75f;
				vel.y = Mathf.Max(-0.5f, vel.y);
			}
			vel.x = xDir + Mathf.Lerp(vel.x, xDir * movementSpeed * 1.5f, 0.0275f);
			rb.velocity = vel;
			
		}

		if (touchingFloor || canDoubleJump|| left || right) {
			if (Input.GetKey(KeyCode.Space))
			{
				Jump();
			}
		}
		animator.SetBool("WallJump", false);

		jumpTimer = jumpTimer - Time.deltaTime;

	}

	#endregion

	#region Jump method

	private void Jump()
	{
		Debug.Log("u jumped lol");
		if (jumpTimer < 0)
		{
			jumpTimer = maxJumpRate;

			//regular jump
			if (touchingFloor)
			{
				animator.SetTrigger("Jump");
				rb.velocity += jumpVector;
				asrc.Play();
			}
			//walljumps
			else if (left || right)
            {
				animator.SetBool("WallJump", true);
				rb.velocity += jumpVector + pushVector;
				asrc.Play();
			}
			//walljump
			else if (right)
            {
				animator.SetBool("WallJump", true);
				rb.velocity += jumpVector - pushVector;
				asrc.Play();
			}
			//double jump, only for bass clef
			else if (!hasDoubleJumped && canDoubleJump)
			{
				animator.SetTrigger("Jump");
				// Debug.Log("DOUBLE JUMP");
				Vector2 vel = rb.velocity;
				vel.y = Mathf.Max(rb.velocity.y, -0.1f);
				rb.velocity = vel + jumpVector;
				//transform.localPosition += new Vector3(0, 1f, 0);
				hasDoubleJumped = true;
				asrc.Play();
			}
		}
	}

	#endregion

	#region Collision Methods

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag("Floor"))
		{
			hasDoubleJumped = false;
		}
	}

	#endregion
}
