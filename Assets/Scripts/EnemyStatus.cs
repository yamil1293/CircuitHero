using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {

    [SerializeField] int enemyMaxHealth = 3;                      // Sets the maximum amount of health an Enemy has.
    private int enemyCurrentHealth = 0;                           // Keeps track of the enemy's current Health.
    [SerializeField] int collisionDamage = 0;                     // Used to damage the Player when they collide with the enemy.
    [SerializeField] GameObject enemyDeathParticle = null;        // Creates deathParticles when the Enemy is killed. 
    private EnemyStatus enemy;                                    // Reference to the EnemyStatus script.

    void Start() {
        // Obtains the components from the PlayerStatus Script.
        enemy = FindObjectOfType<EnemyStatus>();
        // Have enemyCurrentHealth take control and keep track of the enemy's remaining health.
        enemyCurrentHealth = enemyMaxHealth;
    }   

    void Update() {
        if (enemyCurrentHealth <= 0) {
            // When the enemy's current health goes to zero or lower,
            // release a particle in its place and destroy the Enemy's GameObject.
            Instantiate(enemyDeathParticle, enemy.transform.position, enemy.transform.rotation);
            Destroy(gameObject);           
        }
    }

    public void AssigningDamage(int blasterDamage) {
        // Subtracts the current value from enemyMaxHealth by the playerBlasterDamage.
        enemyCurrentHealth -= blasterDamage;
        // Plays the EnemyHurt Audio.
        GetComponent<AudioSource>().Play();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Checks for the Player GameObject's tag.
        if (other.tag == "Player") {
            // Pass the collisionDamage amount to the PlayerStatus script.
            other.GetComponent<PlayerStatus>().DamagePlayer(collisionDamage);
        }
    }
}
