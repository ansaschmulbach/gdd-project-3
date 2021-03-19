using System;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{

    #region Inspector Variables

    #endregion
    
    #region Cached Values

    private PlayerMovement cr_pm;
    private Collider2D cr_col;
    private Collider2D cr_feet_col;
    private Collider2D cr_left_col;
    private Collider2D cr_right_col;

    #endregion

    #region Initialization Methods

    void Start()
    {
        cr_pm = GetComponent<PlayerMovement>();
        cr_col = GetComponent<Collider2D>();
        cr_feet_col = GetComponentInChildren<FeetController>().GetComponent<BoxCollider2D>();
        if (cr_pm.canWallJump)
        {
            cr_left_col = GetComponentInChildren<LeftColliderScript>().GetComponent<BoxCollider2D>();
            cr_right_col = GetComponentInChildren<RightColliderScript>().GetComponent<BoxCollider2D>();
        }
        SetStartState();
    }

    protected abstract void SetStartState();

    #endregion
    
    #region Enable/Disable Methods

    public void SetEnabled()
    {
        Debug.Log("hii");
        cr_pm.isActive = true;
        this.GetComponent<Rigidbody2D>().isKinematic = false;
        cr_col.enabled = true;
        cr_feet_col.enabled = true;
        if (cr_pm.canWallJump)
        {
            cr_left_col.enabled = true;
            cr_right_col.enabled = true;   
        }
    }
    
    public void SetDisabled()
    {
        cr_pm.isActive = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        cr_col.enabled = false;
        cr_feet_col.enabled = false;
        if (cr_pm.canWallJump)
        {
            cr_left_col.enabled = false;
            cr_right_col.enabled = false;
        }
    }

    public void DisableMovement()
    {
        cr_pm.isActive = false;
    }
    
    #endregion

    #region Health/Death Methods

    public void Die()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i]);
        }
        Debug.Log("Game Over");
    }
    
    #endregion

}