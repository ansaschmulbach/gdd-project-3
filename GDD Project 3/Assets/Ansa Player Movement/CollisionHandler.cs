using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(BoxCollider2D))]
public class CollisionHandler : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] private float skinWidth = 0.015f;
    [SerializeField] private int horizontalRayCount = 2;
    [SerializeField] private int verticalRayCount = 2;
    [SerializeField] private LayerMask collisionMask;
    
    #endregion

    #region Private variables

    private float horizRaySpacing;
    private float vertRaySpacing;
    private BoxCollider2D boxCollider;
    private CornerPositions cornerPos;
    private AnsaPlayerMovement playerMovement;

    #endregion
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        horizRaySpacing = HorizontalRaysSpacing();
        vertRaySpacing = VerticalRaysSpacing();
        playerMovement = GetComponent<AnsaPlayerMovement>();
        Debug.Log("");
    }
    
    public Collider2D UpdateCollisionVelocity(ref Vector3 velocity, ref Vector3 platformVel)
    {
        UpdateRaycastOrigins();
        //HorizontalCollisions(ref velocity);
        return VerticalCollisions(ref velocity, ref platformVel);
    }

    Collider2D VerticalCollisions(ref Vector3 velocity, ref Vector3 platformVel)
    {
        float direction = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        float yPos = (direction == -1) ? cornerPos.bottomRight.y: cornerPos.topLeft.y;
        playerMovement.isTouchingGround = false;
        Vector3 platformVelocity = Vector3.zero;
        Collider2D groundCollider = null;

        for (float i = cornerPos.topLeft.x; i <= cornerPos.bottomRight.x + 0.0001f; i += vertRaySpacing)
        {
            Vector2 rayOrigin = new Vector3(i, yPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, rayLength, collisionMask);
            if (hit)
            {
                IPlatform platform = hit.collider.GetComponent<IPlatform>();
                platformVelocity = platform.velocity();
                if (hit.distance <= skinWidth + 0.0001f + Math.Abs(platformVelocity.y) && direction == -1)
                {
                    playerMovement.isTouchingGround = true;
                    playerMovement.isJumping = false;
                    platformVel = platformVelocity;
                    groundCollider = hit.collider;
                }
                velocity.y = (hit.distance - skinWidth) * direction;
                rayLength = hit.distance;
            }

            velocity += platformVel;
            Debug.DrawRay(rayOrigin, Vector3.up * (direction * rayLength), Color.red);
            // Debug.Log(platformVel + " " + hit.distance);  
        }   
        
        return groundCollider;
    }
    
    void HorizontalCollisions(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        float xPos = (direction == -1) ? cornerPos.topLeft.x: cornerPos.bottomRight.x;

        for (float i = cornerPos.bottomRight.y; i <= cornerPos.topLeft.y + 0.0001f; i += horizRaySpacing)
        {
            Vector2 rayOrigin = new Vector3(xPos, i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayLength, collisionMask);
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * direction;
                rayLength = hit.distance;
            }
            Debug.DrawRay(rayOrigin, Vector3.up * (direction * rayLength), Color.red);
        }   
        
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        cornerPos.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        cornerPos.topLeft = new Vector2(bounds.min.x, bounds.max.y);
    }


    #region Spacing Methods

    float HorizontalRaysSpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        return bounds.size.y / (horizontalRayCount - 1);
    }
    
    float VerticalRaysSpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        return bounds.size.x / (verticalRayCount - 1);
    }

    #endregion
    struct CornerPositions
    {
        public Vector2 topLeft, bottomRight;
    }
    
}
