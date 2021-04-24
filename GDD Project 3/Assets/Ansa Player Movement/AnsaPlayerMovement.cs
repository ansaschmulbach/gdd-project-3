using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CollisionHandler))]
public class AnsaPlayerMovement : PlayerMovement
{


    #region Inspector Variables

    [Header("Jump variables")]
    [SerializeField] private float timeInAir;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float xJumpDist;
    
    [Header("Land variables")]
    [SerializeField] private float x0Land; //x(0) velocity on land
    [SerializeField] private float xAccLand;

    #endregion

    #region Calculated Variables

    private float xJumpVel;
    private float yJumpV0;
    private float gravity;

    #endregion
    
    #region Private Variables

    [Header("For Debugging- do not edit")]
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 acceleration;
    private bool isJumpingLast; //used to detect a switch in jumping
    [SerializeField] private float xPressedLast;
    [SerializeField] private bool firstXPress = true;
    private CollisionHandler colHandler;
    
    #endregion

    #region Public Variables

    [Header("Public Variables - Do not Edit")]
    public bool isJumping;
    public bool isTouchingGround;

    #endregion
    
    void Start()
    {
        xJumpVel = XVel();
        yJumpV0 = JumpY0Vel();
        gravity = Gravity();
        this.velocity = Vector3.zero;
        this.acceleration = Vector3.up * gravity;
        isJumping = false;
        isJumpingLast = false;
        xPressedLast = 0f;
        colHandler = GetComponent<CollisionHandler>();
        Debug.Log("");
    }

    void Update()
    {
        
        xJumpVel = XVel();
        yJumpV0 = JumpY0Vel();
        gravity = Gravity();
        
        if (Input.GetKeyDown("space") && isTouchingGround)
        {
            this.velocity.y = yJumpV0;
            this.velocity.x = 0;
            isJumping = true;
            isTouchingGround = false;
        }
        
        float xInput = Input.GetAxisRaw("Horizontal");
        firstXPress = (Math.Sign(xPressedLast) != Math.Sign(xInput)) && (xInput != 0);
        float xMult = 0;
        float xVelAdd = isJumping ? xJumpVel : x0Land;
        if (firstXPress || (isJumping ^ isJumpingLast))
        {
            xMult = 1;
            velocity.x = xVelAdd * xMult * xInput;
        }
        velocity.x *= Math.Abs(xInput);
        velocity.z = 0;

        // Debug.Log("VelAdd, XMult, Xinput: " + xVelAdd + ", " + xMult + ", " + xInput);
        // Debug.Log("VelAccel: " +  velocity + ", " + acceleration);
        //
        //
        
        
        float xAccel = isJumping ? 0 : xAccLand;
        acceleration.x = xAccel * xInput;
        acceleration.y = gravity;
        acceleration.z = 0;
        // Debug.Log("VelAccel: " +  velocity + ", " + acceleration);
        
        UpdatePositionVelocity();
        
        //Update Variables
        xPressedLast = xInput;
        isJumpingLast = isJumping;

    }

    #region Position Update methods

    void UpdatePositionVelocity()
    {
        Vector3 newPosUpdate = velocity * Time.deltaTime + (0.5f * Time.deltaTime * Time.deltaTime) * acceleration;
        Vector3 platformVel = Vector3.zero;
        Collider2D groundCollider = colHandler.UpdateCollisionVelocity(ref newPosUpdate, ref platformVel);
        if (isTouchingGround)
        {
            float yDist = groundCollider.bounds.size.y/2 + GetComponent<Collider2D>().bounds.size.y/2;
            float xPos = this.transform.position.x;
            float yPos = groundCollider.transform.position.y + yDist;
            float zPos = this.transform.position.z;
            this.transform.position = new Vector3(xPos, yPos, zPos) + newPosUpdate;
        }
        else
        {
            transform.Translate(newPosUpdate);
        }
        this.velocity += acceleration * Time.deltaTime;
        if (isTouchingGround)
        {
            Debug.Log(platformVel.y);
            velocity.y = platformVel.y;
        }
        
    }

    #endregion

    #region Calculating Variables Methods

    float XVel()
    {
        return xJumpDist / timeInAir;
    }

    float JumpY0Vel()
    {
        return (2 * jumpHeight)/timeInAir;
    }

    float Gravity()
    {
        return (-2 * jumpHeight) / (timeInAir * timeInAir);
    }

    #endregion

    #region Collision methods

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            isTouchingGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isTouchingGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isTouchingGround = false;
        }
    }
    

    #endregion

}
