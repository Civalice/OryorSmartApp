using UnityEngine;
using System.Collections;
using TMPro;

public class ListAddTable : MonoBehaviour {
	public TextMeshPro fName;
	public TextMeshPro rCal;
	//public bool[] selCat = new bool[];
	public Food fData;
	public ControlAddEvent cae;
	public Collider2D mBox;
	public Collider2D pBox;
	public Collider2D pBoxFooter;
	Vector2 LastTouch;
	
	public DietaryMainSound mSound;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown)
		{
			LastTouch = touchPos;
		}
		if (touchedUp && (Mathf.Abs(LastTouch.y-touchPos.y)<=0.09f) && (pBox.OverlapPoint(touchPos) && !pBoxFooter.OverlapPoint(touchPos))) {
			if (mBox.OverlapPoint(touchPos))
			{
				mSound.playDietarySound("click");
				cae.setText(fData);
			}
		}
	}
	public void setContent(Food f){
		fName.text = f.FoodName;
		rCal.text = f.reciveCal.ToString();
		//selCat = f.selCat;
		fData = f;
	}
}
