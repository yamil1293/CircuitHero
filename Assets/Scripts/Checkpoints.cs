using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour {

     GameMaster gameMaster;                       // Reference to the GameMaster configurations.

    // Use this for initialization
    void Start() {
        // Obtains the components from the GameMaster Script.
        gameMaster = FindObjectOfType<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Checks to see if the Player collides with the GameObject attached to this script.
        if (other.tag == "Player") {
            // Changes which checkpoint the Player is currently using by colliding with it.
            gameMaster.currentCheckpoint = gameObject;
            Debug.Log("Activated Checkpoint " + transform.position);
        }
    } 
}
