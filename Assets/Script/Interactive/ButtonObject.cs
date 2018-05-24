using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonObject : MonoBehaviour {
	public Collider2D box;
	public delegate void ButtonAction();
	public event ButtonAction OnClicked;
	public event ButtonAction OnReleased;

	public bool ButtonSpriteChange = false;
	public GameObject ButtonSpriteNormal;
	public GameObject ButtonSpritePress;

	private bool isenable = true;
	private bool IsTouch;

	Vector2 lastTouchPos;

	public void Enable() {
		isenable = true;
		}
	public void Disable() {
		isenable = false;
	}

	public bool IsEnable() {
		return isenable;
		}
	// Use this for initialization
	void Start () {
		}
	// Update is called once per frame
	protected virtual void Update () {
		if (PopupObject.IsPopup) return;
		if (GetComponent<Collider2D>() == null) return;
		if (LoadingScript.IsLoading)
			return;
		if (!isenable) {
			return;
		}
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) 
		{
			if ((box!=null) && (!box.OverlapPoint(touchPos)))
				return;
			if (this.GetComponent<Collider2D>().OverlapPoint(touchPos))
			{
				if ((box==null)|| ((box!=null) && (box.OverlapPoint(touchPos))))
				{
					IsTouch = true;
					lastTouchPos = touchPos;
					if (ButtonSpriteChange) {
						ButtonSpritePress.SetActive (true);
						ButtonSpriteNormal.SetActive (false);
					}
					//run event here
					if (OnClicked != null) {
						Debug.Log ("OnClicked");
						OnClicked();
					}
				}
			}
		}
		if (touchedUp) {
			if (ButtonSpriteChange) {
				ButtonSpritePress.SetActive (false);
				ButtonSpriteNormal.SetActive (true);
			}
			if (this.GetComponent<Collider2D>().OverlapPoint(touchPos))
			{
				if (IsTouch&&(Vector2.Distance(touchPos,lastTouchPos)<0.5f)) { 
					if (OnReleased != null) {
						Debug.Log ("OnReleased");
						OnReleased();
					}
				}
			}
			IsTouch = false;
		}
	}
}