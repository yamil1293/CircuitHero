using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour{

    // Calculates the Player's health and keeps it up to date.
    [System.Serializable] public class HealthCalculator {
        // Sets the maximum amount of health an Enemy has.
        [SerializeField] public int playerMaxHealth = 100;

        // Used to collect and return the Player's current health.
        int calculatingPlayerHealth;
        // Values being handled under playerCurrentHealth.
        public int playerCurrentHealth {
            // Calculates Player Health and returns it under playerCurrentHealth.
            get { return calculatingPlayerHealth; }
            set { calculatingPlayerHealth = Mathf.Clamp(value, 0, playerMaxHealth); }
        }

        public void Init() {
            // Immediately sets the playerCurrentHealth value with whatever is in playerMaxHealth.
            playerCurrentHealth = playerMaxHealth;
        }
    }

    [SerializeField] StatusIndicator statusIndicator = null;              // Provides access to the StatusIndicator GameObject attached to the player.
    GameMaster gameMaster;                                                // References the GameMaster's script and prefab configurations. 
    public HealthCalculator healthCalculator = new HealthCalculator();    // HealthCalculator class is now accessible in other scripts and inspector.  

    [HideInInspector] public float lockingPlayerScale;                    // Obtains the Player's localScale when a collision occurs.
    [HideInInspector] bool playerHasCollided = false;                     // Tracks whenever the Player makes a collision with a hostile force.
    [HideInInspector] public bool wasKnockedBack = false;                 // Turns on when the Player collides with an enemy or a hazard.
    [HideInInspector] bool isInvincible = false;                          // Stays on for a period of time after a collision with an enemy or hazard.
    [SerializeField] float knockedBackTimer = 1.0f;                       // Sets a limit on when the Player should exit out of knockback.
    [SerializeField] float invincibilityTimer = 2.0f;                     // Sets a limit on when the Player should be able to take damage again.

    void Start() {
        // Immedaitely passes the numeric value within playerMaxHealth to playerCurrentHealth.
        healthCalculator.Init();
        // Locate and obtain access to the GameMaster prefab.
        gameMaster = FindObjectOfType<GameMaster>();

        // Checks to see if a statusIndicator is made available in the inpsector.
        if (statusIndicator == null) {
            // Provides an alert to the Unity Editor's Console Log.
            Debug.LogError("No status indicator referenced on Player");
        }

        else {
            // Otherwise pass the values obtained in healthCalculator to the StatusIndicator.
            statusIndicator.SetHealth(healthCalculator.playerCurrentHealth, healthCalculator.playerMaxHealth);
        }
    }

    public void DamagePlayer(int outsideDamage) {
        // If the Player is still recovering, don't add additional damage right now. 
        if (isInvincible == false) {
            // Player's health will decrease when colliding with another GameObject.      
            healthCalculator.playerCurrentHealth -= outsideDamage;

            if (healthCalculator.playerCurrentHealth > 0) {
                // Plays the PlayerHurt Audio.
                GetComponent<AudioSource>().Play();
            }
        }

        // If Player's health hits or goes below zero.
        if (healthCalculator.playerCurrentHealth <= 0) {
            // Activate the RespawnPlayer sequence in the GameMaster.
            gameMaster.RespawnPlayer();

            // Reset the Player's health amount to Max once he respawns. 
            healthCalculator.playerCurrentHealth = healthCalculator.playerMaxHealth;
        }

        // Otherwise continue to update the StatusIndicator with the latest playerCurrentHealth values.
        statusIndicator.SetHealth(healthCalculator.playerCurrentHealth, healthCalculator.playerMaxHealth);
    }
   
    void OnTriggerEnter2D(Collider2D other) {
        // Checks to see if the Player collides with an Enemy GameObject.
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Hazard") {
            // The Player's GameObject will report that Collision with the Enemy is occuring.
            playerHasCollided = true;

            // Checks to see if the Player's GameObject is active and not invincible.
            if (isInvincible == false && gameObject.activeInHierarchy) {
                // If so and the Player is attacked, start the Knockbacked procedure.
                StartCoroutine("KnockbackPlayerCoroutine");
            }
        }

        // Checks to see if the Player is interacting with a Checkpoint GameObject.
        if (other.gameObject.tag == "Checkpoint") {
            // If so, then respawn the Player with no collision booleans being active.
            playerHasCollided = false;
            wasKnockedBack = false;
            isInvincible = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // Checks to see if the Player collides with an Enemy GameObject.
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Hazard") {
            // The Player's GameObject will now report that no Collision is occuring.
            playerHasCollided = false;
        }     
    }

    void OnTriggerStay2D(Collider2D other) {
        // Checks to see if the Player collides with an Enemy GameObject.
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Hazard") {

            // Checks to see if the Player's GameObject is active, not invincible and is currently colliding with an Enemy.
            if (playerHasCollided == true && isInvincible == false && gameObject.activeInHierarchy) {
                // If so and the Player is attacked/colliding, start the Knockbacked procedure.
                StartCoroutine("KnockbackPlayerCoroutine");
            }
        }
    }

    IEnumerator KnockbackPlayerCoroutine() {
        // 1. Records what direction the player is facing for appropriate knockback. 
        lockingPlayerScale = transform.localScale.x;

        // 2. Sets the Player's status to currently being knockedback.
        wasKnockedBack = true;

        // 3. Sets the Player's status to currently being invincibile from damage.
        isInvincible = true;

        // 4. The Player continues to be knockedback until the Timer has passed.
        yield return new WaitForSeconds(knockedBackTimer);

        // 5. The Player is no longer knockedback. Still immune from damage.
        wasKnockedBack = false;

        // 6. The Player continues to be invincible until the Timer has passed.
        yield return new WaitForSeconds(invincibilityTimer);

        // 7. The Player is no longer invincible and can be damaged again.
        isInvincible = false;
    } 
}
