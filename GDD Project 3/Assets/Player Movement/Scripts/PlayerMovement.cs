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
	public bool movementRequiresPropulsion;

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

	#endregion

	#region Propulsion variables

	private bool m_canPropulse = false;

	public bool canPropulse
	{
		set { m_canPropulse = value; }
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
	}

    // Update is called once per frame
    void Update()
    {

		if (!isActive)
		{
			return;
		}

		if (m_canPropulse || !movementRequiresPropulsion)
		{
			if (Input.GetAxisRaw("Horizontal") == 1)
			{
				rb.AddForce(Vector2.right * movementSpeed * Time.deltaTime, ForceMode2D.Force);
			}

			if (Input.GetAxisRaw("Horizontal") == -1)
			{
				rb.AddForce(Vector2.left * movementSpeed * Time.deltaTime, ForceMode2D.Force);
			}
		}

		if (m_canPropulse) {
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
			rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Force);
		}
	}

	#endregion
}
