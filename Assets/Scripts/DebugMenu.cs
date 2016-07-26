using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(ReturnToOtherScene))]
[RequireComponent(typeof(MouseResponse))]
public class DebugMenu : MonoBehaviour {

    // Allows the User to go back to the Title Menu.
    [SerializeField] string backToTitleScreen = null;

    // Allows the User to type in the Stages to be accessed by this button. 
    [SerializeField] string stage1 = null;           
    [SerializeField] string stage2 = null;          
    [SerializeField] string stage3 = null;          
    [SerializeField] string stage4 = null;
    [SerializeField] string stage5 = null;
    [SerializeField] string stage6 = null;
    [SerializeField] string stage7 = null;
    [SerializeField] string stage8 = null;
    [SerializeField] string stage9 = null;
    [SerializeField] string stage10 = null;
    [SerializeField] string stage11 = null;
    [SerializeField] string stage12 = null;
    [SerializeField] string stage13 = null;
    [SerializeField] string stage14 = null;
    [SerializeField] string stage15 = null;
    [SerializeField] string stage16 = null;
    [SerializeField] string stage17 = null;
    [SerializeField] string stage18 = null;
    [SerializeField] string stage19 = null;
    [SerializeField] string stage20 = null;
    
    public void BackToTitleScreen() {
        // Access the Title Menu Scene when this button is pressed within the Main Menu.
        SceneManager.LoadScene(backToTitleScreen);
    }

    public void Stage1() {
        // Loads Stage 1.
        SceneManager.LoadScene(stage1);
    }

    public void Stage2() {
        // Loads Stage 2.
        SceneManager.LoadScene(stage2);
    }

    public void Stage3() {
        // Loads Stage 3.
        SceneManager.LoadScene(stage3);
    }

    public void Stage4() {
        // Loads Stage 4.
        SceneManager.LoadScene(stage4);
    }

    public void Stage5() {
        // Loads Stage 5.
        SceneManager.LoadScene(stage5);
    }

    public void Stage6() {
        // Loads Stage 6.
        SceneManager.LoadScene(stage6);
    }

    public void Stage7() {
        // Loads Stage 7.
        SceneManager.LoadScene(stage7);
    }

    public void Stage8() {
        // Loads Stage 8.
        SceneManager.LoadScene(stage8);
    }

    public void Stage9() {
        // Loads Stage 9.
        SceneManager.LoadScene(stage9);
    }

    public void Stage10() {
        // Loads Stage 10.
        SceneManager.LoadScene(stage10);
    }

    public void Stage11() {
        // Loads Stage 11.
        SceneManager.LoadScene(stage11);
    }

    public void Stage12() {
        // Loads Stage 12.
        SceneManager.LoadScene(stage12);
    }

    public void Stage13() {
        // Loads Stage 13.
        SceneManager.LoadScene(stage13);
    }

    public void Stage14() {
        // Loads Stage 14.
        SceneManager.LoadScene(stage14);
    }

    public void Stage15() {
        // Loads Stage 15.
        SceneManager.LoadScene(stage15);
    }

    public void Stage16() {
        // Loads Stage 16.
        SceneManager.LoadScene(stage16);
    }

    public void Stage17() {
        // Loads Stage 17.
        SceneManager.LoadScene(stage17);
    }

    public void Stage18() {
        // Loads Stage 18.
        SceneManager.LoadScene(stage18);
    }

    public void Stage19() {
        // Loads Stage 19.
        SceneManager.LoadScene(stage19);
    }

    public void Stage20() {
        // Loads Stage 20.
        SceneManager.LoadScene(stage20);
    }
}
