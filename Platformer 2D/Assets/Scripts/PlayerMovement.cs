using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D rb;
	private float x;
	private RaycastHit2D GroundCheck;
	public float ColliderDistance = 0.55f;
	public float ColliderDistance2 = 0.65f;
	private float targetX;
	
	//Jump variables
	public int jumpCount = 0;
	public int jumpMax = 1;
	public float jumpHeight = 3.0f;
	public float jumpSpeed = 10.0f;
	public float jumpCutoff = 1.0f;
	public float slowFallSpeed = 1.0f;
	public float FallSpeed = 3.0f;
	public float fastFallSpeed = 5.0f;
	public float currentY = 0.0f;
	public float maxFallSpeed = 10f;
	public float fallMultiplier = 5.0f;
	public float reqFallSpeed = -0.1f;
	public float currentHeight = 0.0f;
	
	//Jump bools
	public bool ground = false;
	public bool ground2 = false;
	public bool falling = true;
	public bool jumping = false;
	public bool peak = false;
	public bool upDone = true;
	public bool left = false;
	public bool right = false;
	
	//location variables
	private float lastY = 0.0f;
	private float nowY = 0.0f;
	private float difference = 0.0f;
	
	//Speed variables
	public float speed = 10.0f;
	public float currentSpeed = 0.0f;
	public float forwardAcceleration = 0.6f;
	public float backwardAcceleration = 1.5f;
	public float slowdown = 0.6f;
	//Platforms
	public float platSpeed;
	public bool plataxis;
	public float platdirection;
	public bool rPlat;
	public bool lPlat;
	public bool mPlat;

	private AudioSource asource;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		asource = GetComponent<AudioSource>();
	}

	void FixedUpdate() {
		
		//y-distance calculation
		lastY = nowY;
		nowY = transform.position.y;
		difference = nowY - lastY;
		currentHeight += difference;

		//horizantal movement
		x = Input.GetAxisRaw("Horizontal");
		targetX = x * speed;
		//X axis platforms
		if ((lPlat || rPlat || mPlat) && !plataxis)
			targetX += platSpeed * platdirection;

		Debug.Log(platSpeed);
		Debug.Log(plataxis);
		Debug.Log(platdirection);

		//Speed calculations
		if ((targetX > 0 && currentSpeed >= 0) || (targetX < 0 && currentSpeed <= 0)) {
			currentSpeed = ForwardAccelerate(targetX, currentSpeed, forwardAcceleration);
			currentSpeed = SnapToSpeed(targetX, currentSpeed, forwardAcceleration);
		}
		else if ((targetX < 0 && currentSpeed > 0) || (targetX > 0 && currentSpeed < 0)) {
			currentSpeed = BackwardAccelerate(targetX, currentSpeed, backwardAcceleration);
			currentSpeed = SnapToSpeed(targetX, currentSpeed, backwardAcceleration);
		}
		else if (targetX == 0) {
			currentSpeed = Slow(targetX, currentSpeed, slowdown);
			currentSpeed = SnapToSpeed(targetX, currentSpeed, slowdown);
		}

		//Vertical calculations
		ground2 = Ground();
		if (ground2 == true && jumping == false) {
			jumpCount = jumpMax;
			falling = false;
			ground = false;	
			peak = false;
			currentY = 0.0f;
		}

		if (ground == true) {
			jumpCount = jumpMax;
			falling = false;
			ground = false;
		}

		//Jump progession
		if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpCount > 0) {
			currentY = Jump(currentY, jumpSpeed);
			jumpCount--;
			jumping = true;
			asource.Play(0);
		}
		if ((jumpHeight - currentHeight > jumpCutoff) && currentHeight > jumpHeight - 4 && jumping) {
			currentY = GoingUp();
		}
		if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || (jumpHeight - currentHeight < jumpCutoff) || peak == true) && (jumping == true && upDone == true)) {
			currentY = Peak();
			peak = true;
		}

		if (falling) {
			if (currentY > -maxFallSpeed)
				currentY = Fall(currentY, fastFallSpeed, FallSpeed, slowFallSpeed, fallMultiplier);
			else
				currentY = -maxFallSpeed;
			peak = false;
		}

		if (difference == 0 && !ground2) {
			falling = true;
			jumping = false;
			peak = false;
		}

		//Moving between jump states
		if (currentY < reqFallSpeed) {
			falling = true;
			jumping = false;
		}

		if (currentY < -maxFallSpeed)
			currentY = -maxFallSpeed;

		//On a wall?
		left = LeftOrRight(Vector2.left);

		right = LeftOrRight(Vector2.right);


		if ((left || right) && !jumping)
			jumpCount = jumpMax;


		//Platform
		if ((lPlat || rPlat || mPlat) && (plataxis))
			currentY += platSpeed * platdirection;

		rb.velocity = new Vector2(currentSpeed, currentY);
	}

	//Horizantal methods
	float ForwardAccelerate(float targetX, float currentSpeed, float forwardAcceleration) {
		//Aceleration towards the target speed
		if (targetX > 0 && currentSpeed < targetX)
			currentSpeed += forwardAcceleration;
		if (targetX < 0 && currentSpeed > targetX)
			currentSpeed -= forwardAcceleration;
		if (targetX == 0 && currentSpeed < targetX)
			currentSpeed += forwardAcceleration;
		if (targetX == 0 && currentSpeed > targetX)
			currentSpeed -= forwardAcceleration;
		return currentSpeed;
	}


	float BackwardAccelerate(float targetX, float currentSpeed, float backwardAcceleration) {
		//Aceleration towards the target speed, but back
		if (targetX > 0 && currentSpeed < targetX)
			currentSpeed += backwardAcceleration;
		if (targetX < 0 && currentSpeed > targetX)
			currentSpeed -= backwardAcceleration;
		if (targetX == 0 && currentSpeed < targetX)
			currentSpeed += backwardAcceleration;
		if (targetX == 0 && currentSpeed > targetX)
			currentSpeed -= backwardAcceleration;

		return currentSpeed;
	}


	float Slow(float targetX, float currentSpeed, float slowdown) {
		//Slowing down if you don't hold anything
		if (targetX > currentSpeed) {
			currentSpeed += slowdown;
		}
		if (targetX < currentSpeed) {
			currentSpeed -= slowdown;
		}

		return currentSpeed;
	}



	float SnapToSpeed(float targetX, float currentSpeed, float acceleration) {
		// Snap to correct value
		if (targetX > 0 && currentSpeed - targetX > 0)
			currentSpeed = targetX;
		if (targetX < 0 && currentSpeed - targetX < 0)
			currentSpeed = targetX;
		if (targetX == 0 && currentSpeed > 0 && currentSpeed - acceleration < 0)
			currentSpeed = 0;
		if (targetX == 0 && currentSpeed < 0 && currentSpeed + acceleration > 0)
			currentSpeed = 0;
		return currentSpeed;
	}


	bool Ground() {
		bool x;
		GroundCheck = Physics2D.Raycast(transform.position, Vector2.down, ColliderDistance);
		Debug.DrawRay(transform.position, Vector2.down, Color.green, ColliderDistance);

		if (GroundCheck.collider != null) {
			if (GroundCheck.collider.tag == "Ground" || GroundCheck.collider.tag == "Platform") {
				x = true;
			}
			else {
				x = false;
			}
			if (GroundCheck.collider.tag == "Platform") {
				platSpeed = GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().speed;
				plataxis = GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().MoveUp;
				platdirection = GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().direction;
				mPlat = true;
			}
			else
				mPlat = false;
		}
		else {
			x = false;
			mPlat = false;
		}

		return x;

	}



	float Jump(float currentY, float jumpSpeed) {
		if (currentY < 0)
			currentY = jumpSpeed;
		else
			currentY += jumpSpeed;
		if (left) {
			currentSpeed = speed * 2;
			currentY += jumpSpeed / 3;
		}
		if (right) {
			currentSpeed = -speed * 2;
			currentY += jumpSpeed / 3;
		}
		if (left && right) {
			currentSpeed = 0;
			currentY += jumpSpeed / 3;
		}
		return currentY;
	}



	float GoingUp() {
		currentY -= 4.0f / 30;
		return currentY;
	}


	float Peak() {
		currentY -= jumpCutoff / (.5f + currentHeight / 3.0f);
		Debug.Log(currentY);
		return currentY;
	}



	float Fall(float currentY, float fastFallSpeed, float FallSpeed, float slowFallSpeed, float fallMultiplier) {
		//if ((Input.GetButton("Jump") || Input.GetKey("up") || Input.GetKey("w")) && currentY < maxFallSpeed)
		//{
		//    currentY -= slowFallSpeed * .1f;
		//}
		if (currentY > -1) {
			if (Input.GetKey("down") || Input.GetKey("s")) {
				currentY -= fastFallSpeed * .2f;
			}
			else {
				currentY -= FallSpeed * .2f;
			}
		}
		else {
			if (Input.GetKey("down") || Input.GetKey("s")) {
				currentY -= fastFallSpeed * .1f;
			}
			else {
				currentY -= FallSpeed * .1f;
			}
		}

		peak = false;

		return currentY;
	}



	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject != null) {
			falling = false;
			currentY = 0.0f;
			currentHeight = 0.0f;
			ground = true;
		}
		foreach (ContactPoint2D contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
	}

	//Walljump stuff
	bool LeftOrRight(Vector2 direction) {
		bool x;
		GroundCheck = Physics2D.Raycast(transform.position, direction, ColliderDistance2);
		Debug.DrawRay(transform.position, direction, Color.green, ColliderDistance2);

		if (GroundCheck.collider != null) {
			x = GroundCheck.collider.CompareTag("Ground") || GroundCheck.collider.CompareTag("Platform");

			if (GroundCheck.collider.CompareTag("Platform")) {
				
				if (!GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().MoveUp) {
					platSpeed = GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().speed;
					plataxis = GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().MoveUp;
					platdirection = GroundCheck.collider.gameObject.GetComponent<PlatformMovement>().direction;
					rPlat = true;
				}
				
				else
					rPlat = false;
			}
		}
		
		else {
			x = false;
			rPlat = false;
		}
		
		return x;
	}
}
