using UnityEngine;

public class Parallax : MonoBehaviour {

	private Transform cam;
	private Vector2 previousCamPos;

	private float distanceX = 0f;
	private float distanceY = 0f;

	public float smoothingX = 1f;
	public float smoothingY = 1f;

	void Start() {
		cam = Camera.main.transform;
	}

	void Update() {
		if (distanceX != 0f) {
			float parallaxX = (previousCamPos.x - cam.position.x) * distanceX;
			Vector2 backgroundTargetPosX = new(transform.position.x + parallaxX, transform.position.y);

			transform.position = Vector2.Lerp(transform.position, backgroundTargetPosX, smoothingX * Time.deltaTime);
		}

		if (distanceY != 0f) {
			float parallaxY = (previousCamPos.y - cam.position.y) * distanceY;
			Vector2 backgroundTargetPosY = new(transform.position.x, transform.position.y + parallaxY);

			transform.position = Vector2.Lerp(transform.position, backgroundTargetPosY, smoothingY * Time.deltaTime);
		}
		previousCamPos = cam.position;
	}
}