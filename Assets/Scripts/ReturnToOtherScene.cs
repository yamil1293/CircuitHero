using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReturnToOtherScene : MonoBehaviour {

    // Allows the User to go back to a designated Scene.
    [SerializeField] string previousScreen = null;

    void Start() {
        // Starts the Returning To a Previous Screen procedure in case the Player wants to go back a scene.
        StartCoroutine("ReturningToAPreviousScreen");
    }

    IEnumerator ReturningToAPreviousScreen() {
        // If the Player wants to go back to a prior Menu Scene.
        while (!Input.GetButtonDown("Jump")) {
            // Unity will wait until the button is Pressed.
            yield return null;
        }

        // Grants access to the PreviousScreen class.
        PreviousScreen();
    }

    public void PreviousScreen() {
        // Access the Title Menu Scene when this button is pressed within the Main Menu.
        SceneManager.LoadScene(previousScreen);
    }
}
