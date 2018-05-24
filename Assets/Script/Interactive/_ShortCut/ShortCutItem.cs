using UnityEngine;
using System.Collections;

public class ShortCutItem : MonoBehaviour {
	public int index;
	public Collider2D PinCollider;
	public PinnItem SelfPinn;
	public Collider2D EventTextCollider;

	protected Vector3 normalPosition;
	public bool IsPinn = false;
	protected bool IsRunning = false;
	protected int ShiftCount = 0;

	public virtual void PinnItem()
	{
		if (IsPinn) return;
		IsPinn = true;
		if (EventTextCollider != null)
			EventTextCollider.enabled = false;
		StopCoroutine("RunPinnAnim");
		StopCoroutine("RunUnPinnAnim");
		StopCoroutine("RunShift");
		StartCoroutine("RunPinnAnim");
	}
	public virtual void PushBackItem()
	{
		if (!IsPinn) return;
		IsPinn = false;
		StopCoroutine("RunPinnAnim");
		StopCoroutine("RunUnPinnAnim");
		StopCoroutine("RunShift");
		StartCoroutine("RunUnPinnAnim");
	}
	public void UnPinnItem()
	{
		if (SelfPinn == null) return;
		SelfPinn.UnPinn();
	}
	IEnumerator RunUnPinnAnim()
	{
		if (SelfPinn == null) 
			yield break;
		IsRunning = true;
		//Shift all life index
		ShortCutMain.ShiftDown(index);
		//swap item to Left Side
		float shiftDist = ShiftCount * 0.6f;
		Vector3 currentPos = transform.localPosition;
		Vector3 targetPos = new Vector3(normalPosition.x,normalPosition.y + shiftDist,normalPosition.z);
		while(Vector3.Distance(currentPos,targetPos)>0.05f)
		{
			shiftDist = ShiftCount * 0.6f;
			targetPos.y = normalPosition.y + shiftDist;
			currentPos = Vector3.Lerp(currentPos,targetPos,Time.deltaTime*5);
			transform.localPosition = currentPos;
			yield return null;
		}
		transform.localPosition = targetPos;
		IsRunning = false;
		if (EventTextCollider != null)
			EventTextCollider.enabled = true;
	}

	IEnumerator RunPinnAnim()
	{
		if (SelfPinn == null) 
			yield break;
		IsRunning = true;
		//Shift all life index
		ShortCutMain.ShiftUp(index);
		PinnHeader.AddItem(SelfPinn);
		//swap item to Left Side
		Vector3 currentPos = transform.localPosition;
		Vector3 targetPos = new Vector3(-8,currentPos.y,0);
		while(Vector3.Distance(currentPos,targetPos)>0.05f)
		{
			targetPos.y = currentPos.y;
			currentPos = Vector3.Lerp(currentPos,targetPos,Time.deltaTime*5);
			transform.localPosition = currentPos;
			yield return null;
		}
		transform.localPosition = targetPos;
		IsRunning = false;
	}

	protected IEnumerator RunShift()
	{
		//calculate ShiftCount for new position
		float shiftDist = ShiftCount * 0.6f;
		Vector3 currentPos = transform.localPosition;
		Vector3 shiftPos = new Vector3(normalPosition.x,normalPosition.y + shiftDist,normalPosition.z);
		while (Vector3.Distance(currentPos,shiftPos)> 0.05f)
		{
			shiftDist = ShiftCount * 0.6f;
			shiftPos.y = normalPosition.y + shiftDist;
			currentPos = Vector3.Lerp(currentPos,shiftPos,Time.deltaTime*5);
			transform.localPosition = currentPos;
			yield return null;
		}
		transform.localPosition = shiftPos;
	}

	public virtual void Shift(int ct)
	{
		ShiftCount += ct;
		if (IsPinn) return;
		StopCoroutine("RunShift");
		StartCoroutine("RunShift");
	}

	public void SetIndex(int idx)
	{
		index = idx;
	}


	void Awake()
	{
		normalPosition = transform.localPosition;
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (MainMenuGlobal.getCurrentState()!=MainMenuState.MS_SHORTCUT&&
		    MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_PIN_RED) return;
		if (PopupObject.IsPopup) return;
		if (PinCollider == null)
			return;
		if (IsRunning) return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown)
		{
			if (PinCollider.OverlapPoint(touchPos))
			{
				if (MainMenuGlobal.getCurrentState()==MainMenuState.MS_TUTORIAL_PIN_RED)
				{
					MainMenuGlobal.Tutorial.NextTutorial();
				}
				//pinned item
				PinnItem();
			}
		}
	}
}
