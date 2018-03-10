using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class PlatformerController : MonoBehaviour {

    public bool ignoreOneWayCollision=false;

    public float maxClimbAngle = 80;

    const float margin = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public LayerMask GroundCollision;
    BoxCollider2D collider2d;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisionInfo;

	// Use this for initialization
	void Start () {
        collider2d = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
	}

    public void Move(Vector3 velocity)
    {
        //Debug.Log("Moving");
        UpdateRaycastOrigins();
        collisionInfo.ResetBools();
        if(velocity.x!=0) HorizontalCollisionDetection(ref velocity);
        if (velocity.y != 0) VerticalCollisionDetection(ref velocity);

        transform.Translate(velocity);
    }

    void ClimbAngle(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        //up distance = distance * sinTheta
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance*Mathf.Sign(velocity.x);
    }

    void Update()
    {
        
    }

    void HorizontalCollisionDetection(ref Vector3 velocity)
    {
        // f = -1 | down 1 = up
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + margin;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // IF ray origin moving left, cast from bottom left ELSE  cast from bottom right
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BL : raycastOrigins.BR;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, GroundCollision);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit&& hit.collider.gameObject.GetComponent<PlatformEffector2D>() == null)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0)
                {
                    //Debug.Log(string.Format("AngleHit: {0}", slopeAngle));
                    if(slopeAngle <= maxClimbAngle)
                    {
                        ClimbAngle(ref velocity, slopeAngle);
                    }
                }
                velocity.x = (hit.distance - margin) * directionX;
                rayLength = hit.distance;

                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }
    }

    void VerticalCollisionDetection(ref Vector3 velocity)
    {
        // f = -1 | down 1 = up
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + margin;
        for (int i = 0; i < verticalRayCount; i++)
        {
            // IF ray origin moving down, cast from bottom left ELSE  cast from top left
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BL : raycastOrigins.TL;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, GroundCollision);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY*rayLength, Color.red);

            if(hit)
            {
                if (hit.collider.gameObject.GetComponent<PlatformEffector2D>() == null||(directionY==-1&&ignoreOneWayCollision==false))
                {
                    velocity.y = (hit.distance - margin) * directionY;
                    rayLength = hit.distance;

                    collisionInfo.above = directionY == 1;
                    collisionInfo.below = directionY == -1;
                }
                
                

            }
        }
    }


	void UpdateRaycastOrigins ()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(margin * -2);

        raycastOrigins.BL = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.BR = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.TL = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.TR = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(margin * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

    }

    struct RaycastOrigins
    {
        //AABB Collision positions

        public Vector2 TL, TR;
        public Vector2 BL, BR;

    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void ResetBools()
        {
            above = false;
            below = false;
            left = false;
            right = false;
            
        }
    }

}
