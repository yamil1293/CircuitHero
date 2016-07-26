using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
	public Rigidbody2D bullet;				// Prefab of the bullet.
	public float speed = 20f;				// The speed the bullet will fire at.


	public PlayerMotor2D playerControls;		// Reference to the PlayerMotor2D script.

	void Awake()
		{
			// Setting up the references.
				//playerCtrl = transform.root.GetComponent<MovementController>();
		}


	void Update ()
	{
		// If the player is aiming up...
		if(Input.GetKey (KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.X))
			{
				// ... instantiate the bullet facing up and set it's velocity upward. 
				Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(0, speed);
			}
		else if(Input.GetKey (KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
			{
				// Otherwise instantiate the bullet facing down and set it's velocity downward.
				Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(0, -speed);
			}


		// If the fire button is pressed in general...
		else if(Input.GetKeyDown(KeyCode.X))
			{
				// If the player is facing right...
				if(playerControls.facingLeft)
				{
					// ... instantiate the bullet facing left and set it's velocity to the left. 
					Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(-speed, 0);	

				}
				else
				{
					// Otherwise instantiate the bullet facing right and set it's velocity to the right.
					Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(speed, 0);
				}
			} 

			/*

			// If the fire button is fired diagonally upward
			else if(Input.GetKey (KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.C))
				{
					//If the player is firing diagonally to the right
					if(playerCtrl.facingRight)
						{
							if(Input.GetKey (KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.C))
							{
								// ... instantiate the bullet facing up, to the right and set it's velocity upward. 
								Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
								bulletInstance.velocity = new Vector2(speed, speed);
							}
						}
					else
					{
						// ... instantiate the bullet facing up, to the left and set it's velocity upward. 
						Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
						bulletInstance.velocity = new Vector2(-speed, speed);
					}
				}


			//If the fire button is fired diagonally downard
			else if(Input.GetKey (KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.C))
				{
					if(playerCtrl.facingRight)
						{
							if (Input.GetKey (KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.C))
							{
								// Otherwise instantiate the bullet facing down, to the right and set it's velocity downward.
								Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
								bulletInstance.velocity = new Vector2(speed, -speed);
							}
						}
					else
						{
							// Otherwise instantiate the bullet facing down, to the left and set it's velocity downward.
							Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
							bulletInstance.velocity = new Vector2(-speed, -speed);
						}
				}
				
			*/
	}
}
