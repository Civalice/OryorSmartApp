using UnityEngine;
using System.Collections;

public class AddAdd : MonoBehaviour {
	public Collider2D mBox;
	public ControlAddEvent cEvent;
	public DietaryMainSound mSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if (mBox.OverlapPoint(touchPos))
			{
				mSound.playDietarySound("click");
				cEvent.AddData();
			}
		}
	}
}
