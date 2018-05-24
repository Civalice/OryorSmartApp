using UnityEngine;
using System.Collections;

public class CloseSuggestPage : MonoBehaviour {
	public Collider2D mbox;
	public GameObject MainPage;
	public GameObject AddPage;
	public GameObject SuggestPage;
	public GameObject Head;
	public DietaryMainSound mSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if(mbox.OverlapPoint(touchPos)){
				mSound.playDietarySound("click");
				MainPage.SetActive(true);
				AddPage.SetActive(false);
				Head.SetActive(true);
				SuggestPage.SetActive(false);
			}
		}
	}
}
