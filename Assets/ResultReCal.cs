using UnityEngine;
using System.Collections;

public class ResultReCal : MonoBehaviour {
	public Collider2D mCollider;
	public GameObject ResultLayer;
	public GameObject Header;
	public GameObject Detail;
	public GameObject BMIDetailBT;
	public GameObject BMRDetailBT;
	public Animator Female;
	public Animator Male;
	public BMISoundController mSound;
	//public GameObject BMIBubbleFat;

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
			if (mCollider.OverlapPoint(touchPos))
			{
				//show button
				Header.SetActive(true);
				Detail.SetActive(true);
				ResultLayer.SetActive (false);
				BMIDetailBT.SetActive(true);
				BMRDetailBT.SetActive(false);
				mSound.playBMISound("click");
			}
		}
	}
}
