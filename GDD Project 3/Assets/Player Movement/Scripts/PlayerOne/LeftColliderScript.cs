using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftColliderScript : MonoBehaviour
{
	/*
	// Returns whether the obj is a wall
	bool isWall(GameObject obj)
	{
		return obj.layer == LayerMask.NameToLayer("Floor");
	}*/

	void OnCollisionEnter2D(Collision2D c)
	{
		GetComponentInParent<PlayerMovement>().leftContact = true;
	}

	void OnCollisionExit2D(Collision2D c)
	{
		GetComponentInParent<PlayerMovement>().leftContact = false;
	}
}
