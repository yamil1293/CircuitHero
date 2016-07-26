using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(MouseResponse))]
public class TitleMenu : MonoBehaviour {

    [SerializeField] string mainGame = null;           // Allows the User to type in the Main Game Scene that will be used for this button. 
    [SerializeField] string debugMenu = null;          // Allows the User to type in the Debug Menu Scene that will be used for this button. 
    [SerializeField] string extraMode = null;          // Allows the User to type in the Extra Mode Scene that will be used for this button. 
    [SerializeField] string systemOptions = null;      // Allows the User to type in the System Options Scene that will be used for this button. 

    public void MainGame() {
        // Access the Main Game Scene when this button is pressed within the Main Menu.
        SceneManager.LoadScene(mainGame);
    }

    public void DebugMenu() {
        // Access the Debug Menu Scene when this button is pressed within the Main Menu.
        SceneManager.LoadScene(debugMenu);
    }

    public void ExtraMode() {
        // Access the Extra Mode Scene when this button is pressed within the Main Menu.
        SceneManager.LoadScene(extraMode);
    }

    public void SystemOptions() {
        // Access the System Options Scene when this button is pressed within the Main Menu.
        SceneManager.LoadScene(systemOptions);
    }

    public void ExitGame() {
        // Turns off the game when this button is pressed within the Main Menu Scene.
        Application.Quit();
        // Checks to see if the Player has ended their game within Unity's Inspector.
        Debug.Log("Player has ended their game.");
    }
}
