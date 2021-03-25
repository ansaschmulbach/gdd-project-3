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

	[SerializeField] [Tooltip("Can you wall jump?")]
	public bool canWallJump = false;

	[SerializeField]
	[Tooltip("Jump speed")]
	public float jumpSpeed;

	[SerializeField]
	[Tooltip("Can move")]
	public bool isActive = true;

	#endregion

	#region Jumping variables

	private float jumpTimer;

	[SerializeField]
	[Tooltip("Max jump rate in seconds")]
	private float maxJumpRate = 0.6f;
	
	/**If you're touching a wall, you can't propulse*/
	private bool touchingWall;

	private Vector2 jumpVector;
	private Vector2 pushVector;

	private Vector2 vel;

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
		set { left = value; }
	}

	/* Whether the player is touching a wall on their right. */
	private bool right;
	public bool rightContact
	{
		get { return right; }
		set { right = value; }
	}

	#endregion

	#region Rigidbody stuff

	private Rigidbody2D rb;

	#endregion

	#region Unity methods

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		pc = GetComponent<PlayerController>();
		jumpTimer = 0;
		jumpVector = new Vector2(0, jumpSpeed / 36);
		pushVector = new Vector2(jumpSpeed / 32, 0);
		vel = Vector2.zero;
		left = false;
		right = false;
	}

	// Update is called once per frame
	void Update()
    {
		//rb.velocity *= 0.99f;

		if (!isActive)
		{
			return;
		}

		if (m_canPropulse || touchingFloor)
		{

			float xDir = Input.GetAxisRaw("Horizontal");
			// rb.AddForce(Vector2.right * (xDir * movementSpeed * Time.deltaTime), ForceMode2D.Force);
			xDir *= 0.2f;
			/*if (!touchingFloor)
            {
				xDir = xDir * 0.75f;
				
            } */
			vel = rb.velocity;

			if (left || right)
			{
				if (left && right && touchingFloor)
                {
					//pc.Die();
                }
				xDir *= 0.2f;
				vel.y = Mathf.Max(-0.4f, vel.y);
			}
			vel.x = xDir + Mathf.Lerp(vel.x, xDir * movementSpeed, 0.02f);
			rb.velocity = vel;
			
		}

		if (touchingFloor || left || right) {
			if (Input.GetKey(KeyCode.Space))
			{
				Jump();
			}
		}

		jumpTimer = Mathf.Max(0f, jumpTimer - Time.deltaTime);

	}

	#endregion

	#region Jump method

	private void Jump()
	{
		if (jumpTimer == 0)
		{
			jumpTimer = maxJumpRate;


			if (touchingFloor)
			{
				// rb.AddForce(Vector2.up * jumpSpeed);
				rb.velocity += jumpVector;
			}
			else if (left)
            {
				// rb.AddForce(new Vector2(jumpSpeed, jumpSpeed));
				rb.velocity += jumpVector + pushVector;

			}
			else if (right)
            {
	            // rb.AddForce(new Vector2(-jumpSpeed, jumpSpeed));
				rb.velocity += jumpVector - pushVector;
			}
		}
	}

	#endregion

	#region Collision Methods

	/*private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag("Wall"))
		{
			this.touchingWall = true;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.collider.CompareTag("Wall"))
		{
			this.touchingWall = false;
		}
	}*/

	#endregion
}
