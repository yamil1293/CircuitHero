using UnityEngine;
using System.Collections;

public class BlasterCalculations : MonoBehaviour {

    // BlasterDamageList class is now accessible in other scripts and inspector.
    public BlasterDamageList blasterDamageList = new BlasterDamageList();
    // BlasterChargingList class is now accessible in other scripts and inspector.           
    public BlasterChargingList blasterChargingList = new BlasterChargingList();

    [Header("Current Blaster Damage")]
    [SerializeField] int standardDamage = 0;             // The standard damage dealt by all Blaster Shots.
    [SerializeField] int chargingDamage = 0;             // The charging damage dealt by all Blaster Shots.
    [SerializeField] int chargedDamage = 0;              // The charged damage dealt by all Blaster Shots.    
    [SerializeField] int secondDamage = 0;               // The damage dealt by the second charged Shot.

    [HideInInspector] public int blasterDamage = 0;      // The general blaster damage amount that is to be passed to a Blaster Shot.

    [Header("Current Charging Blaster")]
    public float chargingTimer = 0.0f;                   // Used as a timer/counter for the Player's charging.
    public float beginCharging = 0.0f;                   // Controls the time needed to start charging Blaster Shots.
    public float fullyChargedLimit = 0.0f;               // Controls the time needed to charge lvl2 Blaster Shots.
    public float secondChargedLimit = 0.0f;              // Controls the time needed to charge lvl3 Blaster Shots.

    // Holds all of the damage calculations for the 4 Blaster Modes.
    [System.Serializable] public class BlasterDamageList {
        [Header("Power Damage")]
        public int standardPowerDamage = 0;                 // The standard damage dealt by Power Shots.
        public int chargingPowerDamage = 0;                 // The charging damage dealt by Power Shots.   
        public int chargedPowerDamage = 0;                  // The charged damage dealt by Power Shots.

        [Header("Magnetic Damage")]
        public int standardMagntDamage = 0;                 // The standard damage dealt by Magnetic Shots.
        public int chargingMagntDamage = 0;                 // The charging damage dealt by Magnetic Shots.
        public int chargedMagntDamage = 0;                  // The charged damage dealt by Magnetic Shots.

        [Header("Thermal Damage")]
        public int standardThrmlDamage = 0;                 // The standard damage dealt by Thermal Shots.
        public int chargedThrmlDamage = 0;                  // The charged damage dealt by Thermal Shots.
        public int chargingThrmlDamage = 0;                 // The charging damage dealt by Thermal Shots.

        [Header("Diffusion Damage")]
        public int standardDffsnDamage = 0;                 // The standard damage dealt by Diffusion Shots.
        public int chargingDffsnDamage = 0;                 // The charging damage dealt by Diffusion Shots.
        public int chargedDffsnDamage = 0;                  // The charged damage dealt by Diffusion Shots.

        [Header("Spiral Damage")]
        public int secondChargedShotDamage = 0;             // The charged damage dealt by a Second Charged Shot.
    }

    // Holds all of the charging calculations for the 4 Blaster Modes.
    [System.Serializable] public class BlasterChargingList {
        [Header("Power Charging")]
        public float powrLvl1Charge = 0;                 // Controls the time needed to charge lvl1 Power Shots.
        public float powrLvl2Charge = 0;                 // Controls the time needed to charge lvl2 Power Shots.
        public float powrLvl3Charge = 0;                 // Controls the time needed to charge lvl3 Power Shots.

        [Header("Magnetic Charging")]
        public float magnLvl1Charge = 0;                 // Controls the time needed to charge lvl1 Magnetic Shots.
        public float magnLvl2Charge = 0;                 // Controls the time needed to charge lvl2 Magnetic Shots.
        public float magnLvl3Charge = 0;                 // Controls the time needed to charge lvl3 Magnetic Shots.

        [Header("Thermal Charging")]
        public float thrmLvl1Charge = 0;                 // Controls the time needed to charge lvl1 Thermal Shots.
        public float thrmLvl2Charge = 0;                 // Controls the time needed to charge lvl2 Thermal Shots.
        public float thrmLvl3Charge = 0;                 // Controls the time needed to charge lvl3 Thermal Shots.

        [Header("Diffusion Charging")]
        public float diffLvl1Charge = 0;                 // Controls the time needed to charge lvl1 Diffusion Shots.
        public float diffLvl2Charge = 0;                 // Controls the time needed to charge lvl2 Diffusion Shots.
        public float diffLvl3Charge = 0;                 // Controls the time needed to charge lvl3 Diffusion Shots.
    }

    BlasterManager blasterManager;                       // References the BlasterManager configurations.

    void Start() {
        // Obtains the components from the BlasterManager script.
        blasterManager = GetComponent<BlasterManager>();
    }

void Update() {
        // Checks to see if any of the 4 locks (power, magnetic, thermal and diffusion) are true.
        if (blasterManager.powerModeIsOn == true || blasterManager.magneticModeIsOn == true || blasterManager.thermalModeIsOn == true
            || blasterManager.diffusionModeIsOn == true) {
            // Then start the Current Blaster Damage procedure.
            StartCoroutine("CurrentBlasterDamageCoroutine");           
            // And the Blaster Damage List procedure.
            StartCoroutine("BlasterDamageListCoroutine");
            // And the Blaster Charging procedure.
            StartCoroutine("BlasterChargingCoroutine");
        }
    }

    IEnumerator CurrentBlasterDamageCoroutine() {
        // Second Charged shot will always have the same damage calculations for all Blaster Modes.
        if (blasterManager.isThisASecondChargedShot == true) {
            // Pass the Second Charged Shot damage amount to ShotProperties.
            blasterDamage = blasterDamageList.secondChargedShotDamage;
        }

        // Checks to see if Power Mode was currently selected by the Player.
        if (blasterManager.powerModeIsOn == true) {
            // Check if isThisAStandardShot is True.
            if (blasterManager.isThisAStandardShot == true) {
                // Pass the Standard Power Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.standardPowerDamage;
            }

            // Check if isThisAChargingShot is True.
            else if (blasterManager.isThisAChargingShot == true) {
                // Pass the Charging Power Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargingPowerDamage;
            }

            // Check if isThisAChargedShot is True.
            else if (blasterManager.isThisAChargedShot == true) {
                // Pass the Charged Power Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargedPowerDamage;
            }
            yield return null;
        }

        // Checks to see if Magnetic Mode was currently selected by the Player.
        else if (blasterManager.magneticModeIsOn == true) {
            // Check if isThisAStandardShot is True.
            if (blasterManager.isThisAStandardShot == true) {
                // Pass the Standard Magnetic Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.standardMagntDamage;
            }

            // Check if isThisAChargingShot is True.
            else if (blasterManager.isThisAChargingShot == true) {
                // Pass the Charging Magnetic Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargingMagntDamage;
            }

            // Check if isThisAChargedShot is True.
            else if (blasterManager.isThisAChargedShot == true) {
                // Pass the Charged Magnetic Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargedMagntDamage;
            }
            yield return null;
        }

        // Checks to see if Thermal Mode was currently selected by the Player.
        else if (blasterManager.thermalModeIsOn == true) {
            // Check if isThisAStandardShot is True.
            if (blasterManager.isThisAStandardShot == true) {
                // Pass the Standard Thermal Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.standardThrmlDamage;
            }

            // Check if isThisAChargingShot is True.
            else if (blasterManager.isThisAChargingShot == true) {
                // Pass the Charging Thermal Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargingThrmlDamage;
            }

            // Check if isThisAChargedShot is True.
            else if (blasterManager.isThisAChargedShot == true) {
                // Pass the Charged Thermal Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargedThrmlDamage;
            }
            yield return null;
        }

        // Checks to see if Diffusion Mode was currently selected by the Player.
        else if (blasterManager.diffusionModeIsOn == true) {
            // Check if isThisAStandardShot is True.
            if (blasterManager.isThisAStandardShot == true) {
                // Pass the Standard Diffusion Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.standardDffsnDamage;
            }

            // Check if isThisAChargingShot is True.
            else if (blasterManager.isThisAChargingShot == true) {
                // Pass the Charging Diffusion Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargingDffsnDamage;
            }

            // Check if isThisAChargedShot is True.
            else if (blasterManager.isThisAChargedShot == true) {
                // Pass the Charged Diffusion Shot damage amount to ShotProperties.
                blasterDamage = blasterDamageList.chargedDffsnDamage;
            }
            yield return null;
        }
    }

    IEnumerator BlasterDamageListCoroutine() {
        // Displays the Second Charged Shot damage amount.
        secondDamage = blasterDamageList.secondChargedShotDamage;

        // Checks to see if Power Mode was currently selected by the Player.
        if (blasterManager.powerModeIsOn == true) {
            // If so, ready all Power Mode damage calculations.
            standardDamage = blasterDamageList.standardPowerDamage;
            chargingDamage = blasterDamageList.chargingPowerDamage;
            chargedDamage = blasterDamageList.chargedPowerDamage;
            yield return null;
        }

        // Checks to see if Magnetic Mode was currently selected by the Player.
        else if (blasterManager.magneticModeIsOn == true) {
            // If so, ready all Magnetic Mode damage calculations.
            standardDamage = blasterDamageList.standardMagntDamage;
            chargingDamage = blasterDamageList.chargingMagntDamage;
            chargedDamage = blasterDamageList.chargedMagntDamage;
            yield return null;
        }

        // Checks to see if Thermal Mode was currently selected by the Player.
        else if (blasterManager.thermalModeIsOn == true) {
            // If so, ready all Thermal Mode damage calculations.
            standardDamage = blasterDamageList.standardThrmlDamage;
            chargingDamage = blasterDamageList.chargingThrmlDamage;
            chargedDamage = blasterDamageList.chargedThrmlDamage;
            yield return null;
        }

        // Checks to see if Diffusion Mode was currently selected by the Player.
        else if (blasterManager.diffusionModeIsOn == true) {
            // If so, ready all Diffusion Mode damage calculations.
            standardDamage = blasterDamageList.standardDffsnDamage;
            chargingDamage = blasterDamageList.chargingDffsnDamage;
            chargedDamage = blasterDamageList.chargedDffsnDamage;
            yield return null;
        }
    }

    IEnumerator BlasterChargingCoroutine() {
        // Checks to see if Power Mode was currently selected by the Player.
        if (blasterManager.powerModeIsOn == true) {
            // If so, ready all Power Mode charge calculations.          
            beginCharging = blasterChargingList.powrLvl1Charge;
            fullyChargedLimit =blasterChargingList.powrLvl2Charge;
            secondChargedLimit = blasterChargingList.powrLvl3Charge;
            yield return null;
        }

        // Checks to see if Magnetic Mode was currently selected by the Player.
        else if (blasterManager.magneticModeIsOn == true) {
            // If so, ready all Magnetic Mode charge calculations.
            beginCharging = blasterChargingList.magnLvl1Charge;
            fullyChargedLimit = blasterChargingList.magnLvl2Charge;
            secondChargedLimit = blasterChargingList.magnLvl3Charge;
            yield return null;
        }

        // Checks to see if Thermal Mode was currently selected by the Player.
        else if (blasterManager.thermalModeIsOn == true) {
            // If so, ready all Thermal Mode charge calculations.
            beginCharging = blasterChargingList.thrmLvl1Charge;
            fullyChargedLimit = blasterChargingList.thrmLvl2Charge;
            secondChargedLimit = blasterChargingList.thrmLvl3Charge;
            yield return null;
        }

        // Checks to see if Diffusion Mode was currently selected by the Player.
        else if (blasterManager.diffusionModeIsOn == true) {
            // If so, ready all Diffusion Mode charge calculations.
            beginCharging = blasterChargingList.diffLvl1Charge;
            fullyChargedLimit = blasterChargingList.diffLvl2Charge;
            secondChargedLimit = blasterChargingList.diffLvl3Charge;
            yield return null;
        }
    }
}

    // [SerializeField] specialProperty = Paralysis, Force, Pierce or NA
    // powrLevel1Special
    // powrLevel2Special
    // powrLevel3Special
    // shotSize = // May just be implemented easily using different prefab/sprite sizes.
    // [SerializeField] specialProperty = Paralysis, Force, Pierce or NA
    // magnLevel1Special
    // magnLevel2Special
    // magnLevel3Special
    // shotSize = // May just be implemented easily using different prefab/sprite sizes.
    // [SerializeField] specialProperty = Paralysis, Force, Pierce or NA
    // thrmLevel1Special
    // thrmLevel2Special
    // thrmLevel3Special
    // shotSize = // May just be implemented easily using different prefab/sprite sizes.
    // [SerializeField] specialProperty = Paralysis, Force, Pierce or NA
    // diffLevel1Special
    // diffLevel2Special
    // diffLevel3Special
    // shotSize = // May just be implemented easily using different prefab/sprite sizes.
