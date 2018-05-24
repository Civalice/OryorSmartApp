using UnityEngine;
using System.Collections;

public class TutorialObject : MonoBehaviour {

	public TutorialPage[] pageList;
	private int currentIndex = 0;

	public Collider2D boxSkip;
	public GameObject skipObject;
	
	private bool IsTouch;
	
	Vector2 lastTouchPos;
	void Awake() {
		foreach(TutorialPage page in pageList)
		{
			page.gameObject.SetActive(false);
		}
	}

	public void StartTutorial()
	{
		currentIndex = 0;
		MainMenuGlobal.SetState(pageList[0].state);
		skipObject.SetActive (true);
		pageList[0].gameObject.SetActive(true);
		pageList[0].HighlightObject();
	}
	public void NextTutorial()
	{
		Debug.Log (MainMenuGlobal.getCurrentState());
		if (currentIndex < pageList.Length)
		{
			pageList[currentIndex].UnHightlightObject();

			pageList[currentIndex].gameObject.SetActive(false);

			currentIndex++;
		}
		if (currentIndex >= pageList.Length)
		{
			MainMenuGlobal.SetState(MainMenuState.MS_NORMAL);
			skipObject.SetActive (false);

			UserCommonData.IsTutorial = false;
			UserCommonData.pGlobal.Save();
			//out to menu state
		}
		else
		{
			MainMenuGlobal.SetState(pageList[currentIndex].state);
			pageList[currentIndex].gameObject.SetActive(true);
			pageList[currentIndex].HighlightObject();
		}
	}
	// Use this for initialization
	void Start () {

	}
	public void skipTutorial(){
		Debug.Log ("Skip Tutorial");
		pageList[currentIndex].UnHightlightObject();
		currentIndex = pageList.Length;
		skipObject.SetActive (false);
		Awake ();

		switch(MainMenuGlobal.getCurrentState())
		{
		case MainMenuState.MS_TUTORIAL_DRAG:
		{
		}break;
		case MainMenuState.MS_TUTORIAL_SHORTCUT:
		{
		}break;
		case MainMenuState.MS_TUTORIAL_PIN_RED:
		{ShortCutMain.pGlobal.TriggerShortcut ();
		}break;
		case MainMenuState.MS_TUTORIAL_UNPIN_RED:
		{			ShortCutMain.pGlobal.TriggerShortcut ();
		}break;
		case MainMenuState.MS_TUTORIAL_PIN_GREEN:
		{ShortCutMain.pGlobal.TriggerShortcut ();
		}break;
		case MainMenuState.MS_TUTORIAL_UNPIN_GREEN:
		{			ShortCutMain.pGlobal.TriggerShortcut ();
		}break;
		case MainMenuState.MS_TUTORIAL_CLOSE_SHORTCUT:
		{	ShortCutMain.pGlobal.TriggerShortcut ();
		}break;
		case MainMenuState.MS_TUTORIAL_SETTING:
		{
		}break;
		}
		NextTutorial ();
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (boxSkip.OverlapPoint(touchPos))
			{
				IsTouch = true;
				lastTouchPos = touchPos;
			}
		}
		if(touchedUp){
			if (boxSkip.OverlapPoint(touchPos))
			{
				if (IsTouch&&(Vector2.Distance(touchPos,lastTouchPos)<0.5f)) { 
					skipTutorial();
				}
			}
		}
		switch(MainMenuGlobal.getCurrentState())
		{
		case MainMenuState.MS_TUTORIAL_DRAG:
		{
			if (touchedDown)
			{
				pageList[currentIndex].gameObject.SetActive(false);
			}
			else if (touchedUp)
			{
				pageList[currentIndex].gameObject.SetActive(true);
			}
		}break;
		case MainMenuState.MS_TUTORIAL_SHORTCUT:
		{
		}break;
		case MainMenuState.MS_TUTORIAL_PIN_RED:
		{
		}break;
		case MainMenuState.MS_TUTORIAL_UNPIN_RED:
		{			
		}break;
		case MainMenuState.MS_TUTORIAL_PIN_GREEN:
		{
		}break;
		case MainMenuState.MS_TUTORIAL_UNPIN_GREEN:
		{			
		}break;
		case MainMenuState.MS_TUTORIAL_CLOSE_SHORTCUT:
		{	
		}break;
		case MainMenuState.MS_TUTORIAL_SETTING:
		{
			if (touchedUp)
			{
				NextTutorial();
			}
		}break;
		}

	}
}
