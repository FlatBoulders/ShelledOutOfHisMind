using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

	private Vector2 origin;
	public GameObject DeathPrefab;
	public GameObject AudioClipSource;
	private AudioSource ASource;

	public bool GoalReached = false;
	public AudioClip fall;
	public AudioClip death;
	void Start() {
		origin = transform.position;
		ASource = AudioClipSource.GetComponent<AudioSource>();
	}

	private void Update() {
		if (transform.position.y < -50) {
			StartCoroutine(TimerSleep());
			ASource.clip = fall;
			ASource.Play();
		}
		
	}
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Enemy")) {
			StartCoroutine(TimerSleep());
			ASource.clip = death;
			ASource.Play();
		}

		if (collision.gameObject.CompareTag("Goal"))
			GoalReached = true;
	}

	IEnumerator TimerSleep() {
		Instantiate(DeathPrefab, transform.position, Quaternion.identity);
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponentInChildren<SpriteRenderer>().enabled = false;
		yield return new WaitForSecondsRealtime(0.25f);
		transform.position = origin;
		gameObject.GetComponent<PlayerMovement>().falling = true;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		GetComponentInChildren<SpriteRenderer>().enabled = true;
	}
}
