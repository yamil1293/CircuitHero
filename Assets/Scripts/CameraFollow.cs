using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    FocusArea focusArea;                                            // Represents the FocusArea structure.

    [SerializeField] Controller2D target = null;                    // Sets the primary target for the Main Camera's focus.   
    [SerializeField] float verticalOffset = 1;                      // Shifts the Main Camera's position on the vertical axis.
    [SerializeField] float lookAheadDistanceX = 4;                  // Shifts the Main Camera's position on the horizontal axis.
    [SerializeField] float lookSmoothTimeX = 0.5f;                  // Smooths the movement made by the Main Camera on the x-axis.
    [SerializeField] float verticalSmoothTime = 0;                  // Smooths the movement made by the Main Camera on the y-axis.
    [SerializeField] Vector2 focusAreaSize = new Vector2 (3,5);     // Size of the focus area around the Target.

    float currentLookAheadX;                                        // Variable used for the calculations made in LateUpdate.
    float targetLookAheadX;                                         // Variable used for the calculations made in LateUpdate.
    float lookAheadDirectionX;                                      // Variable used for the calculations made in LateUpdate.
    float smoothLookVelocityX;                                      // Variable used for the calculations made in LateUpdate.
    float smoothVelocityY;                                          // Variable used for the calculations made in LateUpdate.

    bool stopLookingAhead;                                          // Prevents LateUpdate from checking the Target's movement.

    void Start() {
        // Update the focusArea variable with the target's bounds and focusAreaSize.
        focusArea = new FocusArea(target.boxCollider.bounds, focusAreaSize);
    }

    void LateUpdate() {
        // Check to see if the target's Prefab is still active in the scene/game.
        if (target.isActiveAndEnabled == true) {
            // Camera will constantly update to track the target's main position. 
            focusArea.Update(target.boxCollider.bounds);
        }

        // Formula to calculate the vertical frame change from the Main Camera.
        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        // Move the camera as the Target moves.
        if (focusArea.velocity.x != 0) {
            lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);

            // Snaps the Camera's movement to the correction direction when the Target changes direction as well.
            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
                stopLookingAhead = false;
                targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
            }

            // Stops the Camera in its tracks when the Target stops moving.
            else {
                if (!stopLookingAhead) {
                    stopLookingAhead = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4f;
                }
            }
        }
             
        // Takes in both the Position and then smooths all movements made by the Main Camera.
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos() {
        // Provides a visible Gizmo that can be seen when the Game is being tested in Unity. 
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube (focusArea.center, focusAreaSize);
    }

    struct FocusArea {
        // Information of the FocusArea's center, left, right, top and bottom positions.
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size) {
            // Calculating the size of the bounding box around the Target.
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            // Calculates the center of the FocusArea and tracks the target's velocity.
            velocity = Vector2.zero;            
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }        

        public void Update(Bounds targetBounds) {
            // Target is currently within the center of the bounding box.
            float shiftX = 0;
            // Checks if the target is moving against the left edge.
            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            }

            // Checks if the target is moving against the right edge.
            else if (targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            
            // Shifts the Target within the bounding box when they are moving left or right.
            left += shiftX;
            right += shiftX;

            // Target is currently within the center of the bounding box.
            float shiftY = 0;
            // Checks if the target is moving against the bottom edge.
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            }

            // Checks if the target is moving against the top edge.
            else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }

            // Shifts the Target within the bounding box when they are moving up or down.
            top += shiftY;
            bottom += shiftY;

            // Update the center position and record how far the Target is moving the bounding box.
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }       
}
