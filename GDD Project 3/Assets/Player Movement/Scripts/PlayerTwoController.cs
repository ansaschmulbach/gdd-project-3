using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoController : PlayerController
{

	protected override void SetStartState()
	{
		DisableMovement();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (collision.collider.TryGetComponent(out PlayerController pc))
			{
				pc.SetDisabled();
				this.SetEnabled();
			}
		}
	}
	
}
