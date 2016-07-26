using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(MouseResponse))]
public class SystemOptions : MonoBehaviour {

    [SerializeField] Slider[] volumeSliders;                // An array that holds the Master, Music and SFX Volume sliders.
    [SerializeField] Toggle[] resolutionToggles;            // An array that holds the three unique resolution toggles.
    [SerializeField] int[] screenWidths;                    // An array that holds the width of the three unique resolution toggles.
    [HideInInspector] int activeScreenResolutionIndex;      // This variable will hold any resolution selected by the Player.

    public void SetScreenResolution(int i) {
        // Checks to see if one of the three resolution toggles has been selected.
        if (resolutionToggles[i].isOn) {
            // This will take the current resolution selected by the Player.
            activeScreenResolutionIndex = i;
            // If so, provide the aspect ration formula here to be later used for the resolution's height.
            float aspectRatio = 16 / 9f;
            // Calculate what is the  set resolution for the Player by using the provided width and aspect ratio
            // to find the resolution's height. Also check to see if its already fullscreen.
            Screen.SetResolution(screenWidths[i], (int) (screenWidths[i] / aspectRatio), false);
        }
    }

    public void SetFullscreen(bool isFullScreen) {
        // This loops through all of the available toggles.
        for (int i = 0; i < resolutionToggles.Length; i++) {
            // If it is fullscreen, then every resolution is noninteractable.
            resolutionToggles[i].interactable = !isFullScreen;
        }
        Debug.Log(isFullScreen);

        // Check to see if the screen is already in fullScreen.
        if (isFullScreen) {
            // This will make an array of all the supported resolutions from the monitor.
            Resolution[] allResolutions = Screen.resolutions;
            // This will then take the maximum resolution that is possible from this array.
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            // Then use the maximum native resolution possible as the set resolution.
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }

        else {
            // If it isn't fullscreen, just take the current resolution selected by the Player.
            SetScreenResolution(activeScreenResolutionIndex);
        }
    }

    public void SetMasterVolume(float value) {
        
    }

    public void SetMusicVolume(float value) {

    }

    public void SetSFXVolume(float value) {

    }
}
