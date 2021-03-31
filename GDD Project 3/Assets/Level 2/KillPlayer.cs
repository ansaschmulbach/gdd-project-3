using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject);
        Debug.Log(other.gameObject.tag);
        Debug.Log(other.gameObject.transform.parent);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hello hello");
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc.enabled)
            {
                pc.Die();     
            }
        }
    }
}
