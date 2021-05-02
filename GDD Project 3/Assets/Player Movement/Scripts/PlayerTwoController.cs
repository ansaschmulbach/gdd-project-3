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
		
		Debug.Log(collision.gameObject.tag);
		
		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("LeftRight"))
		{
			PlayerController pc = collision.collider.GetComponentInParent<PlayerController>();
			this.SetEnabled();
			pc.SetDisabled();
			GameObject transitioner = GameObject.FindWithTag("LevelTransitioner");
			transitioner.GetComponent<LevelTransitioner>().EnableTransition();
		}

	}
	
}
