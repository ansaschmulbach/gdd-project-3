using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneController : PlayerController
{
	protected override void SetStartState()
	{
		if (order == lm.lastPlayerIndex)
        {
			SetEnabled();
		} else
        {
			//SetDisabled();
			DisableMovement();
        }
	}
	
}
