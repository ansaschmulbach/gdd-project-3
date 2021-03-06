using System;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{

    #region Cached Values

    private PlayerMovement cr_pm;
    private Collider2D cr_col;
    private Collider2D cr_feet_col;

    #endregion

    #region Initialization Methods

    void Start()
    {
        cr_pm = GetComponent<PlayerMovement>();
        cr_col = GetComponent<Collider2D>();
        cr_feet_col = GetComponentInChildren<FeetController>().GetComponent<BoxCollider2D>();
        SetStartState();
    }

    protected abstract void SetStartState();

    #endregion
    
    #region Enable/Disable Methods

    public void SetEnabled()
    {
        cr_pm.isActive = true;
        this.GetComponent<Rigidbody2D>().isKinematic = false;
        cr_col.enabled = true;
        cr_feet_col.enabled = true;
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
    }

    public void DisableMovement()
    {
        cr_pm.isActive = false;
    }
    
    #endregion


}