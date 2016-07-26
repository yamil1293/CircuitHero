using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour{

    public LayerMask collisionMask;                                 // Determines what GameObject this should collide with.
    public const float skinWidth = .015f;                           // Creates the inner boundaries for Raycast collisions.
    public int horizontalRayCount = 4;                              // Sets the amount of Raycasts being fired on the x-axis.
    public int verticalRayCount = 4;                                // Sets the amount of Raycasts being fired on the y-axis.

    [HideInInspector] public float horizontalRaySpacing;            // Calculating the spacing between each raycount on the x-axis.
    [HideInInspector] public float verticalRaySpacing;              // Calculating the spacing between each raycount on the y-axis.
    [HideInInspector] public BoxCollider2D boxCollider;             // Obtain all information from the BoxCollider2D.
    public RaycastOrigins raycastOrigins;                           // Allows us to call the structure within RaycastOrigins.

    public virtual void Awake() {
        // Placing the component information onto a variable from a box collider.
        boxCollider = GetComponent<BoxCollider2D>();      
    }

    public virtual void Start() {
        // Begin by calculating the Rayspacing in a GameObject.
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins() {
        // Obtaining the boundaries of our colliders.
        Bounds bounds = boxCollider.bounds;

        // Shruken in all sides by skinWidth.
        bounds.Expand(skinWidth * -2);

        // Now casting out raycasts from the positions listed below.
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing() {
        // Obtaining the boundaries of our colliders.
        Bounds bounds = boxCollider.bounds;
        // Shruken in all sides by skinWidth.
        bounds.Expand(skinWidth * -2);

        // Needs at least 2 Raycounts on both axis for this to work.
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        // Sets up the spacing between all horizontal and Vertical Raycasts.
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    // Determines the Raycast origins.
    public struct RaycastOrigins {
        // Creates all raycast positions from these specified locations.
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
