using UnityEngine;

public class SmoothCamera : MonoBehaviour {

	public Transform target;
	public float smoothing = 0.3f;
	private Vector2 TargetPosition;

	void Update() {
		TargetPosition = new Vector2(target.position.x, target.position.y);
		transform.position = Vector2.Lerp(transform.position, TargetPosition, smoothing * Time.deltaTime);
	}
}