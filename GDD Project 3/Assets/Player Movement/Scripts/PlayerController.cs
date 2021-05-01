using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class PlayerController : MonoBehaviour
{

    #region Inspector Variables
    [SerializeField]
    [Tooltip("Order in which the player uses the characters in the level; 0 for first, 1 for second")]
    protected int order;
    #endregion

    #region Cached Values

    protected LevelManager lm;
    protected PlayerMovement cr_pm;
    protected Collider2D cr_col;
    protected Collider2D cr_feet_col;
    protected Collider2D cr_left_col;
    protected Collider2D cr_right_col;
    protected Vector3 initial_position;

    #endregion

    #region Initialization Methods

    IEnumerator Start()
    {
        lm = LevelManager.instance;
        lm.playerOrder[order] = gameObject;
        cr_pm = GetComponent<PlayerMovement>();
        cr_col = GetComponent<Collider2D>();
        cr_feet_col = GetComponentInChildren<FeetController>().GetComponent<BoxCollider2D>();
        initial_position = transform.position;
        if (cr_pm.canWallJump)
        {
            cr_left_col = GetComponentInChildren<LeftColliderScript>().GetComponent<BoxCollider2D>();
            cr_right_col = GetComponentInChildren<RightColliderScript>().GetComponent<BoxCollider2D>();
            print("set left/right colliders");

            /*if (cr_left_col)
            {
                print("found left");
            }
            if (cr_right_col)
            {
                print("found right");
            }*/
        }
        SetStartState();

        yield return null;

        lm.MovePlayerToCheckpoint();
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
        ResetEQ();
        ColorManager c = ColorManager.instance;
        c.SwitchColor();
    }

    private void ResetEQ()
    {
        AudioManager am = AudioManager.instance;
        if (am)
        {
            am.muffle(21000f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    #endregion

}