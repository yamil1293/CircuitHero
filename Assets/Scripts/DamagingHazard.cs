using UnityEngine;
using System.Collections;

public class DamagingHazard : MonoBehaviour {
    
    [SerializeField] int collisionDamage = 0;                             // Used to damage the Player when they collide with the enemy.
    [HideInInspector] bool hazardHasCollided = false;                     // Tracks whenever the Enemy makes a collision with the Player.
   
    void OnTriggerEnter2D(Collider2D other) {
        // Checks to see if this Obstacle has collided with the Player GameObject.
        if (other.gameObject.tag == "Player") {
            // This GameObject will report that Collision with the Player is occuring.
            hazardHasCollided = true;

            // Also, pass the instantKillDamage amount to the PlayerStatus script.
            other.GetComponent<PlayerStatus>().DamagePlayer(collisionDamage);

            // Checks to see if the Player is currently active within the game.
            if (!other.GetComponent<PlayerStatus>().gameObject.activeInHierarchy) {
                // If not, this GameObject will now report that Collision is no longer occuring.
                hazardHasCollided = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // Checks to see if this Obstacle has collided with the Player GameObject.
        if (other.gameObject.tag == "Player") {
            // If not, this GameObject will now report that its collision is no longer occuring.
            hazardHasCollided = false;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        // Checks to see if this Obstacle has collided with the Player GameObject.
        if (other.gameObject.tag == "Player") {
            // Checks to see if the Player is still currently colliding against this GameObject.
            if (hazardHasCollided == true) {
                // If so, pass the instantKillDamage amount to the PlayerStatus script.
                GameObject.Find("Player").GetComponent<PlayerStatus>().DamagePlayer(collisionDamage);
            }
        }       
    }
}
