using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets._2D
{
    [RequireComponent (typeof (Controller2D))]
    public class PlayerConfig : MonoBehaviour
    {
        bool facingRight = true;                                            // For determining which way the player is currently facing.

        [SerializeField] float maxSpeed = 10f;                              // The fastest the player can travel in the x axis.
        [SerializeField] float jumpForce = 400f;                            // Amount of force added when the player jumps.

        //Controller2D controller;                                            // Reference to the Controller2D configurations.
        Controller2DDemo controller;

        [SerializeField] bool airControl = false;                           // Whether or not a player can steer while jumping.
        [SerializeField] LayerMask whatIsGround;                            // A mask determining what is ground to the character.

        Transform groundCheck;                                              // A position marking where to check if the player is grounded.
        float groundedRadius = .2f;                                         // Radius of the overlap circle to determine if grounded.
        bool grounded = false;                                              // Whether or not the player is grounded.
        Transform ceilingCheck;                                             // A position marking where to check for ceilings.
        float ceilingRadius = .01f;                                         // Radius of the overlap circle to determine if the player can stand up.

        Animator animatorVariable;                                          // Reference to the player's animator component.
        Transform playerGraphics;                                           // Reference to the graphics so we can change direction.

        void Start() {
              controller = GetComponent<Controller2DDemo>();   
        }

        private void Awake()
        {
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
            animatorVariable.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);            
        }

        public void Move(float move, bool jump)
        {            
            // Only control the player if grounded or airControl is turned on.
            if (grounded || airControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                animatorVariable.SetFloat("Speed", Mathf.Abs(move));
                
                // Move the character
                GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
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
            // If the player should jump...
            if (grounded && jump)
            {
                // Add a vertical force to the player.
                grounded = false;
                animatorVariable.SetBool("Ground", false);
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            }
        }
        
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 thePlayerScale = playerGraphics.localScale;
            thePlayerScale.x *= -1;
            playerGraphics.localScale = thePlayerScale;
        }
    }
}
