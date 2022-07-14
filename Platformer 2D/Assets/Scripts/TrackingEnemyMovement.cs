using System.Collections;
using UnityEngine;

public class TrackingEnemyMovement : MonoBehaviour {

	public float speed = 2f;
	public float TrackingRadius = 10f;
	public float FallDespawnDistance = 15f;
	public bool IsLeft = false;
	public GameObject PlayerTarget;

	private float direction;
	private float ColliderDistance;
	private float CollisionDisplacement;
	private bool PlayerLeft;
	private bool IsIdle = true;

	private Vector2 movement = Vector2.zero;
	private Vector2 GroundCheckRaycast;
	private Vector2 origin;
	private Vector2 IdleDestination;
	private RaycastHit2D PlayerInRadius;
	private RaycastHit2D GroundCheck;

	void Start() {
		origin = transform.position;
		IdleDestination.x = origin.x;
		ColliderDistance = (transform.localScale.y / 2f) + 0.1f;
	}

	void Update() {
		if (transform.position.y + FallDespawnDistance < origin.y)
			Destroy(gameObject);

		if (IsIdle) {
			direction = IsLeft ? -1f : 1f;

			if (transform.position.x > origin.x + TrackingRadius || transform.position.x < TrackingRadius || IsNotGrounded()) {
				IsLeft = !IsLeft;
				GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
			}
		}

		if (PlayerTrack()) {
			IsIdle = false;
			speed = 4f;

			PlayerLeft = PlayerTarget.transform.position.x < transform.position.x;
			direction = PlayerLeft ? -1f : 1f;
		}

		else {
			IsIdle = true;
			speed = 2f;
			StartCoroutine(TimerSleep());
			IdleDestination.y = transform.position.y;
			transform.position = Vector2.Lerp(transform.position, IdleDestination, Time.deltaTime);
		}

		movement.x = speed * direction * Time.deltaTime;
		transform.Translate(movement);
	}

	private bool PlayerTrack() {
		PlayerInRadius = Physics2D.Raycast(transform.position, Vector2.right * direction, TrackingRadius);
		//Debug.DrawRay(transform.position, Vector2.right * direction, Color.red, TrackingRadius);

		if (PlayerInRadius.collider != null)
			return PlayerInRadius.collider.CompareTag("Player");
		return false;
	}

	private bool IsNotGrounded() {
		CollisionDisplacement = (transform.localScale.x / 2f) + 0.1f;
		GroundCheckRaycast = new(transform.position.x + (CollisionDisplacement * direction), transform.position.y);
		GroundCheck = Physics2D.Raycast(GroundCheckRaycast, Vector2.down, ColliderDistance);
		//Debug.DrawRay(transform.position, Vector2.down, Color.yellow, ColliderDistance);
		return GroundCheck.collider == null;
	}

	IEnumerator TimerSleep() {
		yield return new WaitForSecondsRealtime(2f);
	}
}