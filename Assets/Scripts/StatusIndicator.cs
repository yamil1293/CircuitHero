using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    // Used to obtain the GUI's text box and rectangle bar for the Status Indicator.
    [SerializeField] private RectTransform healthBarRectangle = null;
    [SerializeField] private Text healthText = null;

    void Start() {
        if (healthBarRectangle == null) {
            // Provides a visible alert in the Unity Editor's Console Log.
            Debug.LogError("Status Indicator: No health bar object referenced!");
        }

        if (healthText == null) {
            // Provides a visible alert in the Unity Editor's Console Log.
            Debug.LogError("Stauts Indicator: No health text object referenced!");
        }
    }
    
    public void SetHealth(int currentGameObjectHealth, int maxGameObjectHealth) {
        // Obtains the values provided in SetHealth and make them affect the healthBarRectangle and healthText.
        float value = (float)currentGameObjectHealth / maxGameObjectHealth; 

        // Resizes the HealthBarRectangle and alters the HealthText depending on changes made to the Player's health.
        healthBarRectangle.localScale = new Vector3(value, healthBarRectangle.localScale.y, healthBarRectangle.localScale.z);
        healthText.text = currentGameObjectHealth + "/" + maxGameObjectHealth + " HP";
    }
}
