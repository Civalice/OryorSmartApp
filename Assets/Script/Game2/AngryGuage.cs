using UnityEngine;
using System.Collections;

public class AngryGuage : MonoBehaviour {
	public GameObject TopGuage;
	public GameObject MiddleGuage;
	public GameObject BottomGuage;

	float percent = 100.0f;

	float timer = 1.0f;
	float currentTimer = 1.0f;
	bool sick = false;
	bool IsGamePlay = false;

	public void SetProgress(float percent)
	{
		//move top and scale middleguage
		//87 = 100%
		float r = (percent < 50.0f) ? 1.0f : (100 - percent) / 50.0f;
		float g = ((percent*2/100.0f)>1.0f)?1.0f:percent*2/100.0f;
		Color mColor = new Color (r, g, 0.0f, 1.0f);

		float progress = 83 * percent / 100.0f;
		TopGuage.transform.localPosition = new Vector3 (0, progress / 100.0f, 0);
		MiddleGuage.transform.localScale = new Vector3 (1, progress, 1);
		TopGuage.GetComponent<SpriteRenderer> ().color = mColor;
		MiddleGuage.GetComponent<SpriteRenderer> ().color = mColor;
		BottomGuage.GetComponent<SpriteRenderer> ().color = mColor;
	}

	public void StartPlayGame()
	{
		IsGamePlay = true;
	}
	public void GameOver()
	{
		IsGamePlay = false;
	}

	public void SetTimer(float time)
	{
		timer = time;
		currentTimer = time;
	}

	public void setSick(bool flag)
	{
		sick = flag;
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Game2Global.GlobalPause)
			return;
		if (sick)
			return;
		if (!IsGamePlay)
			return;
		currentTimer -= Time.deltaTime;
		if (currentTimer < 0) {
			//pig Angry
			Game2Global.PigAngry();
			currentTimer = timer;
		}
		percent = currentTimer*100 / timer;
		SetProgress (percent);
	}
}
