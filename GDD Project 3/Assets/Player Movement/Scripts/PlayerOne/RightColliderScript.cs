﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightColliderScript : MonoBehaviour
{
	PlayerMovement pm;

	void Start()
	{
		pm = GetComponentInParent<PlayerMovement>();
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		pm.rightContact = true;
	}

	void OnCollisionExit2D(Collision2D c)
	{
		pm.rightContact = false;
	}
}
