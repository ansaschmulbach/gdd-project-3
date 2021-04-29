using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoController : PlayerController
{

	protected override void SetStartState()
	{
		if (order == lm.lastPlayerIndex)
		{
			SetEnabled();
		}
		else
		{
			DisableMovement();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
		Debug.Log(collision.gameObject.tag);
		
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log(collision.gameObject);
			if (collision.gameObject.TryGetComponent(out PlayerController pc))
			{
				Debug.Log("setting disabled");
				this.SetEnabled();
				pc.SetDisabled();
				lm.UpdateLastPlayer(gameObject);
			}
		} 
		else if (collision.gameObject.CompareTag("LeftRight"))
		{
			PlayerController pc = collision.collider.GetComponentInParent<PlayerController>();
			this.SetEnabled();
			pc.SetDisabled();
			lm.UpdateLastPlayer(gameObject);
		}

	}
	
}
