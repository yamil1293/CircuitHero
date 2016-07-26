using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour { 

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

    [SerializeField] StatusIndicator statusIndicator = null;              // Provides access to a StatusIndicator GameObject attached to the player.
    GameMaster gameMaster;                                                // References the GameMaster configurations.        
    public HealthCalculator healthCalculator = new HealthCalculator();    // Obtains the numerical values from playerMaxHealth and playerCurrentHealth.  

    void Start() {
        // Immedaitely passes the numeric value within playerMaxHealth to playerCurrentHealth.
        healthCalculator.Init();

        // Locate and obtain access to the GameMaster prefab.
        gameMaster = FindObjectOfType<GameMaster>();

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
        // Player's health will decrease when colliding with another GameObject.
        healthCalculator.playerCurrentHealth -= outsideDamage;
        // Plays the PlayerHurt Audio.
        GetComponent<AudioSource>().Play();

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
}
