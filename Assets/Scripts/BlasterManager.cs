using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlasterCalculations))]
[RequireComponent(typeof(PrefabBlasterManager))]
public class BlasterManager : MonoBehaviour {

    Transform firePoint;                                          // Used to get the local position of the firePoint GameObject.
    Controller2D playerControl;                                   // References the Controller2D configurations.
    BlasterManager blasterManager;                                // References the BlasterManager configurations.
    PrefabBlasterManager prefabBlasterManager;                    // References the PrefabBlasterManager configurations.
    BlasterCalculations blasterCalculations;                      // References the BlasterCalculations configurations.

    public bool powerModeIsOn = false;                            // Used to lock in Power Mode when it is being used.
    public bool magneticModeIsOn = false;                         // Used to lock in Magnetic Mode when it is being used.
    public bool thermalModeIsOn = false;                          // Used to lock in Thermal Mode when it is being used.
    public bool diffusionModeIsOn = false;                        // Used to lock in Diffusion Mode when it is being used.

    public bool isThisAStandardShot = false;                      // Used to lock in Standard Shot when it is being fired.
    public bool isThisAChargingShot = false;                      // Used to lock in Charging Shot when it is being fired.
    public bool isThisAChargedShot = false;                       // Used to lock in Charged Shot when it is being fired.
    public bool isThisASecondChargedShot = false;                 // Used to lock in Second Charged Shot when it is being fired.

    [HideInInspector] bool secondChargedShotChecked = false;      // Checks to see if a Second Charged Blaster shot is possible.

    void Awake() {
        // Obtains the firePoint's transform for its position usage.
        firePoint = transform.FindChild("FirePoint");

        if (firePoint == null) {
            // Provides an alert that the firePoint no longer exists.
            Debug.LogError("No firePoint? WHAT?!");
        }                   
    }

    void Start() {
        // Obtains the components from the Controller2D script.
        playerControl = GetComponentInParent<Controller2D>();
        // Obtains the components from the BlasterManager script.
        blasterManager = FindObjectOfType<BlasterManager>();
        // Obtains the components from the PrefabBlasterManager script.
        prefabBlasterManager = GetComponent<PrefabBlasterManager>();
        // Obtains the components from the BlasterCalculations script.
        blasterCalculations = GetComponent<BlasterCalculations>();
 
        // If all 4 locks (power, magnetic, thermal and diffusion) are false, make powerModeIsOn true by default.
        if (powerModeIsOn == false && magneticModeIsOn == false && thermalModeIsOn == false && diffusionModeIsOn == false) {
            // powerModeIsOn will become true when everything else is (or becomes) false.
            powerModeIsOn = true;
        }

        // If all 4 checks (standard, charging, charged, and second charged) are false, make isThisAStandardShot true by default.
        if (isThisAStandardShot == false && isThisAChargingShot == false && isThisAChargedShot == false && isThisASecondChargedShot == false) {
            // isThisAStandardShot will become true when everything else is (or becomes) false.
            isThisAStandardShot = true;
        }
    }

    void Update() {
        // Used to obtain the x and y position of firePoint.
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        // Checks to see if the Player presses the corresponding buttons to activate a Blaster Mode.
        if (Input.GetAxisRaw("HorizontalSelect") < 0 || Input.GetAxisRaw("HorizontalSelect") > 0 || Input.GetAxisRaw("VerticalSelect") < 0 || 
            Input.GetAxisRaw("VerticalSelect") > 0) {
            // If the Player does press a Blaster Mode button, start the Blaster Select procedure.
            StartCoroutine("BlasterSelectCoroutine");
        }

        // Checks to see if the Player presses the Shoot button.
        if (Input.GetButtonDown("Shoot") || Input.GetButton("Shoot") || Input.GetButtonUp("Shoot")) {
            // If the Player does press the Shoot button, start the Charging Status procedure.
            StartCoroutine("ChargingStatusCoroutine");
        }

        // Checks for the rotation of the Arm prefab and any upward/downward inputs, if any.
        // While it appears to be 90 in the inspector, the actual rotation # is around .70f. 
        if ((blasterManager.transform.localRotation.z == 0.0f && Input.GetAxisRaw("VerticalMove") == 0)           
           || (blasterManager.transform.localRotation.z == 0.0f && Input.GetAxisRaw("VerticalMove") == -1 && playerControl.collisions.below)
           || (blasterManager.transform.localRotation.z > 0.700f && (Input.GetAxisRaw("VerticalMove") == 1 || Input.GetAxisRaw("VerticalMove") == -1))) {
                     
            // Fires the standard/spiral shot Prefab with the ShotTrail script doing the rest.        
            if (Input.GetButtonDown("Shoot")) {
                // Starts the chargingTimer counter which alters what shot is being fired.
                blasterCalculations.chargingTimer = 0f;

                // Checks to see if a Second Charged Shot is ready to be fired. If not...
                if (secondChargedShotChecked == false) {
                    // Spawn the standardShot from the firePointPosition.
                    Instantiate(prefabBlasterManager.standardShot, firePointPosition, Quaternion.identity);
                }

                // Checks to see if a Second Charged Shot is ready to be fired. If so...
                if (secondChargedShotChecked == true) {
                    // Spawn the secondChargedShot from the firePointPosition.
                    Instantiate(prefabBlasterManager.secondChargedShot, firePointPosition, Quaternion.identity);
                    // Make isThisASecondChargedShot false.
                    secondChargedShotChecked = false;
                }
            }

            // Used to control the charging rate of the Blaster as long as the button is being held.
            if (Input.GetButton("Shoot")) {
                // Tracks the amount of time the Player holds the button down.
                blasterCalculations.chargingTimer += Time.deltaTime;

                // Checks to see if the chargingTimer has reached or surpased the secondChargedLimit.
                if (blasterCalculations.chargingTimer >= blasterCalculations.secondChargedLimit) {
                    // Lock the chargingTimer to the secondChargedLimit amount.
                    blasterCalculations.chargingTimer = blasterCalculations.secondChargedLimit;
                    // Make isThisASecondChargedShot true.
                    secondChargedShotChecked = true;
                }
            }

            // Used to fire a stronger Blaster shot if the chargingTimer has passed a certain limit.
            if (Input.GetButtonUp("Shoot")) {
                // Checks to see if the chargingTimer hasn't passed the beginCharging Limit to prevent accidental charging.
                // Also checks to see if the chargingTimer hasn't reached/passed the fullyChargedLimit.
                if (blasterCalculations.beginCharging < blasterCalculations.chargingTimer 
                    && blasterCalculations.chargingTimer < blasterCalculations.fullyChargedLimit) {
                    // If so, instantiate the chargingShot from the firePointPosition.
                    Instantiate(prefabBlasterManager.chargingShot, firePointPosition, Quaternion.identity);
                }

                // Checks to see if the chargingTimer has reached or surpased the fullyChargedLimit.
                if (blasterCalculations.chargingTimer >= blasterCalculations.fullyChargedLimit) {
                    // If so, instantiate the chargedShot from the firePointPosition.
                    Instantiate(prefabBlasterManager.chargedShot, firePointPosition, Quaternion.identity);
                }
            }
        }

        // Checks to see if the Player alters the Vertical Move variable by aiming up, down or moving back to default.
        if (Input.GetAxisRaw("VerticalMove") == 1 || Input.GetAxisRaw("VerticalMove") == -1 || Input.GetAxisRaw("VerticalMove") == 0) {
            // If the Player aims up, down or moves back to default, start the Arm Rotation procedure.
            StartCoroutine("ArmRotationCoroutine");
        }
    }

    IEnumerator BlasterSelectCoroutine() {
        // If the Player presses this button for Power Mode...
        if (Input.GetAxisRaw("VerticalSelect") > 0) { 
            // powerModeIsOn will switch to true while everything else will become false.
            powerModeIsOn = true;
            magneticModeIsOn = false;
            thermalModeIsOn = false;
            diffusionModeIsOn = false;
            // Stop the BlasterSelectCoroutine right here.
            yield return null;
        }

        // If the Player presses this button for Magnetic Mode...
        else if (Input.GetAxisRaw("HorizontalSelect") > 0) {
            // magneticModeIsOn will switch to true while everything else will become false.
            powerModeIsOn = false;
            magneticModeIsOn = true;
            thermalModeIsOn = false;
            diffusionModeIsOn = false;
            // Stop the BlasterSelectCoroutine right here.
            yield return null;
        }

        // If the Player presses this button for Thermal Mode...
        else if (Input.GetAxisRaw("VerticalSelect") < 0) {
            // thermalModeIsOn will switch to true while everything else will become false.
            powerModeIsOn = false;
            magneticModeIsOn = false;
            thermalModeIsOn = true;
            diffusionModeIsOn = false;
            // Stop the BlasterSelectCoroutine right here.
            yield return null;
        }

        // If the Player presses this button for Diffusion Mode...
        else if (Input.GetAxisRaw("HorizontalSelect") < 0) {
            // diffusionModeIsOn will switch to true while everything else will become false.
            powerModeIsOn = false;
            magneticModeIsOn = false;
            thermalModeIsOn = false;
            diffusionModeIsOn = true;
            // Stop the BlasterSelectCoroutine right here.
            yield return null;
        }                         
    }

    IEnumerator ArmRotationCoroutine() {
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

            // Stop the ArmRotationCoroutine right here.
            yield return null;
        }

        // Takes a negative vertical input from the player when holding down.
        if (Input.GetAxisRaw("VerticalMove") == -1) {
            // Code checks if the negative vertical input is done in the air.
            if (!playerControl.collisions.below) {
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

            // Code checks if the negative vertical input is done in the ground.
            if (playerControl.collisions.below) {
                // Snaps the Player's arm back to its default position regardless of button press.
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            // Stop the ArmRotationCoroutine right here.
            yield return null;      
        }

        // Takes a neutral vertical input from the player when nothing is held.
        if (Input.GetAxisRaw("VerticalMove") == 0) {
            // Prevents the Player from shoot upwards or downwards if the up/down button isn't being held.
            transform.rotation = Quaternion.Euler(0, 0, 0);
            // Stop the ArmRotationCoroutine right here.
            yield return null;
        } 
    }
   
    IEnumerator ChargingStatusCoroutine() {
        // Checks to see if a Standard Shot is possible.
        if (blasterCalculations.chargingTimer < blasterCalculations.beginCharging && secondChargedShotChecked == false) {       
            // Set isThisAStandardShot to true, make the rest false.
            isThisAStandardShot = true;
            isThisAChargingShot = false;
            isThisAChargedShot = false;
            isThisASecondChargedShot = false;
        }

        // Checks to see if a Charging Shot is possible.
        else if (blasterCalculations.beginCharging < blasterCalculations.chargingTimer 
            && blasterCalculations.chargingTimer < blasterCalculations.fullyChargedLimit) {
            // Set isThisAChargingShot to true, make the rest false.
            isThisAStandardShot = false;
            isThisAChargingShot = true;
            isThisAChargedShot = false;
            isThisASecondChargedShot = false;

            // When the Player releases the button here, make this true.
            if (Input.GetButtonUp("Shoot")) {
                isThisAStandardShot = true;
            }
        }

        // Checks to see if a Charged Shot is possible.
        else if (blasterCalculations.chargingTimer > blasterCalculations.fullyChargedLimit 
            && blasterCalculations.chargingTimer < blasterCalculations.secondChargedLimit) {
            // Set isThisAChargedShot to true, make the rest false.
            isThisAStandardShot = false;
            isThisAChargingShot = false;
            isThisAChargedShot = true;
            isThisASecondChargedShot = false;

            // When the Player releases the button here, make this true.
            if (Input.GetButtonUp("Shoot")) {
                isThisAStandardShot = true;
            }
        }

        // Checks to see if a Spiral Shot is possible.
        else if (blasterCalculations.chargingTimer >= blasterCalculations.secondChargedLimit) {
            // Set isThisASpiralShot to true, make the rest false.
            isThisAStandardShot = false;
            isThisAChargingShot = false;
            
            // Keep this true until the Player releases the Shoot Button.
            while (Input.GetButton("Shoot")) {
                isThisAChargedShot = true;
                yield return null;
            }

            // Once the Shoot button is let go, set everything else to false.
            isThisAChargedShot = false;
            isThisASecondChargedShot = true;
        }
        yield return null;
    }       
}
