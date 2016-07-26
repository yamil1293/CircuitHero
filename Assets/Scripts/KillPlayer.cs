using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

    GameMaster gameMaster;                                        // Reference to the GameMaster configurations.  
    [SerializeField] int instantKillDamage = 0;                   // Damage the Player takes when they collide with the enemy.

    void Start () {
        // Obtains the components from the GameMaster Script.
        gameMaster = FindObjectOfType<GameMaster>();  
	}
		
    void OnTriggerEnter2D(Collider2D other) {
        // Checks to see if the Player collides with the GameObject attached to this script.
        if (other.name == "Player") {
            // Pass the instantKillDamage amount to the PlayerStatus script.
            other.GetComponent<PlayerStatus>().DamagePlayer(instantKillDamage);
        }
    }   
}
