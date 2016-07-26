using UnityEngine;
using System.Collections;

public class DestroyFinishedParticles : MonoBehaviour {
   
    private ParticleSystem thisParticleSystem;                  // Reference the Particle System attached to its Prefab.
	
	void Start () {
        // Obtain the components of this Prefab (and Particle System info).
        thisParticleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        // Checks to see if the Particle System is still running.
        if (thisParticleSystem.isPlaying) {
            // If so, allow the Particle System to continue going.
            return;
        }

        // If not, destroy the Particle System and remove it from the scene.
        Destroy(gameObject);
	}

    void OnBecameInvisible() {
        // Makes sure to destroy the particle effects if the Player keeps dying at the same spot.
        Destroy(gameObject);
    }
}
