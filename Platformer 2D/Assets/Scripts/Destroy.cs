using UnityEngine;

public class Destroy : MonoBehaviour {

	private ParticleSystem parts;
	void Start() {
		parts = GetComponent<ParticleSystem>();
	}
	void Update() {
		Destroy(gameObject, parts.main.duration + parts.main.startLifetimeMultiplier + 0.1f);
	}
}