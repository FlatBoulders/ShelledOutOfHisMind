using UnityEngine;

public class MusicLoopCheck : MonoBehaviour {
	private AudioSource music;
	public AudioClip LoopSource;
	
	void Start() {
		music = GetComponent<AudioSource>();
	}
	void Update() {
		if (!music.isPlaying) {
			music.clip = LoopSource;
			music.Play();
			music.loop = true;
		}
	}
}
