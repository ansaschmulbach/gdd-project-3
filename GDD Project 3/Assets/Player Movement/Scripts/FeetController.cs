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
	

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
			//pm.canPropulse = true;
			pm.touchingFloor = true;
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
			//pm.canPropulse = true;
			pm.touchingFloor = false;
		}

	}
	
}
