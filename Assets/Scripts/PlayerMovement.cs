using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour {

    [Header("Regular Movement")]
    [SerializeField] float moveSpeed = 7.0f;                            // The speed the player can travel in the x axis.
    [SerializeField] float knockbackSpeed = 0.0f;                       // The speed the Player gets knockedback in the x & y-axis.

    [Header("Jump Movement")]
    [SerializeField] float minJumpHeight = 0f;                          // Set the minimum distance of how far the Player can jump.
    [SerializeField] float maxJumpHeight = 3.5f;                        // Set the maximum distance of how far the Player can jump.
    [SerializeField] float timeToJumpApex = .4f;                        // Set the time needed to reach the jump's heighest point.   

    [Header("Wall Jump Movement")]
    [SerializeField] Vector2 wallJumpClimb = new Vector2(7.5f, 16f);    // Set the speed when the Player is climbing walls.
    [SerializeField] Vector2 wallJumpOff = new Vector2(8.5f, 7f);       // Set the distance when the Player taps away from the wall.
    [SerializeField] Vector2 wallLeap = new Vector2(18f, 17f);          // Set the distance when the Player jumps away from the wall.
    [SerializeField] float wallSlideSpeedMax = 3;                       // Sets the limit of how fast the Player slides down walls.
    [SerializeField] float wallStickTime = .25f;                        // Sets how long the Player sticks on walls before sliding down.

    [Header("Dash Movement")]
    [SerializeField] float timeHeld = 0.0f;                             // Used as a timer/counter for the Player's dash.
    [SerializeField] float timeForFullDash = 0.3f;                      // Used as a limiter for the Player's dash.
    [SerializeField] float dashDistance = 10;                           // Multiplies the amount of force used for the dash.
    [SerializeField] float minDashForce = 0.0f;                         // Calculates the minimum distance force to dash.
    [SerializeField] float maxDashForce = 2.0f;                         // Calculates the maximum distance force to dash.

    float gravity;                                                      // Provides calculations for jumping and wall movement.  
    Vector3 velocity;                                                   // Moves the Player through the x and y-axis.  
    float maximumJumpVelocity;                                          // Calculates the minimum distance to jump.
    float minimumJumpVelocity;                                          // Calculates the maximum distance to jump.
    float velocityXSmoothing;                                           // Provides a gradual speed for the Player's x-axis. 
    float accelerationTimeInTheAir = .1f;                               // Used for calculating velocity in the x axis on the air.
    float accelerationTimeOnTheGround = .1f;                            // Used for calculating velocity in the x axis on the ground. 
    float timeToWallUnstick;                                            // Sets how long until the Player starts sliding. 
    float horizontalDashJumpForce;                                      // Calculates the distance possible for dashjumping.
    float dashJumpCoverage;                                             // Used for calculating the actual dashjumping.
    float lockingAllDashJumping = 0;                                    // Used to prevent any sort of mid-air dashjumping.

    Controller2D controller;                                            // Reference to the Controller2D configurations. 
    PlayerStatus playerKnockbackStatus;                                 // Reference to the PlayerStatus configurations.


    //float lockingAllAirDashes = 0;                                    // Used to limit the Player's air dashing amount. 


    void Start() {
        // Obtains the components from the Controller2D Script.
        controller = GetComponent<Controller2D>();
        // Locate and obtain access to the PlayerStatus script.
        playerKnockbackStatus = GetComponent<PlayerStatus>();

        // Formulas for gravity, jumping and walljumping.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maximumJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minimumJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        // Formulas for dashjumping.
        horizontalDashJumpForce = maxDashForce * dashDistance;
        dashJumpCoverage = horizontalDashJumpForce * .75f;
    }

    // Variable to keep track of dashing directions.
    bool flipped = false;

    void Flip() {
        // Perform a check to see if the Player hasn't been knockback yet.
        if (playerKnockbackStatus.wasKnockedBack == false) {
            // Flips the Player character by its scale value.
            Vector3 playerScale = transform.localScale;
            // Multiply the player's x local scale by -1.
            playerScale.x *= -1;
            transform.localScale = playerScale;

            // Prevents the player from keeping the dash's momentum if direction was changed.
            if (Input.GetButton("Dash")) {
                flipped = true;
            }
        }
    }

    void Update() {
        // Takes all horizontal input from the player when moving left or right.
        Vector2 input = new Vector2(Input.GetAxisRaw("HorizontalMove"), Input.GetAxisRaw("VerticalMove"));

        // If the input is moving the player right while the player is facing left...    
        if (Input.GetAxisRaw("HorizontalMove") < 0 && transform.localScale.x > 0) {
                // ... flip the player.
                Flip();
            }

        // Otherwise if the input is moving the player left while the player is facing right...
        else if (Input.GetAxisRaw("HorizontalMove") > 0 && transform.localScale.x < 0) {
            // ... flip the player.
            Flip();
        }

        // Determines collision direction when colliding against walls.
        int wallDirectionX = (controller.collisions.left) ? -1 : 1;

        // Movement calculations when the Player is moving left or right.
        float targetVelocityX = input.x * moveSpeed;
        float standardMovementVelocity = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)      
            ? accelerationTimeOnTheGround : accelerationTimeInTheAir);
        velocity.x = standardMovementVelocity;

        // Perform a check to see if the Player hasn't been knockback yet.
        if (playerKnockbackStatus.wasKnockedBack == false) {
            // Controls various aspects when moving against and on walls.
            bool wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
                // Tells the game that Player is sliding on a wall.
                wallSliding = true;

                // Prevents the velocity from going further then wallSlideSpeedMax.
                if (velocity.y < -wallSlideSpeedMax) {
                    velocity.y = -wallSlideSpeedMax;
                }

                // Determines how fast the Player unsticks from walls.
                if (timeToWallUnstick > 0) {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    // Gradually pushes the player off of the wall.
                    if (input.x != wallDirectionX && input.x != 0) {
                        timeToWallUnstick -= Time.deltaTime;
                    }

                    // Pushes the Player off of the wall immediately. 
                    else {
                        timeToWallUnstick = wallStickTime;
                    }
                }

                // Pushes the Player off of the wall immediately.
                else {
                    timeToWallUnstick = wallStickTime;
                }
            }

            // For jumping on walls and for maximum height.
            if (Input.GetButtonDown("Jump")) {
                // Code only executes when the Player is on a wall.
                if (wallSliding) {
                    // Executes only when the Player is jumping towards a wall.
                    if (wallDirectionX == input.x) {
                        velocity.x = -wallDirectionX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }

                    // Executes only when the Player is inactive on a wall.
                    else if (input.x == 0) {
                        velocity.x = -wallDirectionX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }

                    // Executes only when the Player is jumping away from a wall.
                    else {
                        velocity.x = -wallDirectionX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }

                    // Resets air dash counter as the Player is on a wall.
                    // lockingAllAirDashes = 0;

                    // Prevents the Player from dashjumping by this point.
                    if (!Input.GetButton("Dash")) {
                        lockingAllDashJumping++;
                    }
                }

                // Code only executes when the Player is on the ground.
                if (controller.collisions.below) {
                    // Used to provide the maximum jumping distance for the Player.                               
                    velocity.y = maximumJumpVelocity;

                    // Resets air dash counter as the Player lands on the ground.
                    // lockingAllAirDashes = 0;

                    // Prevents the Player from dashjumping by this point.
                    if (!Input.GetButton("Dash")) {
                        lockingAllDashJumping++;
                    }
                }
            }

            // For jumping with minimum height.
            if (Input.GetButtonUp("Jump")) {
                // Used to provide the minimum jumping distance for the Player.
                if (velocity.y > minimumJumpVelocity) {
                    velocity.y = minimumJumpVelocity;
                }

                // Resets air dash counter as the Player lands on the ground.
                // lockingAllAirDashes = 0;

                // Prevents the Player from dashjumping by this point.
                if (!Input.GetButton("Dash")) {
                    lockingAllDashJumping++;
                }
            }

            // If dashing jumping, allow the player to continue their dashing momentum.
            if ((Input.GetButton("Dash") && Input.GetButton("Jump")) && lockingAllDashJumping < 1) {
                // Used to provide dashjump momentum when moving towards the right.
                if (transform.localScale.x > 0) {
                    velocity.x = dashJumpCoverage;
                }

                // Used to provide dashjump momentum when moving towards the left.
                else if (transform.localScale.x < 0) {
                    velocity.x = -dashJumpCoverage;
                }

                // Stops the Player's dashing immediately if they jump without moving left or right.
                if (Input.GetAxisRaw("HorizontalMove") == 0) {
                    velocity.x = 0;
                }
            }        

            // Starts the timer when Dashing.
            if (Input.GetButtonDown("Dash")) { 
                // Starts the timeHeld counter for dash calculations.
                timeHeld = 0f;
            }

            // Lets the player starts Dashing once more when the button is released.
            if (Input.GetButtonUp("Dash")) {
                // Allows the Player to dash at a new direction once the button is released.
                flipped = false;
            }

            // For the main Dashing code.
            if (Input.GetButton("Dash")) {
                // Tracks the amount of time the Player holds the button down.
                timeHeld += Time.deltaTime;

                // Controls the timeHeld counter by using timeForFullDash as the limiter.
                if (timeHeld >= timeForFullDash) {
                    timeHeld = timeForFullDash;
                }

                // Formula used to calculate the Player's various dash movements.
                float horizontalDashForce = ((maxDashForce - minDashForce) * (timeHeld / timeForFullDash)) + minDashForce;
                float dashCoverage = horizontalDashForce * dashDistance;
 
                // If no change of direction occurs during the dash, dash formula will continue.
                if (flipped == false) {
                    // Used to provide ground dash velocity when moving/facing towards the right.
                    if (controller.collisions.below && transform.localScale.x > 0) {
                        velocity.x = dashCoverage;
                    }

                    // Used to provide ground dash velocity when moving/facing towards the left.
                    if (controller.collisions.below && transform.localScale.x < 0) {
                        velocity.x = -dashCoverage;
                    }
                }

                // Ends the dash with normal velocity as the timeHeld counter reaches timeForFullDash.
                if ((timeHeld == timeForFullDash) && controller.collisions.below) {
                    // Returns Player back to normal velocity and cancels any more dash momentum.                           
                    velocity.x = standardMovementVelocity;
                }
            }
        }

        // Perform a check to see if the Player hasn't been knockback yet.
        else if (playerKnockbackStatus.wasKnockedBack == true) {
            // If so check to see what direction the Player is currently facing when knockedback.
            if (playerKnockbackStatus.lockingPlayerScale > 0) {
                // If so, then forcibly push the Player towards the left when damaged.
                velocity = new Vector2(-knockbackSpeed, knockbackSpeed / 2.5f);
            }

            // If the Player was facing the left direction when knockedback...
            else if (playerKnockbackStatus.lockingPlayerScale < 0) {
                // Then forcibly push the Player towards the right when damaged.
                velocity = new Vector2(knockbackSpeed, knockbackSpeed / 2.5f);
            }
        }

        /*
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        // Code for air dashing, still needs work.
        if (Input.GetKey(KeyCode.X) && !controller.collisions.below && (totalAirDashes < 1)) {
            if (transform.localScale.x > 0) {
                velocity.x = airDashVelocity;
            }

            else {
                velocity.x = -airDashVelocity;
            }

            // Adds a counter to the air dash total.
            totalAirDashes++;
            Debug.Log(velocity.x);
            // Debug.Log(transform.localPosition);
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        */

        // Affects the game's gravity and movement inputs.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.fixedDeltaTime, input);

        if (controller.collisions.above || controller.collisions.below) {
            // Sets the Player's gravity to 0 if they're on the ground or against the ceiling.
            velocity.y = 0;
        }

        if (controller.collisions.below) {  
            // Brings back the opportunity to dashjumping again to the Player.
            lockingAllDashJumping = 0;
        }
    }
}
