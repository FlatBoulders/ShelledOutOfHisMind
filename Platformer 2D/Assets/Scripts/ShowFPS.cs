using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour {

	private TMP_Text TMP;
	private float deltaTime;
	private float fps;

	void Start() {
		TMP = GetComponent<TextMeshProUGUI>();
	}

	void Update() {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		fps = 1.0f / deltaTime;

		if (fps >= 240f)
			TMP.text = "240";
		else
			TMP.text = Mathf.Ceil(fps).ToString();
	}
}