using UnityEngine;
using System.Collections;

public class DieSelectedCat : MonoBehaviour {
	public Collider2D mBox;
	public GameObject thisSel;
	public GameObject thisSeled;
	public DietaryMainSound mSound;

	public int SelCat;
	private bool Sel = false;

	public ControlAddEvent control;

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
				if(!Sel){thisSeled.SetActive(true);thisSel.SetActive(false);}
				else{thisSeled.SetActive(false);thisSel.SetActive(true);}
				Sel=!Sel;
				control.selectingCat(SelCat);
			}
		}
	}
}
