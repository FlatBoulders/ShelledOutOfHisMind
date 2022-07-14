using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour {

	public float speed = 2f;

	public bool IsLeft = false;
	private float direction;

	public float MaxDistance = 10f;
	public float FallDespawnDistance = 15f;
	private Vector2 movement;

	private float StartX;
	private float StartY;

	void Start() {
		StartX = transform.position.x;
		StartY = transform.position.y;
	}

	void Update() {
		if (transform.position.y + FallDespawnDistance < StartY)
			Destroy(gameObject);

		direction = IsLeft ? -1f : 1f;
		movement = new Vector2(speed * direction, 0f) * Time.deltaTime;
		transform.Translate(movement);

		if (transform.position.x > StartX + MaxDistance || transform.position.x < StartX) {
			GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
			IsLeft = !IsLeft;
		}
	}
}