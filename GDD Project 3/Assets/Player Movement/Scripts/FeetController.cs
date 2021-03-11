using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetController : MonoBehaviour
{

	PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
		pm = GetComponentInParent<PlayerMovement>();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Floor"))
		{
			//pm.canPropulse = true;
			pm.touchingFloor = true;	
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Floor"))
		{
			//pm.canPropulse = false;
			pm.touchingFloor = false;
		}
	}
}
