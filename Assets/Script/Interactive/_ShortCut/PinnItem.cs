using UnityEngine;
using System.Collections;

public class PinnItem : MonoBehaviour {
	public Collider2D PinCollider;
	public ShortCutItem UnPinnObj;
	public Collider2D EventCollider;

	public int index;

	public void Pinn(int idx)
	{
		index = idx;
		transform.localPosition = new Vector3(-8,index*-0.6f,0);
		if (MainMenuGlobal.getCurrentState() == MainMenuState.MS_SHORTCUT)
			UserCommonData.AddBookmark(UnPinnObj.index);
		if (EventCollider != null)
			EventCollider.enabled = false;
		//run Shift in
		StopCoroutine("PlayUnPin");
		StopCoroutine("PlayPin");
		StopCoroutine("PlayShiftUp");
		StartCoroutine("PlayPin");
	}

	public void UnPinn()
	{
		PinnHeader.ShiftUp(this);
		UnPinnObj.PushBackItem();
		if (MainMenuGlobal.getCurrentState() == MainMenuState.MS_SHORTCUT)
			UserCommonData.RemoveBookmark(UnPinnObj.index);
		if (EventCollider != null)
			EventCollider.enabled = false;
		StopCoroutine("PlayUnPin");
		StopCoroutine("PlayPin");
		StopCoroutine("PlayShiftUp");
		StartCoroutine("PlayUnPin");
	}

	public void ShiftUp()
	{
		index--;
		StopCoroutine("PlayShiftUp");
		StartCoroutine("PlayShiftUp");
	}

	IEnumerator PlayPin()
	{
//		transform.localPosition = new Vector3(0,index*-0.6f,0);
		Vector3 currentPosition = transform.localPosition;
		Vector3 targetPosition = new Vector3(0,index*-0.6f,0);
		while (Vector3.Distance(currentPosition,targetPosition) > 0.05f)
		{
			targetPosition.y = index*-0.6f;
			currentPosition = Vector3.Lerp(currentPosition,targetPosition,Time.deltaTime*5);
			transform.localPosition = currentPosition;
			yield return null;
		}
		transform.localPosition = targetPosition;
		if (EventCollider != null)
			EventCollider.enabled = true;
	}

	IEnumerator PlayUnPin()
	{
//		transform.localPosition = new Vector3(-8,0,0);
		Vector3 currentPosition = transform.localPosition;
		Vector3 targetPosition = new Vector3(-8,index*-0.6f,0);
		while (Vector3.Distance(currentPosition,targetPosition) > 0.05f)
		{
			targetPosition.y = index*-0.6f;
			currentPosition = Vector3.Lerp(currentPosition,targetPosition,Time.deltaTime*5);
			transform.localPosition = currentPosition;
			yield return null;
		}
		transform.localPosition = targetPosition;

		yield return null;
	}

	IEnumerator PlayShiftUp()
	{
//		transform.localPosition = new Vector3(0,index*-0.6f,0);
		Vector3 currentPosition = transform.localPosition;
		Vector3 targetPosition = new Vector3(0,index*-0.6f,0);
		while (Vector3.Distance(currentPosition,targetPosition) > 0.05f)
		{
			targetPosition.y = index*-0.6f;
			currentPosition = Vector3.Lerp(currentPosition,targetPosition,Time.deltaTime*5);
			transform.localPosition = currentPosition;
			yield return null;
		}
		transform.localPosition = targetPosition;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (MainMenuGlobal.getCurrentState()!=MainMenuState.MS_SHORTCUT&&
		    MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_UNPIN_RED) return;

		if (PopupObject.IsPopup) return;
		if (PinCollider == null)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown)
		{
			if (PinCollider.OverlapPoint(touchPos))
			{
				if (MainMenuGlobal.getCurrentState()==MainMenuState.MS_TUTORIAL_UNPIN_RED)
				{
					MainMenuGlobal.Tutorial.NextTutorial();
				}
				//UnPinned item
				UnPinn();
			}
		}

	}
}
