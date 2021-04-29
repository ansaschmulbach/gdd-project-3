using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

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
	private float maxJumpRate = 0.6f;

	private bool doubleJumpThrottle;

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
		rb = GetComponent<Rigidbody2D>();
		rb.interpolation = RigidbodyInterpolation2D.Interpolate;
		pc = GetComponent<PlayerController>();
		//asrc = GetComponent<AudioSource>();
		//asrc.playOnAwake = false;
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
		} else
		{
			doubleJumpThrottle = false;
		}

		if (m_canPropulse || touchingFloor)
		{

			float xDir = 0.2f * Input.GetAxisRaw("Horizontal");
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

			if ((xDir < 0 && left) || (xDir > 0 && right))
			{
				if (left && right && touchingFloor)
                {
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

		jumpTimer = jumpTimer - Time.deltaTime;

	}

	#endregion

	#region Jump method

	private void Jump()
	{
		if (jumpTimer < 0)
		{
			jumpTimer = maxJumpRate;


			if (touchingFloor)
			{
				rb.velocity += jumpVector;
				asrc.Play();
			}
			else if (left)
            {
				rb.velocity += jumpVector + pushVector;
				asrc.Play();
			}
			else if (right)
            {
				rb.velocity += jumpVector - pushVector;
				asrc.Play();

			}
			else if (!hasDoubleJumped && canDoubleJump)
			{
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
