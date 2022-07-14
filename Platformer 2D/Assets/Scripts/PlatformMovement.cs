using UnityEngine;

public class PlatformMovement : MonoBehaviour {
	public bool MoveUp = true;
	public bool StartPositive = true;
	public float radius = 5f;
	public float speed = 2f;

	public float direction;
	public Vector2 axis;
	public Vector2 StartPosition;
	public Color AxisColor;
	public SpriteRenderer sr;
	public Color orange = new(1f, 0.5f, 0f, 1f);

	void Start() {
		sr = GetComponent<SpriteRenderer>();

		axis = MoveUp ? Vector2.up : Vector2.right;
		AxisColor = MoveUp ? orange : Color.blue;
		sr.color = AxisColor;

		direction = StartPositive ? 1f : -1f;
		StartPosition = transform.position;
	}

	void Update() {
		transform.Translate(axis * speed * direction * Time.deltaTime);
		if (Mathf.Abs(Vector2.Distance(StartPosition, transform.position)) > radius)
			direction *= -1f;
	}
}
