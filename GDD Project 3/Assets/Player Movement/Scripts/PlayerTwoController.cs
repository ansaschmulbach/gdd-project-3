using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoController : MonoBehaviour
{
	PlayerMovement pm;

	// Start is called before the first frame update
	void Start()
    {
		pm = GetComponent<PlayerMovement>();
		pm.isActive = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<PlayerMovement>().isActive = false;
			collision.collider.enabled = false;
			Destroy(collision.collider.GetComponent<Rigidbody2D>());
			pm.isActive = true;
		}
	}
}
