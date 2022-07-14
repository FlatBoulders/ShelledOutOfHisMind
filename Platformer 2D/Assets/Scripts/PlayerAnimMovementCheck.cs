using UnityEngine;

public class PlayerAnimMovementCheck : MonoBehaviour {
	private Animator anim;
	private PlayerMovement pm;
	private SpriteRenderer sr;

	void Start() {
		anim = GetComponent<Animator>();
		pm = GetComponent<PlayerMovement>();
		sr = GetComponent<SpriteRenderer>();
	}

	void Update() {
		if (pm.currentSpeed != 0)
			anim.SetFloat("IdleSpeed", 1f);
		else
			anim.SetFloat("IdleSpeed", 0f);

		if (pm.currentSpeed > 0f)
			sr.flipX = false;
		
		else if (pm.currentSpeed < 0f)
			sr.flipX = true;
	}	
}
