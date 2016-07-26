using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {

    public int offsetX = 2;                 // The offset so that we don't get any wierd errors

    // Used for checking if we need to instantiate stuff
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;       // Used if the object is not tilable

    private float spriteWidth = 0f;         // The width of our element
    private Camera cameraVariable;
    private Transform myTransform;

    void Awake() {
        cameraVariable = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
	void Start () {
        SpriteRenderer spriteRendererVariable = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRendererVariable.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        // Does it still need buddies? If not do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false) {
            // Calculate the cameras extend (half the width) of what the camera can see in world coordinates
            float cameraHorizonralExtend = cameraVariable.orthographicSize * Screen.width / Screen.height;

            // Calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - cameraHorizonralExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + cameraHorizonralExtend;

            // Checking if we can see the edge of the element and then calling MakeNewBuddy if we can
            if (cameraVariable.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }

            else if (cameraVariable.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    // A function that creates a buddy on the side required
    void MakeNewBuddy(int rightOrLeft) {
        // Calculating the new position for our new buddy
        Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // Instantating our new body and storing him in a variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // If not tilable let's reverse the x size of our object to get rid of ugly seams
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0) {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }

        else {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }

    // A Function that gets rid of GameObjects that are away from the Main Camera.
    void OnBecameInvisible() {
        Destroy(gameObject);
    }       
}
