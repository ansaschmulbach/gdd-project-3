using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnsaPlayerMovement : PlayerMovement
{

    [Header("Jump variables")]
    [SerializeField] private float timeInAir;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float xJumpDist;
    
    [Header("Land variables")]
    [SerializeField] private float x0Land; //x(0) velocity on land
    [SerializeField] private float xAccLand;
    
    
    #region Calculated Variables

    private float xJumpVel;
    private float yJumpV0;
    private float gravity;

    #endregion
    
    #region Private Variables

    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private bool isJumping;
    private bool isTouchingGround;
    private bool isJumpingLast; //used to detect a switch in jumping
    [SerializeField] private bool firstXPress = true;

    #endregion

    void Start()
    {
        xJumpVel = XVel();
        yJumpV0 = JumpY0Vel();
        gravity = Gravity();
        this.velocity = Vector3.zero;
        this.acceleration = Vector3.up * gravity;
        isJumping = false;
    }

    void Update()
    {
        
        xJumpVel = XVel();
        yJumpV0 = JumpY0Vel();
        gravity = Gravity();
        
        if (Input.GetKeyDown("space"))
        {
            this.velocity.y = yJumpV0;
            this.velocity.x = 0;
            isJumping = true;
            isTouchingGround = false;
        }
        
        float xInput = Input.GetAxis("Horizontal");
        float xMult = firstXPress ? 1 : 0;
        float xVelAdd = isJumping ? xJumpVel : x0Land;
        firstXPress = !firstXPress && (xInput != 0);
        velocity.x += xVelAdd * xMult;
        velocity.x *= xInput;
        velocity.z = 0;


        float xAccel = isJumping ? 0 : xAccLand;
        acceleration.x = xAccel;
        acceleration.y = gravity;
        acceleration.z = 0;
        Debug.Log("VelAccel: " +  velocity + ", " + acceleration);
        
        UpdatePositionVelocity();
    }

    #region Position Update methods

    void UpdatePositionVelocity()
    {
        this.transform.position += velocity * Time.deltaTime + (0.5f * Time.deltaTime * Time.deltaTime) * acceleration;
        this.velocity += acceleration * Time.deltaTime;
        if (isTouchingGround)
        {
            this.velocity.y = 0;
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
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isTouchingGround = false;
        }
    }
    

    #endregion

}
