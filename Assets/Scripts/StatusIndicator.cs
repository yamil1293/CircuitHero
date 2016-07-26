using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusIndicator : MonoBehaviour {

    [SerializeField] RectTransform healthBarRectangle = null;       // References the attached healthBarRectangle's GameObject on the StatusIndicator.
    [SerializeField] Text healthText = null;                        // References the attached healthText's GameObject on the StatusIndicator.
    [SerializeField] Image modeIcon = null;                         // References the attached modeIcon's GameObject on the StatusIndicator.

    BlasterManager blasterManager;                                  // References the BlasterManager configurations.

    bool blueBlasterMode;                                           // Used to hold the Power Mode boolean value in the Blaster Manager Script.
    bool yellowBlasterMode;                                         // Used to hold the Magnetic Mode boolean value in the Blaster Manager Script.
    bool redBlasterMode;                                            // Used to hold the Thermal Mode boolean value in the Blaster Manager Script.
    bool greenBlasterMode;                                          // Used to hold the Diffusion Mode boolean value in the Blaster Manager Script.

    void Start() {
        // First finds the modeIcon GameObject and then obtains the Image components.
        modeIcon = GameObject.Find("ModeIcon").GetComponent<Image>();
        // Obtains the components and scripts from the Arm GameObject on the Player.
        GameObject armBlasterContainer = GameObject.Find("Arm");
        // Has access to all of the public variables located within PrefabBlasterManager.
        blasterManager = armBlasterContainer.GetComponent<BlasterManager>();

        if (healthBarRectangle == null) {
            // Provides a visible alert in the Unity Editor's Console Log.
            Debug.LogError("Status Indicator: No health bar object referenced!");
        }

        if (healthText == null) {
            // Provides a visible alert in the Unity Editor's Console Log.
            Debug.LogError("Stauts Indicator: No health text object referenced!");
        }

        if (modeIcon == null) { 
            // Provides a visible alert in the Unity Editor's Console Log.
            Debug.LogError("Stauts Indicator: No mode icon object referenced!");
        }                        
    }

    void Update() {
        // Takes all of the boolean values from each of the Blaster Modes.
        blueBlasterMode = blasterManager.powerModeIsOn;
        yellowBlasterMode = blasterManager.magneticModeIsOn;
        redBlasterMode = blasterManager.thermalModeIsOn;
        greenBlasterMode = blasterManager.diffusionModeIsOn;
        
        // If the Player does press a Blaster Mode button, start the Blaster Mode Coloring procedure.
        StartCoroutine("BlasterModeColoring");
    }

    public void SetHealth(int currentGameObjectHealth, int maxGameObjectHealth) {
        // Obtains the values provided in SetHealth and make them affect the healthBarRectangle and healthText.
        float value = (float)currentGameObjectHealth / maxGameObjectHealth; 

        // Resizes the HealthBarRectangle and alters the HealthText depending on changes made to the Player's health.
        healthBarRectangle.localScale = new Vector3(value, healthBarRectangle.localScale.y, healthBarRectangle.localScale.z);
        healthText.text = currentGameObjectHealth + "/" + maxGameObjectHealth + " HP";
    }

    IEnumerator BlasterModeColoring() {
        // Checks to see if Power Mode was currently selected by the Player.
        if (blueBlasterMode == true) {
            modeIcon.color = Color.blue;
            yield return null;
        }

        // Checks to see if Magnetic Mode was currently selected by the Player.
        if (yellowBlasterMode == true) {
            modeIcon.color = Color.yellow;
            yield return null;
        }

        // Checks to see if Thermal Mode was currently selected by the Player.
        if (redBlasterMode == true) {
            modeIcon.color = Color.red;
            yield return null;
        }

        // Checks to see if Diffusion Mode was currently selected by the Player.
        if (greenBlasterMode == true) {
            modeIcon.color = Color.green;
            yield return null;
        }
        // Color newColor = modeIconImage.color;
        // newColor.a = 1;
        // modeIconImage.color = newColor;
    }
}
