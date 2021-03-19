using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            Debug.Log(other.gameObject);
            if (pc.enabled)
            {
                pc.Die();     
            }
        }
        else if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            PlayerController pc = other.transform.parent.GetComponent<PlayerController>();
            if (pc.enabled)
            {
                pc.Die();
            }
        }
    }
}
