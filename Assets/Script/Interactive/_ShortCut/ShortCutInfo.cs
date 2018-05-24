using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider2D))]
public class ShortCutInfo : MonoBehaviour {
	public string HeaderText;
	public string DetailText;
	public string ButtonText = "ปิด";
	// Use this for initialization
	void Awake () {
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) 
		{
			if (this.GetComponent<Collider2D>().OverlapPoint(touchPos))
			{
				//run event here
				PopupObject.ShowAlertPopup(HeaderText,DetailText,ButtonText);
//				StopCoroutine("TestTiming");
//				StartCoroutine("TestTiming");
//				PopupObject.ShowWaitingPopup(HeaderText,"Cancel",ProgressCB,Cancel);
			}
		}
	}
//
//	float progress = 0.0f;
//
//	IEnumerator TestTiming()
//	{
//		float time = 0.0f;
//		progress = 0.0f;
//		while (progress < 1.0f)
//		{
//			time += Time.deltaTime;
//			progress = time/5.0f;
//			yield return null;
//		}
//	}
//
//	void Cancel()
//	{
//		StopCoroutine("TestTiming");
//	}
//
//	float ProgressCB(out string text)
//	{
//		text = "Waiting " + progress;
//		return progress;
//	}
}
