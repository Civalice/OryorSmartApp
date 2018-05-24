using UnityEngine;
using System.Collections;

public class DieAddBT : MonoBehaviour {
	public Collider2D mBox;
	public GameObject MainPage;
	public GameObject AddPage;
	public DietaryMainSound mSound;
	public ControlAddEvent cae;

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
				//show button
				mSound.playDietarySound("click");
				MainPage.SetActive(false);
				AddPage.SetActive(true);
				cae.setAddState();
			}
		}
	}
}
