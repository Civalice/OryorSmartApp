using UnityEngine;
using System.Collections;

public class BTBMIFemale : MonoBehaviour {
	public Collider2D mBox;
	public GameObject Male;
	public GameObject MalePress;
	public GameObject Female;
	public GameObject FemalePress;
	public BMISoundController mSound;

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
				mSound.playBMISound("click");
				BTBMICal.sex = false;
				Male.SetActive (true);
				MalePress.SetActive (false);
				Female.SetActive (false);
				FemalePress.SetActive (true);
				Debug.Log(BTBMICal.sex);
			}
		}
	}
}
