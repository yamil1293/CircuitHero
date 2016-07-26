using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class PlayerConfig : MonoBehaviour {

    [SerializeField] float moveSpeed = 10f;                              // The speed the player can travel in the x axis.
    [SerializeField] float maximumJumpHeight = 4;
    [SerializeField] float minimumJumpHeight = 1;
    [SerializeField] float timeToJumpApex = .4f;

    [SerializeField] float dashDistance = 10;

    float accelerationTimeInTheAir = .1f;
    float accelerationTimeOnTheGround =.1f;

    [SerializeField] Vector2 wallJumpClimb  = new Vector2(0,0);
    [SerializeField] Vector2 wallJumpOff = new Vector2(0,0);
    [SerializeField] Vector2 wallLeap = new Vector2(0,0);


    [SerializeField] float wallSlideSpeedMax = 3;
    [SerializeField] float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maximumJumpVelocity;
    float minimumJumpVelocity;

    float dashVelocity = 2f;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;                                            // Reference to the Controller2D configurations.

    void Start() {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maximumJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maximumJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minimumJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minimumJumpHeight);
    }

    // Flips the Player character by its scale value
    void Flip() {
        // Multiply the player's x local scale by -1
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;        
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // If the input is moving the player right and the player is facing left...
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.localScale.x > 0) {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.localScale.x < 0) {
            // ... flip the player.
            Flip();
        }
  
        int wallDirectionX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)
                         ? accelerationTimeOnTheGround : accelerationTimeInTheAir);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0) {
                velocityXSmoothing = 0;
                velocity.x = 0;
                
                if (input.x != wallDirectionX && input.x != 0) {
                    timeToWallUnstick -= Time.deltaTime;
                }

                else {
                    timeToWallUnstick = wallStickTime;
                }
            }

            else {
                timeToWallUnstick = wallStickTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (wallSliding) {
                if (wallDirectionX == input.x) {
                    velocity.x = -wallDirectionX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }

                else if (input.x == 0) {
                    velocity.x = -wallDirectionX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }

                else {
                    velocity.x = -wallDirectionX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }

            if (controller.collisions.below) {
                if (input.y != -1) {
                    velocity.y = maximumJumpVelocity;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            if (velocity.y > minimumJumpVelocity) {
                velocity.y = minimumJumpVelocity;
            }
        }

        //Dashing Code
        if (Input.GetKey(KeyCode.Z)) {
            velocity.x = dashDistance + (Mathf.Sqrt(2 * Mathf.Abs(gravity) * dashVelocity));
            Debug.Log(velocity);
            Debug.Log(transform.localPosition);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.fixedDeltaTime, input);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }
}
