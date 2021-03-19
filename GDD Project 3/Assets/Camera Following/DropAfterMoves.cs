using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAfterMoves : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] [Tooltip("The number of contacts allowed before this platform drops")]
    private int m_NumContacts = 1;

    [SerializeField] [Tooltip("The amount of time to wait before dropping")]
    private float m_DropDelay = 0.2f;

    [SerializeField] [Tooltip("Amount of time between when collisions are counted")]
    private float m_ColTimer = 0.03f;
    
    #endregion
    
    #region Cached Variables

    private Rigidbody2D p_RB;

    #endregion

    #region private variables

    private bool p_dropping;
    private float p_ColTimer = 0;

    #endregion
    
    #region  Initialization

    void Start()
    {
        p_RB = this.GetComponent<Rigidbody2D>();
        p_RB.constraints = RigidbodyConstraints2D.FreezeAll;
        p_dropping = false;
        p_ColTimer = m_ColTimer;
    }

    #endregion

    #region Updates

    private void Update()
    {
        p_ColTimer -= Time.deltaTime;
    }

    #endregion
    
    #region Collision Methods

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (p_dropping)
        {
            return;
        }

        if (p_ColTimer > 0)
        {
            return;
        }
        
        PlayerMovement pm_child = other.collider.GetComponent<PlayerMovement>();
        PlayerMovement pm_parent = other.collider.GetComponentInParent<PlayerMovement>();
        PlayerMovement pm = null;
        if (pm_child != null)
        {
            pm = pm_child;
        } 
        else if (pm_parent != null)
        {
            pm = pm_parent;
        }
        else
        {
            return;
        }
        
        if (pm.isActive)
        {
            Debug.Log(other.gameObject);
            this.m_NumContacts -= 1;
            if (this.m_NumContacts == 0)
            {
                StartCoroutine(Drop());
                p_dropping = true;
            }
            
        }

        p_ColTimer = m_ColTimer;

    }

    #endregion

    #region Drop
    
    
    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(m_DropDelay);
        //GetComponent<Collider2D>().enabled = false;
        this.p_RB.constraints = RigidbodyConstraints2D.None;  
        this.p_RB.velocity = Vector2.down;
    }

    #endregion

}
