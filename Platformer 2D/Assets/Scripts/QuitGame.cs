using UnityEngine;

public class QuitGame : MonoBehaviour {

	void Update() {
		if (Input.GetKeyUp(KeyCode.Escape))
			Application.Quit();
	}
}