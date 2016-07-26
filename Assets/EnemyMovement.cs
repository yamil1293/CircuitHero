using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyController2D))]
public class EnemyMovement : MonoBehaviour {

    [SerializeField] float enemyMoveSpeed = 0.0f;                    // Moves the enemy to a specific direction.
    [SerializeField] bool enemyMovesRight = false;                   // Used to change the enemy's direction when moving.

    // EnemyController2D enemyController;                            // Reference to the Controller2D configurations.

    void Start () {
        // Obtains the components from the Controller2D Script.
        // enemyController = GetComponent<EnemyController2D>();       
    }
	
	void Update () {
        
        // If the enemy is facing or moving right, keep going right.
        if (enemyMovesRight) {
            transform.Translate(enemyMoveSpeed * Time.deltaTime, 0, 0);
        }

        // If the enemy is facing or moving left, keep going left.
        else {
            transform.Translate(-enemyMoveSpeed * Time.deltaTime, 0, 0); ;
        }
        
	}

    void OnTriggerEnter2D(Collider2D other) {
        // Checks to see if the Enemy collides with the GameObject attached to this script.
        if (other.tag == "Environment") {
            // Moves the enemy to the opposite direction..
            Debug.Log("Enemy hit a wall!");
            enemyMovesRight = !enemyMovesRight;
        }
    }
}
