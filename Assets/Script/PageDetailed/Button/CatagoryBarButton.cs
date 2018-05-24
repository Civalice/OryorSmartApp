using UnityEngine;
using System.Collections;

public class CatagoryBarButton : MonoBehaviour {
	public Collider2D ShelfButton;
	public Collider2D FullListButton;
	public Collider2D FavouriteButton;
	public ContentMainSound mSound;

	public GameObject Arrow;
	public float ArrowSpeed = 10.0f;
	public int CatagoryNumber = 1;
	private Vector3 nextPosition;
	// Use this for initialization
	void Start () {
		Arrow.transform.localPosition = new Vector3(CatagoryNumber*2-4,-0.25f,0);
		nextPosition = Arrow.transform.localPosition;
		switch (CatagoryNumber) {
		case 1:
			CatagoryNumber1();
			break;
		case 2:
			CatagoryNumber2();
			break;
		case 3:
			CatagoryNumber3();
			break;
				}
	}
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		//				Debug.Log (Camera.main.ScreenToWorldPoint (touchPos));
		if (touchedDown) {
			if (ShelfButton.OverlapPoint(touchPos)) {
				mSound.playContentSound("click");
				CatagoryNumber = 1;
				CatagoryNumber1();
			} else if (FullListButton.OverlapPoint(touchPos)) {
				mSound.playContentSound("click");
				CatagoryNumber = 2;
				CatagoryNumber2();
			} else if (FavouriteButton.OverlapPoint(touchPos)) {
				mSound.playContentSound("click");
				CatagoryNumber = 3;
				CatagoryNumber3();
			}
//						nextPosition = new Vector3 (CatagoryNumber * 2 - 4, -0.25f, 0);
			nextPosition.x = CatagoryNumber * 2 - 4;
			StopCoroutine("MoveArrow");
			StartCoroutine("MoveArrow");
		}
	}
	void CatagoryNumber1()
	{
		PageDetailGlobal.ShowShelf ();
	}
	void CatagoryNumber2()
	{
		PageDetailGlobal.ShowFullList ();
	}
	void CatagoryNumber3()
	{
		PageDetailGlobal.ShowFavList ();
	}
	IEnumerator MoveArrow()
	{
		while (Vector3.Distance(Arrow.transform.localPosition, nextPosition) > 0.05f) {
			Arrow.transform.localPosition = Vector3.Lerp(Arrow.transform.localPosition, nextPosition, ArrowSpeed*Time.deltaTime);
			yield return null;
				}
//		Arrow.transform.localPosition = new Vector3 (CatagoryNumber * 2 - 4, -0.25f, 0);
		Arrow.transform.localPosition = nextPosition;
	}
}
