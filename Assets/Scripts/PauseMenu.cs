using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    [SerializeField] bool isGamePaused = false;                                   // Checks the game to see if the Player paused it (or not).
    [HideInInspector] PlayerMovement playerMovementController;                    // Reference the PlayerMovement configurations.
    [HideInInspector] BlasterManager armMovementController;                       // Reference the BlasterManager configurations.
    [HideInInspector] CameraFollow cameraMovementController;                      // Reference the CameraFollow configurations.

    [SerializeField] GameObject pauseMenuCanvas = null;                           // Used to enable/disable the Pause Screen when playing.
    [SerializeField] GameObject gameOverMenuCanvas = null;                        // Used to enable the Game Over Screen when death occurs.
    [SerializeField] string returnToCurrentLevel = null;                          // Records the current Game Scene the Player is in for respawning with this button.
    [SerializeField] string backToTheTitleScreen = null;                          // Allows the User to type in the Title Screen Scene that will be used for this button.

    void Start() {
        // Obtains the components from the PlayerMovement, CameraFollow and BlasterManager Script.
        playerMovementController = FindObjectOfType<PlayerMovement>();
        armMovementController = FindObjectOfType<BlasterManager>();
        cameraMovementController = FindObjectOfType<CameraFollow>();
    }

    void Update() {
        // Checks to see if the Player has paused the game.
        if (isGamePaused == true) {
            // Enable the Pause Menu Canvas to show that the game is paused.
            pauseMenuCanvas.SetActive(true);
            // Disable all forms of movement from the Player while the game is paused.
            playerMovementController.enabled = false;
            armMovementController.enabled = false;
            cameraMovementController.enabled = false;

            // Stops other forms of movement made by GameObjects in that scene. 
            Time.timeScale = 0.0f;
        }

        // Checks to see if the Player has unpaused the game.
        else {
            // Disable the Pause Menu Canvas to show that the game is unpaused.
            pauseMenuCanvas.SetActive(false);
            // Re-enabled all forms of movement from the Player when the game is unpaused.
            playerMovementController.enabled = true;
            armMovementController.enabled = true;
            cameraMovementController.enabled = true;

            // Resumes other forms of movement made by GameObjects in that scene.
            Time.timeScale = 1.0f;
        }      

        // If the Player presses the Start Button to pause the game...
        if (Input.GetButtonDown("Menu")) {
            // Then either pause the game or return the Player back to the action.
            isGamePaused = !isGamePaused;
        }
    }

    public void GameOverMenuPrompt() {
        // Enable the Pause Menu Canvas to show that the game is paused.
        gameOverMenuCanvas.SetActive(true);
        // Disable all forms of movement from the Player while the game is paused.
        playerMovementController.enabled = false;
        armMovementController.enabled = false;
        cameraMovementController.enabled = false;

        // Stops other forms of movement made by GameObjects in that scene. 
        Time.timeScale = 0.0f;
    }

    public void RestartLevel() {
        // Access the current Scene the Player is in when this button is pressed.
        gameOverMenuCanvas.SetActive(false);
    }

    public void ReturnToTitleScreen() {
        // Access the Title Screen Scene when this button is pressed.
        SceneManager.LoadScene(backToTheTitleScreen);
    }
}
