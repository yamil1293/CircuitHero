using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MouseResponse : MonoBehaviour {
  
    void Start () {
        // Starts the No Outside Mouse Clicks procedure in case the Player doesn't click on a interactable GUI.
        StartCoroutine("NoOutsideMouseClicks");
    }

    IEnumerator NoOutsideMouseClicks() {
        // Constantly checks to see if the Player hasn't left clicked in the game.
        while (!Input.GetMouseButtonDown(0)) {
            // Then check to see if the firstSelectedGameObject is no longer highlighted and is currently being replaced.
            if (EventSystem.current.currentSelectedGameObject != EventSystem.current.firstSelectedGameObject
            && EventSystem.current.currentSelectedGameObject != null) {
                // If so, switch the firstSelectedGameObject to the one currently highlighted (currentSelectedGameObject).
                EventSystem.current.firstSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            }

            // Also check to see if the currentSelectedGameObject is interacting with another GameObject.
            if (EventSystem.current.currentSelectedGameObject == null) {              
                // If not, return to whatever gameObject selected in firstSelectedGameObject.
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
                // Provides a quick confirmation in the Unity Console.
                Debug.Log("Reselecting last input");
            }

            // Quickly pause to execute this IEnumerator again until one frame passes.
            yield return new WaitForEndOfFrame();
        }
    } 
}
