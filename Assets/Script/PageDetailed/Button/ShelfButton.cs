using UnityEngine;
using System.Collections;

public class ShelfButton : MonoBehaviour {
	public Collider2D LikeButton;
	public Collider2D FavButton;
	public Collider2D ShareButton;

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
			if (LikeButton.OverlapPoint (touchPos)) {
						}
			if (FavButton.OverlapPoint (touchPos)) {
								//add Favourite to list
						}
			if (ShareButton.OverlapPoint (touchPos)) {
						}			
				}
	}
}
