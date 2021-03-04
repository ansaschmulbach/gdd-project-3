using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneController : MonoBehaviour
{
	PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
		pm = GetComponent<PlayerMovement>();
		pm.isActive = true;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
