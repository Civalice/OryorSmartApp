using UnityEngine;
using System.Collections;
using TMPro;

public class SubMenuButton : MonoBehaviour {
	public GameObject SubMenuBG;
	public TextMeshPro mText;
	public delegate void eventCallback(int idx);
	public event eventCallback OnPressed;
	public Collider2D mCollider;
	public int ButtonIdx = -1;
	void Start()
	{
	}

	void Update()
	{
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (mCollider.OverlapPoint(touchPos))
			{
				//show button
				OnPressed(ButtonIdx);
			}
		}
	}
	
	public void SetText(string text)
	{
		mText.text = text;
	}
};
