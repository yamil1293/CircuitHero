using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour { 

    // Calculates the Enemy's health and keeps it up to date.
    [System.Serializable] public class HealthCalculator {
        // Sets the maximum amount of health an Enemy has.
        [SerializeField] public int enemyMaxHealth = 3;

        // Used to collect and return the Enemy's current health.
        int calculatingEnemyHealth;
        // Values being handled under enemyCurrentHealth.
        public int enemyCurrentHealth {
            // Calculates Enemy's health and returns it under enemyCurrentHealth.
            get { return calculatingEnemyHealth; }
            set { calculatingEnemyHealth = Mathf.Clamp(value, 0, enemyMaxHealth); }
        }

        public void Init() {
            // Immediately sets the enemyCurrentHealth value with whatever is in enemyMaxHealth.
            enemyCurrentHealth = enemyMaxHealth;
        }
    }

    [SerializeField] StatusIndicator statusIndicator = null;              // Provides access to the StatusIndicator GameObject attached to the enemy.
    public HealthCalculator healthCalculator = new HealthCalculator();    // Obtains the numerical values from enemyMaxHealth and enemyCurrentHealth.  
    [SerializeField] int collisionDamage = 0;                             // Used to damage the Player when they collide with the enemy.
    [SerializeField] GameObject enemyDeathParticle = null;                // Creates deathParticles when the Enemy is killed. 
    [HideInInspector] bool enemyHasCollided = false;                      // Tracks whenever the Enemy makes a collision with the Player.

    void Start() {
        // Immedaitely passes the numeric value within enemyMaxHealth to enemyCurrentHealth.
        healthCalculator.Init();

        // Checks to see if a statusIndicator is made available in the inpsector.
        if (statusIndicator == null) {
            // Provides an alert to the Unity Editor's Console Log.
            Debug.LogError("No status indicator referenced on Enemy");
        }

        else {
            // Otherwise pass the values obtained in healthCalculator to the StatusIndicator.
            statusIndicator.SetHealth(healthCalculator.enemyCurrentHealth, healthCalculator.enemyMaxHealth);
        }
    }

    public void AssigningDamage(int blasterDamage) {
        // Enemy's health will decrease when colliding with another GameObject.
        healthCalculator.enemyCurrentHealth -= blasterDamage;

        // Plays the EnemyHurt Audio.
        GetComponent<AudioSource>().Play();
 
        // If the Enemy's health hits or goes below zero.
        if (healthCalculator.enemyCurrentHealth <= 0) {
            // When the enemy's current health goes to zero or lower,
            // release a particle in its place and destroy the Enemy's GameObject.
            Instantiate(enemyDeathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        // Otherwise continue to update the StatusIndicator with the latest enemyCurrentHealth values.
        statusIndicator.SetHealth(healthCalculator.enemyCurrentHealth, healthCalculator.enemyMaxHealth);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Checks to see if this Enemy has collided with the Player GameObject.
        if (other.gameObject.tag == "Player") {
            // The Enemy's GameObject will report that Collision with the Player is occuring.
            enemyHasCollided = true;

            // Pass the collisionDamage amount to the PlayerStatus script.
            other.GetComponent<PlayerStatus>().DamagePlayer(collisionDamage);

            // Checks to see if the Player is currently active within the game.
            if (!other.GetComponent<PlayerStatus>().gameObject.activeInHierarchy) {             
                // If not, this Enemy's GameObject will now report that Collision is no longer occuring.
                enemyHasCollided = false;
            }
        }
    }    

    void OnTriggerExit2D(Collider2D other) {
        // Checks to see if this Enemy is still colliding with the Player GameObject.
        if (other.gameObject.tag == "Player") {
            // If not, now this Enemy's GameObject will report that its collision is no longer occuring.
            enemyHasCollided = false;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        // Checks to see if this Enemy has collided with the Player GameObject.
        if (other.gameObject.tag == "Player") {
            // Checks to see if the Player is still currently colliding against this Enemy GameObject.
            if (enemyHasCollided == true) {
                // If so, pass the collisionDamage amount to the PlayerStatus script.
                GameObject.Find("Player").GetComponent<PlayerStatus>().DamagePlayer(collisionDamage);
            }
        }

        // Checks to see if the Player is currently active within the game.
        if (!other.GetComponent<PlayerStatus>().gameObject.activeInHierarchy) {
            // If not, the Enemy's GameObject will now report that Collision is no longer occuring.
            enemyHasCollided = false;
        }
    }
}
