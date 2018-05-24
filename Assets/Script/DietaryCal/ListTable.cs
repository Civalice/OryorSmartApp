using UnityEngine;
using System.Collections;
using TMPro;

public class ListTable : MonoBehaviour {
	public ControlMainEvent cEvent;
	public GameObject MainPage;
	public GameObject AddPage;
	public ControlAddEvent cae;
	private int Pos;
	public TextMeshPro fName;
	public TextMeshPro rCal;
	public Food fData;

	public GameObject edit;
	public GameObject delete;
	public GameObject cancel;

	public Collider2D mBox;
	public Collider2D editBox;
	public Collider2D cancelBox;
	public Collider2D deleteBox;
	public Collider2D pBoxFootter;

	private bool IsEdit = false;
	private bool IsTouch = false;
	private bool IsSlide = false;
	private bool IsFunc = false;
	
	private Vector2 firstPos;
	private Vector2 lastTouch;

	public DietaryMainSound mSound;

	// Use this for initialization
	void Start () {
		firstPos.x = 0;
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if(mBox.OverlapPoint(touchPos) && !(pBoxFootter.OverlapPoint(touchPos))){
				IsTouch = true;
				lastTouch = touchPos;
				if(firstPos.x==0){
					firstPos = touchPos;
					//Debug.Log(Vector2.Distance(firstPos,lastTouch).ToString()+" "+firstPos.x+" "+touchPos.x);
				}
			}
			if(cancelBox.OverlapPoint(touchPos)){
				mSound.playDietarySound("click");
				IsEdit = false;
				edit.SetActive(false);
				delete.SetActive(false);
				cancel.SetActive(false);
			}
			if(deleteBox.OverlapPoint(touchPos)){
				mSound.playDietarySound("click");
				IsEdit = false;
				cEvent.deleteFoodList(fData,Pos);
				edit.SetActive(false);
				delete.SetActive(false);
				cancel.SetActive(false);
			}
			if(editBox.OverlapPoint(touchPos)){
				mSound.playDietarySound("click");
				IsEdit = false;
				edit.SetActive(false);
				delete.SetActive(false);
				cancel.SetActive(false);
				
				MainPage.SetActive(false);
				AddPage.SetActive(true);

				cae.EditMode(fData,Pos);
			}
		}
		if (touchedUp) {
			//touch up here
			if (IsTouch) {
				IsTouch = false;
				//Debug.Log(Vector2.Distance(firstPos,lastTouch).ToString()+" "+firstPos.x+" "+touchPos.x);
				if((Mathf.Abs(firstPos.x-touchPos.x)>=0.2f || Mathf.Abs(firstPos.y-touchPos.y)<=0.09f) && !IsFunc)
				{
					//IsSlide = true;
					IsEdit = true;
					IsFunc = true;
					edit.SetActive(true);
					delete.SetActive(true);
					cancel.SetActive(true);
				}
				firstPos.x = 0;
			}
			if(!IsEdit){
				IsFunc = false;
			}
		}
	}
	public void setContent(Food f,int i){
		fName.text = f.FoodName;
		rCal.text = f.reciveCal.ToString();
		//selCat = f.selCat;
		fData = f;
		Pos = i;
	}
}
