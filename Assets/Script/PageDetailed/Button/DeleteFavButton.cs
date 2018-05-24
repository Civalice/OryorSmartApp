using UnityEngine;
using System.Collections;

public class DeleteFavButton : MonoBehaviour {
	public Collider2D ButtonCollider;
	public Collider2D box = null;
	public ContentData cData;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
//			if (ButtonCollider.OverlapPoint(touchPos))
			if (ButtonCollider.OverlapPoint(touchPos)&&
			    ((box == null)||((box != null)&&(box.OverlapPoint(touchPos)))))
			{
				//show button
				PageDetailGlobal.RemoveFavourite(cData);
			}
		}
	}

	public void SetContentData(ContentData data)
	{
		cData = data;
	}
}
