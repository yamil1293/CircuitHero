using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShotProperties))]
public class ShotDirection : MonoBehaviour {

    [SerializeField] int shotSpeed = 10;                              // The speed the shot can travel in the x and y axis.
    [HideInInspector] BlasterManager arm;                             // Reference to the BlasterManager configurations.
    [HideInInspector] Controller2D playerController;                  // Reference to the Controller2D configurations.                                            
    [HideInInspector] Vector2 direction;                              // Used to hold changes made to the x and y axis.

    void Start() {
        // Obtains the components from the Controller2D and BlasterManager Script.
        playerController = FindObjectOfType<Controller2D>();    
        arm = FindObjectOfType<BlasterManager>();

        // Checks to see if the Player's arm has not rotated.
       if (arm.transform.localRotation.z == 0.0f) {
            // Checks to see if the Player isn't holding up or down when shooting
            // or if the Player is shooting when holding down on the ground.
            if (Input.GetAxisRaw("VerticalMove") == 0              
                || (Input.GetAxisRaw("VerticalMove") == -1 && playerController.collisions.below )) {
                // Checks to see if the Player is moving/facing right.
                if (playerController.transform.localScale.x > 0) {
                    // Used to make the Player shoot towards the right.
                    direction = new Vector2(shotSpeed, 0);
                }

                // Checks to see if the Player is moving/facing left.
                if (playerController.transform.localScale.x < 0) {
                    // Used to make the Player shoot towards the left.
                    direction = new Vector2(-shotSpeed, 0);
                }
            }         
        }
                
        // Checks to see if the Player's arm has rotated 90 degrees.
        if (arm.transform.localRotation.z > 0.70f) {
            // Checks to see if the Player is holding up.
            if (Input.GetAxisRaw("VerticalMove") == 1) {
                // Used to make the Player shoot upwards.
                direction = new Vector2(0, shotSpeed);
            }

            // Checks to see if the Player is holding down.
            if (Input.GetAxisRaw("VerticalMove") == -1) {
                // Used to make the Player shoot downwards.
                direction = new Vector2(0, -shotSpeed);                
            }
        }
    }

    void Update () {
        // Adds force to whatever prefab is attached to it.
        // Direction is changed automatically based on the Player's action.        
        transform.Translate(direction * Time.deltaTime);        
    } 
}
