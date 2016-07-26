using UnityEngine;
using System.Collections;

public class BlasterManager : MonoBehaviour {

    [Header("Charging Blaster")]
    [SerializeField] float chargingTimer = 0.0f;                      // Used as a timer/counter for the Player's charging.
    [SerializeField] float fullyChargedLimit = 0.0f;                  // Controls the time needed to charge lvl2 Blaster Shots.
    [SerializeField] float spiralChargedLimit = 0.0f;                 // Controls the time needed to charge lvl3 Blaster Shots.
    [HideInInspector] bool spiralShotIsReady = false;                 // Controls whether a standard or a spiral shot is being fired.

    [Header("Firing Blaster")]  
    [SerializeField] GameObject standardShot = null;                  // Holds the standard shot fired by the Player.
    [SerializeField] GameObject chargingShot = null;                  // Holds the charging shot fired by the Player.    
    [SerializeField] GameObject chargedShot = null;                   // Holds the charged shot fired by the Player.    
    [SerializeField] GameObject spiralShot = null;                    // Holds the spiral shot fired by the Player.

    Transform firePoint;                                              // Used to get the local position of the firePoint GameObject.
    Controller2D playerControl;                                       // References the Controller2D configurations.
    BlasterManager arm;                                               // References the BlasterManager configurations.

    void Start() {
        // Obtains the components from the Controller2D and BlasterManager Script.
        playerControl = GetComponentInParent<Controller2D>();
        arm = FindObjectOfType<BlasterManager>();
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

        // Checks for the rotation of the Arm prefab and any upward/downward inputs, if any.
        // While it appears to be 90 in the inspector, the actual rotation # is around .70f. 
        if ((arm.transform.localRotation.z == 0.0f && Input.GetAxisRaw("VerticalMove") == 0)
           || (arm.transform.localRotation.z > 0.700f && (Input.GetAxisRaw("VerticalMove") == 1
           || Input.GetAxisRaw("VerticalMove") == -1))) {

            // Fires the standard/spiral shot Prefab with the ShotTrail script doing the rest.        
            if (Input.GetButtonDown("Shoot")) {
                // Starts the chargingTimer counter which alters what shot is being fired.
                chargingTimer = 0f;

                // Checks to see if a Spiral Shot is ready to be fired. If not...
                if (spiralShotIsReady == false) {
                    // Spawn the standardShot from the firePointPosition.
                    Instantiate(standardShot, firePointPosition, Quaternion.identity);
                }

                // Checks to see if a Spiral Shot is ready to be fired. If so...
                if (spiralShotIsReady == true) {
                    // Spawn the spiralShot from the firePointPosition.
                    // Then set the spiralShot check back to false so it can only be done once per charge.
                    Instantiate(spiralShot, firePointPosition, Quaternion.identity);
                    spiralShotIsReady = false;
                }
            }

            // Used to control the charging rate of the Blaster as long as the button is being held.
            if (Input.GetButton("Shoot")) {
                // Tracks the amount of time the Player holds the button down.
                chargingTimer += Time.deltaTime;

                // Checks to see if the chargingTimer has reached or surpased the spiralChargedLimit.
                if (chargingTimer >= spiralChargedLimit) {
                    // If so, set the spiralShot check to true to instantiate it.
                    spiralShotIsReady = true;
                }
            }

            // Used to fire a stronger Blaster shot if the chargingTimer has passed a certain limit.
            if (Input.GetButtonUp("Shoot")) {
                // Checks to see if the chargingTimer has exceeded 0.2f to prevent accidental charging.
                // Also check to see if the chargingTimer hasn't reached the fullyChargedLimit.
                if (0.35f < chargingTimer && chargingTimer < fullyChargedLimit) {
                    // If so, instantiate the chargingShot from the firePointPosition.
                    Instantiate(chargingShot, firePointPosition, Quaternion.identity);
                }

                // Checks to see if the chargingTimer has reached or surpased the fullyChargedLimit.
                if (chargingTimer >= fullyChargedLimit) {               
                    // If so, instantiate the chargedShot from the firePointPosition.
                    Instantiate(chargedShot, firePointPosition, Quaternion.identity);
                }
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

        // Takes a negative vertical input from the player when holding down.
        if (Input.GetAxisRaw("VerticalMove") == -1) {
            // Code checks if the negative vertical input is done in the air.
            if (!playerControl.collisions.below) {
                // Checks to see if the Player is moving/facing right.
                if (transform.localScale.x > 0)                {
                    // Used to make the Player shoot downwards when holding down.
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                }

                // Checks to see if the Player is moving/facing left.
                if (transform.localScale.x < 0)                {
                    // Used to make the Player shoot downwards when holding down.
                    transform.rotation = Quaternion.Euler(0, 0, -270);
                }
            }

            // Code checks if the negative vertical input is done in the ground.
            if (playerControl.collisions.below) {
                // Snaps the Player's arm back to its default position regardless of button press.
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }       
        }

        // Takes a neutral vertical input from the player when nothing is held.
        if (Input.GetAxisRaw("VerticalMove") == 0) {
            // Prevents the Player from shoot upwards or downwards if the up/down button isn't being held.
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
