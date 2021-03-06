﻿using System;
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
	
	#endregion

	#region Propulsion variables

	private bool m_canPropulse = true;
	public bool canPropulse
	{
		get { return m_canPropulse; }
		set { m_canPropulse = value; }
	}

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
		jumpTimer = 0;
		left = false;
		right = false;
	}

	// Update is called once per frame
	void Update()
    {

		if (!isActive)
		{
			return;
		}

		if (m_canPropulse && !touchingWall || touchingFloor)
		{

			float xDir = Input.GetAxisRaw("Horizontal");
			rb.AddForce(Vector2.right * (xDir * movementSpeed * Time.deltaTime), ForceMode2D.Force);
			
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
				rb.AddForce(Vector2.up * jumpSpeed);
			}
			else if (left)
            {
	            rb.AddForce(new Vector2(jumpSpeed, jumpSpeed));
			} else if (right)
            {
	            rb.AddForce(new Vector2(-jumpSpeed, jumpSpeed));
            }
		}
	}

	#endregion

	#region Collision Methods

	private void OnCollisionEnter2D(Collision2D other)
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
	}

	#endregion
}
