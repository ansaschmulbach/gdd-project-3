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
        Debug.Log(" ");
    }
    
    public void UpdateCollisionVelocity(ref Vector3 velocity)
    {
        UpdateRaycastOrigins();
        VerticalCollisions(ref velocity);
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        float yPos = (direction == -1) ? cornerPos.bottomRight.y: cornerPos.topLeft.y;
        Vector3 platformVelocity = Vector3.zero;
        playerMovement.isTouchingGround = false;
        for (float i = cornerPos.topLeft.x; i <= cornerPos.bottomRight.x + 0.0001f; i += vertRaySpacing)
        {
            Vector2 rayOrigin = new Vector3(i, yPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, rayLength, collisionMask);
            if (hit)
            {
                if (hit.distance <= skinWidth + 0.0001f && direction == -1)
                {
                    Debug.Log(hit.collider.gameObject);
                    playerMovement.isTouchingGround = true;
                    playerMovement.isJumping = false;
                    IPlatform platform = hit.collider.gameObject.GetComponent<IPlatform>();
                    platformVelocity = platform.velocity();
                }
                velocity.y = (hit.distance - skinWidth) * direction;
                rayLength = hit.distance;
            }

            velocity += platformVelocity;
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
