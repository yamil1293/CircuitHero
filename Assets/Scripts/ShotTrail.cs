using UnityEngine;
using System.Collections;

public class ShotTrail : MonoBehaviour {

    [SerializeField] int shotSpeed = 10;                              // The speed the shot can travel in the x and y axis.
    [SerializeField] int blasterDamage = 0;                           // The amount of damage this blaster shot deals to enemies. 
    [SerializeField] GameObject impactEffect = null;                  // This particle is created after every collision.

    BlasterManager arm;                                               // Reference to the BlasterManager configurations.
    Controller2D playerController;                                    // Reference to the Controller2D configurations.                                            
    Vector2 direction;                                                // Used to hold changes made to the x and y axis.

    void Start() {
        // Obtains the components from the Controller2D and BlasterManager Script.
        playerController = FindObjectOfType<Controller2D>();    
        arm = FindObjectOfType<BlasterManager>();

        // Checks to see if the Player's arm has not rotated or if the Player isn't holding up or down.
        if (arm.transform.localRotation.z == 0.0f && Input.GetAxisRaw("VerticalMove") == 0) {
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
    
    void OnTriggerEnter2D(Collider2D other) {
        // Checks for the Enemy GameObject's tag.
        if (other.tag == "Enemy") {
            // Pass the damage amount to the EnemyStatus script.
            other.GetComponent<EnemyStatus>().AssigningDamage(blasterDamage);
        }

        // Checks for other GameObjects' tags in case alternate actions need to be taken.
        if (other.tag == "Environmental" && other.tag == "Through") {
            // If the shot collides with the above tags, destroy the shot's GameObject. 
            Destroy(gameObject);
        }

        // If the shot Prefab collides with anything else that triggers it, destroy it.
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    } 
}
