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
		pm.canPropulse = true;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		pm.canPropulse = false;
	}
}
