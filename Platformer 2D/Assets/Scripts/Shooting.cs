using System;
using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public float TrackingRadius;
	public GameObject beam;
	public GameObject DestroyedPrefab;
	public float BeamLength;

	private RaycastHit2D[] EnemyCheck;
	private float direction = 1f;
	private bool CanShoot = true;
	private AudioSource ASource;

	void Start() {
		beam.SetActive(false);
		TrackingRadius = BeamLength * transform.localScale.x / 2f;
	}

	void Update() {
		if (Input.GetAxisRaw("Horizontal") != 0f) {
			direction = Input.GetAxisRaw("Horizontal");
			transform.localPosition = new(direction * 0.3f, 0f);
		}

		if ((Input.GetButton("Fire1") || Input.GetKeyDown(KeyCode.Return)) && CanShoot) {
			CanShoot = false;

			EnemyCheck = Physics2D.RaycastAll(transform.position, Vector2.right * direction, TrackingRadius);

			beam.transform.localPosition = new(direction * BeamLength / 2f, beam.transform.localPosition.y);
			beam.SetActive(true);			

			if (EnemyCheck != null) {
				for (int i = 0; i < EnemyCheck.Length; i++) {
					if (EnemyCheck[i].collider.CompareTag("Enemy")) {
						ASource.Play();
						//Debug.Log("Hit " + EnemyCheck[i].collider.name);
						Instantiate(DestroyedPrefab, EnemyCheck[i].collider.gameObject.transform.position, Quaternion.identity);
						Destroy(EnemyCheck[i].collider.gameObject);
					}
				}
			}
			Array.Clear(EnemyCheck, 0, EnemyCheck.Length);

			StartCoroutine(TimerSleep());
		}
	}

	IEnumerator TimerSleep() {
		yield return new WaitForSecondsRealtime(0.1f);
		beam.SetActive(false);
		yield return new WaitForSecondsRealtime(0.15f);
		CanShoot = true;
	}
}