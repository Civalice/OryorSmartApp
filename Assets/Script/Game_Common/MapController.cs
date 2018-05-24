using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	public BoxCollider2D touchArea;
	public GameObject MapObject;

	private Vector2 lastTouchPos;
	private Vector3 currentMapPos;
	private Vector2 pos;
	private bool IsDrag = false;
	// Use this for initialization
	void Start () {
		//set boxcollider size due to Area size
		//640x960 = 6.3
		touchArea.size = new Vector2 (touchArea.size.x,touchArea.size.y+ScreenRatio.SizeDiff()*2);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameLandGlobal.state != GameLandState.GS_MAPMODE) return;
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		pos = TouchInterface.GetTouchPosition ();
		if (TouchDown)
		{
			if (touchArea.OverlapPoint(pos)){
				lastTouchPos = pos;
				currentMapPos = MapObject.transform.localPosition;
				IsDrag = true;
			}
		}
		else if (TouchUp)
		{
			IsDrag = false;
		}

		if (IsDrag)
		{
			//calculate CurrentMapPos
			Vector3 movedPosition = currentMapPos + new Vector3(pos.x - lastTouchPos.x,
			                                                    pos.y - lastTouchPos.y,
			                                                    0);
//			MapObject.rigidbody2D.transform.localPosition = movedPosition;
			MapObject.GetComponent<Rigidbody2D>().velocity = (movedPosition-currentMapPos)*60;
			lastTouchPos = pos;
		}
		else
		{
			MapObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		}
	}	
}
