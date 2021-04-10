using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftColliderScript : MonoBehaviour
{
	PlayerMovement pm;

	void Start()
    {
		pm = GetComponentInParent<PlayerMovement>();
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		pm.leftContact = true;
	}

	void OnCollisionExit2D(Collision2D c)
	{
		pm.leftContact = false;
	}
}
