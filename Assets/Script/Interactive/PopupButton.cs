using UnityEngine;
using System.Collections;
using TMPro;

public class PopupButton : MonoBehaviour {
	public Collider2D box;
	public TextMeshPro buttonText;
	public delegate void ButtonAction();
	public ButtonAction OnClicked;
	public ButtonAction OnReleased;
	public ButtonAction HideEvent;
	
	public bool isenable = true;
	private bool IsTouch;
	
	public void Enable() {
		isenable = true;
	}
	public void Disable() {
		isenable = false;
	}
	
	public bool IsEnable() {
		return isenable;
	}

	public void SetText(string txt)
	{
		buttonText.text = txt;
	}
	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	protected virtual void Update () {
		if (!PopupObject.IsPopup) return;
		if (GetComponent<Collider2D>() == null) return;
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
					//run event here
					if (OnClicked != null) {
						Debug.Log ("OnClicked");
						OnClicked();
					}
				}
			}
		}
		if (touchedUp) {
			if (this.GetComponent<Collider2D>().OverlapPoint(touchPos))
			{
				if (IsTouch) { 
					if (HideEvent != null)
					{
						HideEvent();
					}
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
