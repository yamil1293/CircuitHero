using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;              // Array (list) of all the back- and foregrounds to be parallaxed
    public float[] parallaxScales;               // The proportion of the camera's movement to move the backgrounds
    public float smoothing = 1f;                 // How smooth the parallax is going to be. Make sure to set this above 0

    private Transform cameraTransform;           // Reference to the main cameras transform
    private Vector3 previousCameraPosition;      // The position of the camera in the previous frame

    // Is called before Start (). Great for references
    void Awake() {
        // Set up the camera reference
        cameraTransform = Camera.main.transform;
    }

    // Use this for initialization
    void Start() {
        // The previous frame had the current frame's camera position
        previousCameraPosition = cameraTransform.position;

        // Assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // For each background
        for (int i = 0; i < backgrounds.Length; i++) {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];

            // Set a traget x position which is the current position plus the parallax
            float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

            // Create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);
        }
        // Set the previousCamPos to the camera's position at the end of the frame
        previousCameraPosition = cameraTransform.position;
	}
}
