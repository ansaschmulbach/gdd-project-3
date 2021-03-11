using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightColliderScript : MonoBehaviour
{
	// Returns whether the obj is a wall
	bool isWall(GameObject obj)
	{
		return obj.layer == LayerMask.NameToLayer("Wall");
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		GetComponentInParent<PlayerMovement>().rightContact = true;
	}

	void OnCollisionExit2D(Collision2D c)
	{
		GetComponentInParent<PlayerMovement>().rightContact = false;
	}
}
