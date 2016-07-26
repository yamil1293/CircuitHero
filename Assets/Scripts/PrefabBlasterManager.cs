using UnityEngine;
using System.Collections;

public class PrefabBlasterManager : MonoBehaviour {
  
    // BlasterPrefabList class is now accessible in other scripts and inspector.           
    public BlasterPrefabList blasterPrefabList = new BlasterPrefabList();
    // BlasterImpactList class is now accessible in other scripts and inspector.           
    public BlasterImpactList blasterImpactList = new BlasterImpactList();

    [Header("Current Blaster Prefabs")]
    public GameObject standardShot = null;                       // Holds the standard shot that is to be fired by the Player.
    public GameObject chargingShot = null;                       // Holds the charging shot that is to be fired by the Player.    
    public GameObject chargedShot = null;                        // Holds the charged shot that is to be fired by the Player.    
    public GameObject secondChargedShot = null;                         // Holds the second charged shot that is to be fired by the Player.

    [Header("Current Impact Prefabs")]
    public GameObject impactEffect = null;                       // Holds the impactEffect that is created by its shot collision.

    // Holds all of the prefabs for the 4 Blaster Modes.
    [System.Serializable] public class BlasterPrefabList {
        [Header("Power Mode Blaster Prefabs")]
        public GameObject standardPowerPrefab = null;             // Holds the Power Mode standard shot in the inspector.
        public GameObject chargingPowerPrefab = null;             // Holds the Power Mode charging shot in the inspector.
        public GameObject chargedPowerPrefab = null;              // Holds the Power Mode charged shot in the inspector.

        [Header("Magnetic Mode Blaster Prefabs")]
        public GameObject standardMagntPrefab = null;             // Holds the Magnetic Mode standard shot in the inspector.
        public GameObject chargingMagntPrefab = null;             // Holds the Magnetic Mode charging shot in the inspector.
        public GameObject chargedMagntPrefab = null;              // Holds the Magnetic Mode charged shot in the inspector.

        [Header("Thermal Mode Blaster Prefabs")]
        public GameObject standardThrmlPrefab = null;             // Holds the Thermal Mode standard shot in the inspector.
        public GameObject chargingThrmlPrefab = null;             // Holds the Thermal Mode charging shot in the inspector.
        public GameObject chargedThrmlPrefab = null;              // Holds the Thermal Mode charged shot in the inspector.

        [Header("Diffusion Mode Blaster Prefabs")]
        public GameObject standardDffsnPrefab = null;             // Holds the Diffusion Mode standard shot in the inspector.
        public GameObject chargingDffsnPrefab = null;             // Holds the Diffusion Mode charging shot in the inspector.
        public GameObject chargedDffsnPrefab = null;              // Holds the Diffusion Mode charged shot in the inspector.

        [Header("All Mode Blaster Prefabs")]
        public GameObject secondChargedShotPrefab = null;         // Holds the Second Charged shot for all Modes in the inspector. 
    }

    // Holds all of the impacts for the 4 Blaster Modes.
    [System.Serializable] public class BlasterImpactList {
        public GameObject powerImpactEffect = null;               // Holds the Power Mode particle used for their collisions.
        public GameObject magneticImpactEffect = null;            // Holds the Magnetic Mode particle used for their collisions.
        public GameObject thermalImpactEffect = null;             // Holds the Thermal Mode particle used for their collisions.
        public GameObject diffusionImpactEffect = null;           // Holds the Diffusion Mode particle used for their collisions.
    }

    BlasterManager blasterManager;                                // References the BlasterManager configurations.

    void Start () {
        // Obtains the components from the BlasterManager script.
        blasterManager = GetComponent<BlasterManager>();
    }

    void Update () {
        // Checks to see if any of the 4 locks (power, magnetic, thermal and diffusion) are true.
        if (blasterManager.powerModeIsOn == true || blasterManager.magneticModeIsOn == true || blasterManager.thermalModeIsOn == true 
            || blasterManager.diffusionModeIsOn == true) {
            // If so, start the Blaster Prefab procedure.
            StartCoroutine("BlasterPrefabCoroutine");
        }        
    }

     public IEnumerator BlasterPrefabCoroutine() {
        // The second charged shot will always have the same prefab for all Blaster Modes.
        secondChargedShot = blasterPrefabList.secondChargedShotPrefab;

        // Checks to see if Power Mode was currently selected by the Player.
        if (blasterManager.powerModeIsOn == true) {
            // If so, ready all Power Mode shot prefabs to be instantiated.
            standardShot = blasterPrefabList.standardPowerPrefab;
            chargingShot = blasterPrefabList.chargingPowerPrefab;
            chargedShot = blasterPrefabList.chargedPowerPrefab;
            impactEffect = blasterImpactList.powerImpactEffect;
            yield return null;
        }

        // Checks to see if Magnetic Mode was currently selected by the Player.
        else if (blasterManager.magneticModeIsOn == true) {
            // If so, ready all Magnetic Mode shot prefabs to be instantiated.
            standardShot = blasterPrefabList.standardMagntPrefab;
            chargingShot = blasterPrefabList.chargingMagntPrefab;
            chargedShot = blasterPrefabList.chargedMagntPrefab;
            impactEffect = blasterImpactList.magneticImpactEffect;
            yield return null;
        }

        // Checks to see if Thermal Mode was currently selected by the Player.
        else if (blasterManager.thermalModeIsOn == true) {
            // If so, ready all Thermal Mode shot prefabs to be instantiated.
            standardShot = blasterPrefabList.standardThrmlPrefab;
            chargingShot = blasterPrefabList.chargingThrmlPrefab;
            chargedShot = blasterPrefabList.chargedThrmlPrefab;
            impactEffect = blasterImpactList.thermalImpactEffect;
            yield return null;
        }

        // Checks to see if Diffusion Mode was currently selected by the Player.
        else if (blasterManager.diffusionModeIsOn == true) {
            // If so, ready all Diffusion Mode shot prefabs to be instantiated.
            standardShot = blasterPrefabList.standardDffsnPrefab;
            chargingShot = blasterPrefabList.chargingDffsnPrefab;
            chargedShot = blasterPrefabList.chargedDffsnPrefab;
            impactEffect = blasterImpactList.diffusionImpactEffect;
            yield return null;
        }    
    }
}
