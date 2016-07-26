using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    [SerializeField] static GameMaster gameMaster;                  // Controls the spawning of the Player and Enemies.
    public GameObject currentCheckpoint;                            // References the checkpoint GameObject the Player just passed.
    [SerializeField] float respawnDelay = 0;                        // Sets the amount of time needed before the Player responds.
    [SerializeField] GameObject deathParticle = null;               // Creates deathParticles when the Player is killed. 
    [SerializeField] GameObject respawnParticle = null;             // Creates respawnParticles when the Player is respawned.
    [SerializeField] CameraShake cameraShake = null;                // Shakes the Main Camera violently when triggered.
      
    private PlayerStatus player;                                   // Reference to the PlayerStatus script.



    [SerializeField] GameObject gameOverMenuCanvas = null;                        // Used to enable the Game Over Screen when death occurs.
    [SerializeField] string backToTheTitleScreen = null;                          // Allows the User to type in the Title Screen Scene that will be used for this button.







    void Awake() {
        // In case gameMaster prefab cannot be found.
        if (gameMaster == null) {
            // Find a game object prefab that has the tag selected instead.
            gameMaster = GameObject.Find ("GameMaster").GetComponent<GameMaster>();
        }
    }

    void Start() {
        // Obtains the components from the PlayerStatus Script.
        player = FindObjectOfType<PlayerStatus>();   
        
        // In case the mainCamera prefab cannot be found.
        if (cameraShake == null) {
            // Alert Unity that cameraShake isn't possible at this moment.
            Debug.LogError("No camera shake referenced in GameMaster.");
        }        
    }
     
    public void RespawnPlayer() {
        // 1. Spawn the deathParticles when the Player is killed in the game.
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);

        // 2. Shuts off the Player Prefab temporarily.
        player.gameObject.SetActive(false);


       // if (!gameOverMenuCanvas.SetActive) {
            // If the Player is killed, start the Respawning procedure.
           // StartCoroutine("RespawnPlayerCoroutine");
       // }
    }

    public IEnumerator RespawnPlayerCoroutine() {      
        // 3. Spawns the Player to the location of a recent checkpoint.
        player.transform.position = currentCheckpoint.transform.position;
 
        // 4. Prevents the Player from respawning until the timelimit has passed.
        yield return new WaitForSeconds(respawnDelay);

        // 5. Turns the Player Prefab back on again for continue play.
        player.gameObject.SetActive(true);

        // 6. When the Player respawns back at the checkpoint, release the respawnParticles.
        Instantiate(respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);         
    }
}
