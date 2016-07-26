using UnityEngine;
using System.Collections;

public class NewBlasterManager : MonoBehaviour {

    [Header("Current Blaster Damage")]
    [SerializeField] int standardDamage = 10;                         // The standard/charging damage dealt by all Blaster Shots.
    [SerializeField] int chargedDamage = 10;                          // The charged damage dealt by all Blaster Shots.    
    [SerializeField] int fusionDamage = 10;                           // The fusion damage dealt all Blaster Shots.
    [SerializeField] int spiralDamage = 10;                           // The damage dealt by the second charged Shot.

    [Header("Current Blaster Rapid & Charging")]
    [SerializeField] float rapidFireRate = 0;                         // Controls the rapid firing rate of all Blaster Shots.
    [SerializeField] float chargingTimer = 0.0f;                      // Used as a timer/counter for the Player's charging.
    [SerializeField] float fullyCharged = 0.0f;                       // Controls the time needed to charge lvl2 Blaster Shots.
    [SerializeField] float spiralCharged = 0.0f;                      // Controls the time needed to charge lvl3 Blaster Shots.       

    [Header("All Blasters Properties")]  
    [SerializeField] GameObject standardShot = null;                  // Holds the standard shot fired by the Player.    

    Transform firePoint;                                              // Used to get the local position of the firePoint GameObject.
    Controller2D player;                                              // Reference to the Controller2D configurations.
    NewBlasterManager arm;                                            // Reference to the NewBlasterManager configurations.

    void Start() {
        // Obtains the components from the Controller2D and NewBlasterManager Script.
        player = GetComponentInParent<Controller2D>();
        arm = FindObjectOfType<NewBlasterManager>();
    }

    void Awake() {
        // Obtains the firePoint's transform for its position usage.
        firePoint = transform.FindChild("FirePoint");
        // Checks to see if the firePoint still exists.
        if (firePoint == null) {
            Debug.LogError("No firePoint? WHAT?!");
        }                   
    }

    void Update() {
        // Used to obtain the x position and y position of firePoint.
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        // Fires the standard shot Prefab with the ShotTrail script doing the rest.
        if (Input.GetButtonDown("Shoot")) {
            // Checks for the rotation of the Arm prefab and any upward/downward inputs, if any.
            // While it appears to be 90 in the inspector, the actual rotation # is around .70f. 
            if ((arm.transform.localRotation.z == 0.0f && Input.GetAxisRaw("VerticalMove") == 0)
            || (arm.transform.localRotation.z > 0.700f && (Input.GetAxisRaw("VerticalMove") == 1
            || Input.GetAxisRaw("VerticalMove") == -1))) {
                // Spawns the standardShot from the firePointPosition.
                Instantiate(standardShot, firePointPosition, Quaternion.identity);
            }
        }

        // Takes a positive vertical input from the player when holding up.
        if (Input.GetAxisRaw("VerticalMove") == 1) {
            // Checks to see if the Player is moving/facing right.
            if (transform.localScale.x > 0) {
                // Used to make the Player shoot upwards when holding up.
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            
            // Checks to see if the Player is moving/facing left.
            if (transform.localScale.x < 0) {
                // Used to make the Player shoot upwards when holding up.
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }

        // Takes a negative vertical input from the player when holding down. Only in the air.
        if (Input.GetAxisRaw("VerticalMove") == -1 && !player.collisions.below) {
            // Checks to see if the Player is moving/facing right.
            if (transform.localScale.x > 0) {
                // Used to make the Player shoot downwards when holding down.
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }

            // Checks to see if the Player is moving/facing left.
            if (transform.localScale.x < 0) {
                // Used to make the Player shoot downwards when holding down.
                transform.rotation = Quaternion.Euler(0, 0, -270);
            }
        }

        // Takes a neutral vertical input from the player when nothing is held.
        if (Input.GetAxisRaw("VerticalMove") == 0) {
            // Prevents the Player from shoot upwards or downwards if the up/down button isn't being held.
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
