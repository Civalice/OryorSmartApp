using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShortCutMain : MonoBehaviour {
	public Collider2D ShortCutCollider;
	public Collider2D TouchArea;

	public GameObject SwapInPts;
	public GameObject SwapOutPts;
	public GameObject DummyPosition;
	public GameObject ShortCutDetail;

	private Vector3 ShortCutCurrentPos;

	public MainMenuSound mSound;

	public List<ShortCutItem> NormalList;

	private bool IsOpen = false;
	private bool IsRunning = false;
	private Vector2 lastTouchPos;
	private bool IsDrag = false;

	public static ShortCutMain pGlobal;
	

	public static void ShiftUp(int idx)
	{
		for (int i = idx ;i < pGlobal.NormalList.Count;i++)
		{
			//call NormalList[i].ShiftUp
			pGlobal.NormalList[i].Shift(1);
		}
	}
	public static void ShiftDown(int idx)
	{
		for (int i = idx ;i < pGlobal.NormalList.Count;i++)
		{
			//call NormalList[i].ShiftUp
			pGlobal.NormalList[i].Shift(-1);
		}
	}

	public void TriggerShortcut()
	{
		IsOpen = !IsOpen;
		if (MainMenuGlobal.getCurrentState()==MainMenuState.MS_TUTORIAL_SHORTCUT)
		{
			MainMenuGlobal.Tutorial.NextTutorial();
		}
		else if (IsOpen)
		{
			MainMenuGlobal.SetState(MainMenuState.MS_SHORTCUT);
		}
		StartCoroutine("RunTrigger");
	}

	IEnumerator RunTrigger()
	{
		Vector3 swapPts = (IsOpen)?SwapInPts.transform.position:SwapOutPts.transform.position;
		IsRunning = true;
		while (Vector3.Distance(transform.position,swapPts) > 0.05f)
		{
			Vector3 newPosition = Vector3.Lerp(transform.position,swapPts,Time.deltaTime*10);
			transform.position = newPosition;
			yield return null;
		}
		transform.position = swapPts;
		IsRunning = false;
		if (MainMenuGlobal.getCurrentState()==MainMenuState.MS_TUTORIAL_CLOSE_SHORTCUT)
		{
			MainMenuGlobal.Tutorial.NextTutorial();
		}
		else if (!IsOpen)
			MainMenuGlobal.SetState(MainMenuState.MS_NORMAL);
	}

	void Awake()
	{
		pGlobal = this;
	}
	// Use this for initialization
	void Start () {
		for(int i = 0;i < NormalList.Count;i++)
		{
			NormalList[i].SetIndex(i);
		}
		if (MainMenuGlobal.getCurrentState() == MainMenuState.MS_SHORTCUT)
		{
			TriggerShortcut();
		}
		LoadShortcut();
	}

	void LoadShortcut()
	{
		int[] arr = UserCommonData.LoadShortcut();
		if (arr == null) return;
		foreach (int a in arr)
		{
			NormalList[a].PinnItem();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PopupObject.IsPopup) return;
		if (MainMenuGlobal.getCurrentState()!=MainMenuState.MS_NORMAL&&
		    MainMenuGlobal.getCurrentState()!=MainMenuState.MS_SHORTCUT&&
			MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_SHORTCUT&&
		    MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_CLOSE_SHORTCUT)
			return;
		if (ShortCutCollider == null)
			return;
		if (IsRunning) 
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown)
		{
			if (ShortCutCollider.OverlapPoint(touchPos))
			{
				//Trigger Shortcut
				mSound.playSound("click");
				TriggerShortcut();
			}
			else if ((MainMenuGlobal.getCurrentState()==MainMenuState.MS_SHORTCUT)&&TouchArea.OverlapPoint(touchPos))
			{
				//mSound.playSound("click");
				lastTouchPos = touchPos;
				ShortCutCurrentPos = DummyPosition.transform.localPosition;
				IsDrag = true;
			}
		}
		if (touchedUp)
		{
			IsDrag = false;
		}

		if (IsDrag)
		{
			float tempPosx = ShortCutCurrentPos.x;
			//calculate CurrentMapPos
			Vector3 movedPosition = ShortCutCurrentPos + new Vector3(0,
			                                                         touchPos.y - lastTouchPos.y,
			                                                    0);
			ShortCutCurrentPos = new Vector3(tempPosx,ShortCutCurrentPos.y,0);
			DummyPosition.GetComponent<Rigidbody2D>().velocity = (movedPosition-ShortCutCurrentPos)*60;
			lastTouchPos = touchPos;
		}
		else
		{
			DummyPosition.GetComponent<Rigidbody2D>().AddForce(-DummyPosition.GetComponent<Rigidbody2D>().velocity*5);
		}
		ShortCutDetail.transform.localPosition = DummyPosition.GetComponent<Rigidbody2D>().transform.localPosition;
	}
}
