using UnityEngine;
using System.Collections;

public class DestroyObjectsOverTime : MonoBehaviour {

    [SerializeField] float lifeTime;                // Assigns the amount of time a GameObject has before it is destroyed.	
	
	void Update () {
        // The amount assigned to lifeTime decreases at the rate made by Time.deltaTime.
        lifeTime -= Time.deltaTime;
        // If lifeTime falls bellow zero because of this...
        if (lifeTime < 0) {
            // ...then destroy the GameObject that is attached to this script.
            Destroy(gameObject);
        }
	}
}
