using UnityEngine;
using System.Collections;

public class ShotTrail : MonoBehaviour {

    [SerializeField] int shotSpeed = 10;                              // The speed the shot can travel in the x and y axis.
    [SerializeField] LayerMask whatToHit;                             // Determines what shots collides with what.

    NewBlasterManager arm;                                            // Reference to the NewBlasterManager configurations.
    Controller2D player;                                              // Reference to the Controller2D configurations.                                            
    Vector2 direction;                                                // Used to hold changes made to the x and y axis.
    
    void Start() {
        // Obtains the components from the Controller2D and NewBlasterManager Script.
        player = FindObjectOfType<Controller2D>();    
        arm = FindObjectOfType<NewBlasterManager>();

        // Checks to see if the Player's arm has not rotated or if the Player isn't holding up or down.
        if (arm.transform.localRotation.z == 0.0f && Input.GetAxisRaw("VerticalMove") == 0) {
            // Checks to see if the Player is moving/facing right.
            if (player.transform.localScale.x > 0) {
                // Used to make the Player shoot towards the right.
                direction = new Vector2(shotSpeed, 0);                       
            }

            // Checks to see if the Player is moving/facing left.
            if (player.transform.localScale.x < 0) {
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
        // Checks for the other GameObject's tag and see if it can be destroyed.
        if (other.tag == "Enemy") {
            Destroy(other.gameObject);
        }

        // If the shot Prefab collides with something that triggers it, destroy it.
        Destroy(gameObject);
    }
}
