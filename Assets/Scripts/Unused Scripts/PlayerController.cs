using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveForce;
	public float dashForce;
	public float jumpForce;
	public float maxSpeed;
	public float strikeForce;

	public Transform groundRaycastOrigin;
	public Transform wallRaycastOrigin;
		
	private Animator animationController;
	
	private Rigidbody2D rigidBody2dBody;
	
	private SpriteRenderer spRend;
	
	private BoxCollider2D boxCollider;

	private Vector3 startPosition;

	private float originalMaxSpeed;
	private float gravity;

	private bool facingRight = true;
	private bool jump = false;
	private bool doubleJump = false;
	private bool dash = false;
    private bool dashJump = false;
	private bool airDash = false;
	private bool wallJump = false;
	private bool strikeDown = false;
	private bool getDown = false;
	private bool getUp = false;
	private bool up = false;
	private bool down = false;
	private bool running = false;
	private bool jumping = false;
	private bool noRight = false;
	private bool noLeft = false;
	private bool grounded = false;
	private bool onGrip = false;
	private bool onWall = false;
	private bool onCeil = false;
	private bool jumped = false;
	private bool gripJump = false;
	private bool doubleJumped = true;
	private bool wallSliding = false;
	private bool gripped = false;
	private bool airDashed = false;
	
	void Awake () 
	{
		animationController = GetComponent<Animator>();
		rigidBody2dBody = GetComponent<Rigidbody2D>();
		spRend = GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		originalMaxSpeed = maxSpeed;
		gravity = rigidBody2dBody.gravityScale;
		startPosition = rigidBody2dBody.position;
	}

	void Update () 
	{

		onCeil = (Physics2D.Raycast(groundRaycastOrigin.position, new Vector2(0,1), 1.25f, 1 << LayerMask.NameToLayer("Ground")));
		grounded = Physics2D.Raycast(transform.position, new Vector2(0,-1), 2, 1 << LayerMask.NameToLayer("Ground"));
		onWall = ((Physics2D.Raycast(wallRaycastOrigin.position, new Vector2(1,0), 0.5f, 1 << LayerMask.NameToLayer("Wall"))||Physics2D.Raycast(transform.position, new Vector2(-1,0), 0.5f, 1 << LayerMask.NameToLayer("Wall"))) && !onGrip);
		onGrip = ((Physics2D.Raycast(groundRaycastOrigin.position, new Vector2(1,0), 0.5f, 1 << LayerMask.NameToLayer("Ground"))||Physics2D.Raycast(groundRaycastOrigin.position, new Vector2(-1,0), 0.5f, 1 << LayerMask.NameToLayer("Ground"))) && !grounded && !onCeil );

		if (Input.GetKey ("s") && grounded && !down && !dash) {
			getDown = true;
		}

		if (!Input.GetKey ("s") && down) {
			getUp = true;
		}
		if (!down && !onGrip && !gripJump) {
			rigidBody2dBody.gravityScale = gravity;
			gripped = false;
			if (Input.GetButtonDown ("Jump") && grounded && !(onWall && (Input.GetKey("d") || Input.GetKey("q")))) {
				jump = true;
			}

			if (Input.GetKeyDown ("left shift") && grounded && !onWall) {
				dash = true;
			}

			if (Input.GetKeyDown ("left shift") && !grounded && !wallJump && !onWall && !airDashed) {
				airDash = true;
			}

			if (dash && Input.GetButtonDown ("Jump") &&!onWall) {
				dashJump = true;
			}

			if (Input.GetButtonDown ("Jump") && !grounded && !strikeDown) {
				if (onWall && facingRight && Input.GetKey ("d")) {
					doubleJumped = false;
					noLeft = false;
					noRight = true;
					StartCoroutine(WallJump(-1));
				} else if (onWall && !facingRight && Input.GetKey ("q")) {
					doubleJumped = false;
					noRight = false;
					noLeft = true;
					StartCoroutine(WallJump(1));
				} else{
					doubleJump = true;
				}
			}
		}

		if (Input.GetKeyDown ("s") && !wallSliding && !grounded && !strikeDown) {
			dash = false;
			airDash = false;
			strikeDown = true;
			StrikeDown();
		}
		if (!onWall && !grounded && !strikeDown) {
			animationController.SetBool ("Idle",false);
			animationController.SetBool ("Run",false);
			animationController.SetBool ("Wall",false);
			animationController.SetBool ("Fall",true);
		}

		if (onWall && !grounded && !strikeDown) {
			if(facingRight && Input.GetKey("d")){
				doubleJumped = true;
				WallSlide();
			}
			else if(!facingRight && Input.GetKey ("q")){
				doubleJumped = true;
				WallSlide();
			}
		}
		if (grounded) {
			animationController.SetBool("Fall",false);
			animationController.SetBool("Wall", false);
			animationController.SetBool("DoubleJump",false);
			if(jump == false){
				animationController.SetBool("Jump",false);
				if(running){
					Run();
				}
			}
			strikeDown = false;
			jumping = false;
			doubleJumped = false;
			dashJump = false;
			maxSpeed = originalMaxSpeed;
			airDash = false;
			noRight = false;
			noLeft = false;
			wallJump = false;
			wallSliding = false;
			gripJump = false;
			airDashed = false;
		}

		if (!onWall && wallSliding) {
			wallSliding = false;
		}

		if (onWall) {
			animationController.SetBool("Dash",false);
			maxSpeed = originalMaxSpeed;
			animationController.SetBool("Fall",false);
			if(dash || airDash){
				airDash = false;
				dash = false;
			}
			if(wallJump){
				StartCoroutine(ResetLeftAndRight());
				wallJump = false;
			}
		}

		if (onGrip && !strikeDown && !grounded && !gripJump) {
			Grip();
		}

		if (getDown && !down) {
			GetDown();
		}

		if (getUp && !up) {
			GetUp();
		}

	}
	
	void FixedUpdate()
	{
		float h = 0;
		if (!strikeDown && !down) {
			if(!onGrip && !gripJump){

				if (Input.GetKey ("d") && !noRight && !(Input.GetKey ("q") && !noLeft)) {
					h = 1;
				} else if (Input.GetKey ("q") && !noLeft && !(Input.GetKey ("d") && !noRight)) {
					h = -1;
				}

				if (h != 0 && !running && grounded) {
					Run ();
				} else if (h == 0 && !jumping && grounded) {
					Idle ();
				}

				if (h * rigidBody2dBody.velocity.x < maxSpeed){
					rigidBody2dBody.AddForce (new Vector2(h * moveForce,0.0f));
				}
			
				if (Mathf.Abs (rigidBody2dBody.velocity.x) > maxSpeed){
					rigidBody2dBody.velocity = new Vector2 (Mathf.Sign (rigidBody2dBody.velocity.x) * maxSpeed, rigidBody2dBody.velocity.y);
				}

				if (h > 0 && !facingRight)
					Flip ();
				else if (h < 0 && facingRight)
					Flip ();

				if (dash || (airDash && !airDashed)) {
					StartCoroutine (Dash ());
				}

				if (jump) {
					Jump ();
				}

				if (doubleJump) {
					DoubleJump ();
				}
			}
			else{
				if(Input.GetButtonDown("Jump") && !gripJump){
					StartCoroutine(GripJump());
				}
			}
		}
	}

	IEnumerator GripJump(){
		gripJump = true;
		rigidBody2dBody.gravityScale = gravity;
		animationController.SetBool("Grip",false);
		animationController.SetBool("Run",false);
		animationController.SetBool("Idle",false);
		animationController.SetBool("Jump",true);
		rigidBody2dBody.AddForce (new Vector2 (0f, jumpForce/1.8f));
		yield return new WaitForSeconds (0.25f);
		if (facingRight) {
			rigidBody2dBody.AddForce (new Vector2 (200, 0f));
		} else {
			rigidBody2dBody.AddForce (new Vector2 (-200, 0f));
		}
		onGrip = false;
	}

	void GetDown(){
		down = true;
		dash = false;
		boxCollider.enabled = false;
		rigidBody2dBody.velocity = new Vector2 (0, rigidBody2dBody.velocity.y);
		animationController.SetBool("Dash",false);
		animationController.SetBool ("Jump", false);
		animationController.SetBool ("Idle", false);
		animationController.SetBool ("Run", false);
		animationController.SetBool ("Strike", false);
		animationController.SetBool ("Wall", false);
		animationController.SetBool ("Kneel", true);
		up = false;
		getDown = false;
	}

	void GetUp(){
		boxCollider.enabled = true;
		animationController.SetBool ("Kneel", false);
		animationController.SetBool ("Idle", true);
		down = false;
		up = true;
		getUp = false;
		spRend.enabled = true;
	}

	void StrikeDown(){
		animationController.SetBool("Grip",false);
		animationController.SetBool("Dash",false);
		animationController.SetBool ("Fall", false);
		animationController.SetBool ("Idle", false);
		animationController.SetBool ("Jump", false);
		animationController.SetBool ("DoubleJump", false);
		animationController.SetBool ("Strike", true);
		rigidBody2dBody.velocity = new Vector2 (0.0f, 0.0f);
		rigidBody2dBody.AddForce(new Vector2(0.0f, -strikeForce));
	}

	void Grip(){
		animationController.SetBool("Strike",false);
		animationController.SetBool("Jump",false);
		animationController.SetBool("DoubleJump",false);
		animationController.SetBool("Kneel",false);
		animationController.SetBool("Fall",false);
		animationController.SetBool("Run",false);
		animationController.SetBool("Idle",false);
		animationController.SetBool("Dash",false);
		animationController.SetBool("Grip",true);
		if (!gripped) {
			rigidBody2dBody.velocity = new Vector2 (0, 0);
			rigidBody2dBody.gravityScale = 0;
		}
		gripped = true;

	}

	void WallSlide (){
		animationController.SetBool("Dash",false);
		animationController.SetBool ("Idle", false);
		animationController.SetBool ("Jump", false);
		animationController.SetBool ("DoubleJump", false);
		animationController.SetBool ("Run", false);
		animationController.SetBool ("Strike", false);
		animationController.SetBool ("Wall", true);
		rigidBody2dBody.velocity = new Vector2 (0, -2);
		wallSliding = true;
	}

	void DoubleJump(){
		if (!doubleJumped) {
			animationController.SetBool("Fall",false);
			animationController.SetBool ("Idle", false);
			animationController.SetBool("Jump",false);
			animationController.SetBool("DoubleJump",true);
			rigidBody2dBody.AddForce (new Vector2 (0f, (jumpForce / 2f)-50*rigidBody2dBody.velocity.y));
			doubleJumped = true;
		} else {
			animationController.SetBool("DoubleJump",false);
			animationController.SetBool("Jump",true);
			doubleJump = false;
		}
	}

	IEnumerator WallJump(int direction){
		animationController.SetBool ("Wall", false);
		animationController.SetBool ("Jump", true);
		Flip ();
		rigidBody2dBody.AddForce (new Vector2 (jumpForce*direction, (jumpForce / 2f) - 50 * rigidBody2dBody.velocity.y));
		yield return new WaitForSeconds (0.01f);
		wallJump = true;
	}

	void Run(){
		if (!down) {
			animationController.SetBool("Dash",false);
			animationController.SetBool("Strike",false);
			animationController.SetBool ("Idle", false);
			animationController.SetBool ("Run", true);
			running = true;
		}
	}

	void Idle(){
		animationController.SetBool("Dash",false);
		animationController.SetBool("Strike",false);
		animationController.SetBool("Run", false);
		animationController.SetBool("Idle", true);
		running = false;
	}

	void Jump(){
		if (!jumped) {
			jumping = true;
			animationController.SetBool("Run",false);
			animationController.SetBool("Idle",false);
			if(!animationController.GetBool("Dash")){
				animationController.SetBool("Jump",true);
			}
			rigidBody2dBody.AddForce (new Vector2 (0f, jumpForce / 2));
			jumped = true;
		} else {
			jump = false;
			jumped = false;
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	IEnumerator Dash(){
		animationController.SetBool("Strike",false);
		animationController.SetBool("Jump",false);
		animationController.SetBool("DoubleJump",false);
		animationController.SetBool("Kneel",false);
		animationController.SetBool("Fall",false);
		animationController.SetBool("Run",false);
		animationController.SetBool("Idle",false);
		animationController.SetBool("Dash",true);
		maxSpeed = originalMaxSpeed * 2;
		if(airDash){
			airDashed = true;
		}
		if (facingRight) {
			rigidBody2dBody.AddForce (new Vector2 (dashForce, 0f));
			noLeft = true;
		} else {
			rigidBody2dBody.AddForce (new Vector2 (-dashForce, 0f));
			noRight = true;
		}
		yield return new WaitForSeconds (0.1f);
		if (!grounded && !airDash) {
			airDash = true;
		}
		dash = false;
	}

	IEnumerator ResetLeftAndRight(){
		doubleJumped = true;
		yield return new WaitForSeconds (0.25f);
		if (!wallJump) {
			noLeft = false;
			noRight = false;
		}
	}

	public void OnTriggerExit2D(Collider2D other){

		if (other.tag.Equals ("Bounds")) {
			rigidBody2dBody.velocity = new Vector2();
			rigidBody2dBody.position = startPosition;
		}
	}
}

