using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    [SerializeField] string levelToLoad = null;

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            SceneManager.LoadScene(levelToLoad);
        }
    }       
}
