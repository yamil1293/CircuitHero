using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PressAnyButtonForTitleScreen : MonoBehaviour {

    [SerializeField] string titleScreen = null;    // Allows the User to continue to the main Title Scene.

    void Start() {
        // Starts the Going To a Main Title Screen procedure so the Player can start the game.
        StartCoroutine("GoingToMainTitleScreen");
    }

    IEnumerator GoingToMainTitleScreen() {
        // If the Player wants to officially start the game.
        while (!Input.GetButtonDown("Jump") && !Input.GetButtonDown("Shoot") && !Input.GetButtonDown("Dash")
            && !Input.GetButtonDown("Start") && !Input.GetButtonDown("Select")) {
            // Unity will wait until the button is Pressed.
            yield return null;
        }

        // Grants access to the the Main Title Screen class.
        TitleScreen();
    }

    public void TitleScreen() {
        // Access the Title Menu Scene when any button is pressed within the Main Menu.
        SceneManager.LoadScene(titleScreen);
    }
}
