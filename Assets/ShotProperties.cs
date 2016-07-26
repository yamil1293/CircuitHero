using UnityEngine;
using System.Collections;

public class ShotProperties : MonoBehaviour {

    [SerializeField] int shotBlasterDamage = 0;                           // Contains the numeric damage amount this blaster shot deals to enemies.
    [SerializeField] GameObject shotImpactEffect;                         // Contains the impactEffect that is created by its shot collision.

    BlasterCalculations blasterCalculations;                              // References the BlasterCalculations configurations.
    PrefabBlasterManager prefabBlasterManager;                            // References the PrefabBlasterManager configurations.
    
    void Start () {
        // Obtains the components from the PrefabBlasterManager script.
        prefabBlasterManager = GetComponent<PrefabBlasterManager>();
        // Obtains the components from the BlasterCalculations script.
        blasterCalculations = GetComponent<BlasterCalculations>();
 
        // Obtains the components and scripts from the Arm GameObject on the Player.
        GameObject armBlasterContainer = GameObject.Find("Arm");
        
        // Has access to all of the public variables located within PrefabBlasterManager.
        prefabBlasterManager = armBlasterContainer.GetComponent<PrefabBlasterManager>();
        // Passes the impactEffect GameObject from the PrefabBlasterManager to the shotImpactEffect here.
        shotImpactEffect = prefabBlasterManager.impactEffect;

        // Has access to all of the public variables located within BlasterCalculations.
        blasterCalculations = armBlasterContainer.GetComponent<BlasterCalculations>();
        // Passes the numeric damage amount from BlasterCalculations to the shotBlasterDamage here.
        shotBlasterDamage = blasterCalculations.blasterDamage;
        Debug.Log(blasterCalculations.blasterDamage);
	}

    void OnTriggerEnter2D(Collider2D other) {
        // Checks for the Enemy GameObject's tag.
        if (other.tag == "Enemy") {
            // Pass the damage amount to the EnemyStatus script.
            other.GetComponent<EnemyStatus>().AssigningDamage(shotBlasterDamage);
        }

        // Checks for other GameObjects' tags in case alternate actions need to be taken.
        if (other.tag == "Environmental" && other.tag == "Through") {
            // If the shot collides with the above tags, destroy the shot's GameObject. 
            Destroy(gameObject);
        }

        // If the shot Prefab collides with anything else that triggers it... 
        Instantiate(shotImpactEffect, transform.position, transform.rotation);
        //Destroy the shot's GameObject and instantiate the impact effects.
        Destroy(gameObject);
    }
}
