using UnityEngine;
using System.Collections;

    [RequireComponent (typeof (Controller2D))]
    public class PlayerConfig : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 10f;                              // The speed the player can travel in the x axis.
        [SerializeField] float maximumJumpHeight = 4;
        [SerializeField] float minimumJumpHeight = 1;
        
        [SerializeField] float timeToJumpApex = .4f;

        float accelerationTimeInTheAir = .2f;
        float accelerationTimeOnTheGround =.1f;

        [SerializeField] Vector2 wallJumpClimb;
        [SerializeField] Vector2 wallJumpOff;
        [SerializeField] Vector2 wallLeap;

        [SerializeField] float wallSlideSpeedMax = 3;
        [SerializeField] float wallStickTime = .25f;
        float timeToWallUnstick;    
        
        float gravity;
        float maximumJumpVelocity;
        float minimumJumpVelocity;
        Vector3 velocity;
        float velocityXSmoothing;  

        Controller2D controller;                                            // Reference to the Controller2D configurations.

        [SerializeField] bool airControl = false;                           // Whether or not a player can steer while jumping.
        [SerializeField] LayerMask whatIsGround = 0;                        // A mask determining what is ground to the character.

        Transform groundCheck;                                              // A position marking where to check if the player is grounded.
        float groundedRadius = .2f;                                         // Radius of the overlap circle to determine if grounded.
        bool grounded = false;                                              // Whether or not the player is grounded.
        Transform ceilingCheck;                                             // A position marking where to check for ceilings.
        
        Animator animatorVariable;                                          // Reference to the player's animator component.
        Transform playerGraphics;                                           // Reference to the graphics so we can change direction.
                           
               

        void Start() {
            controller = GetComponent<Controller2D>();

            gravity = -(2 * maximumJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            maximumJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minimumJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minimumJumpHeight);
        }            

        void Update()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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

                    if (timeToWallUnstick > 0)
                    {
                        velocityXSmoothing = 0;
                        velocity.x = 0;

                        if (input.x != wallDirectionX && input.x != 0)
                        {
                            timeToWallUnstick -= Time.deltaTime;
                        }
                        else
                        {
                            timeToWallUnstick = wallStickTime;
                        }
                    }
                    else {
                        timeToWallUnstick = wallStickTime;
                    }
             }           
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (wallSliding) {
                    if (wallDirectionX == input.x)
                    {
                        velocity.x = -wallDirectionX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirectionX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    else {
                        velocity.x = -wallDirectionX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }
                }
                if (controller.collisions.below) {
                    velocity.y = maximumJumpVelocity;
                }              
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (velocity.y > minimumJumpVelocity)
                {
                    velocity.y = minimumJumpVelocity;
                }
            }
            
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
    }   
        
        /*  
        private void Awake()
        {         
            //movingPlayerCharacter = GetComponent<PlayerConfig>();
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            animatorVariable = GetComponent<Animator>();
            playerGraphics = transform.FindChild("Graphics");
            if (playerGraphics == null)
            {
                Debug.LogError("Let's freak out! There is no 'Graphics' object as a child of the player");
            }
        }

        private void FixedUpdate()
        {
            grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground.
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    grounded = true;
            }          

            animatorVariable.SetBool("Ground", grounded);
            // Set the vertical animation.
            animatorVariable.SetFloat("vSpeed", velocity.y);                               
        }

        public void Move(float move, bool jump)
        {                     
            // Only control the player if grounded or airControl is turned on.
            if (grounded || airControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                animatorVariable.SetFloat("Speed", Mathf.Abs(move));
                
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                {
                    // ... flip the player.
                    Flip();
                }              
            }
        }        
        
        private void Flip()
        {
            // Switch the way the player is labelled as facing
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
        }
        */    
    }
