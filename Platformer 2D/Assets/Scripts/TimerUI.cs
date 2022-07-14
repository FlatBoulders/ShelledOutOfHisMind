using System.Collections;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour {

	private TMP_Text TMP;
	private const int start = 60;

	public GameObject player;
	public GameObject goal;

	private bool GoalReached;
	private int TimeCompleted;

	void Start() {
		TMP = GetComponent<TextMeshProUGUI>();
		TMP.text = start.ToString();
		StartCoroutine(TimerSleep());
		GoalReached = player.GetComponent<PlayerDeath>().GoalReached;
	}

	IEnumerator TimerSleep() {
		if (!GoalReached) {
			for (TimeCompleted = 0; TimeCompleted <= start; TimeCompleted++) {
				yield return new WaitForSecondsRealtime(1f);
				TMP.text = (start - TimeCompleted).ToString();
			}
		}

		if (TimeCompleted >= 60) {
			TMP.color = Color.red;
			TMP.text = "Times Up...";
			goal.SetActive(false);
		}
		
		else if (GoalReached) {
			TMP.color = Color.green;
			TMP.text = "Goal Reached!";
		}
	}
}