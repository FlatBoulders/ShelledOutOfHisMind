using UnityEngine;

public class MainMenu : MonoBehaviour {

	void Start() {
		Time.timeScale = 0f;
	}

	void Update() {
		if (Input.anyKey) {
			Time.timeScale = 1f;
			Destroy(gameObject);
		}
	}
}