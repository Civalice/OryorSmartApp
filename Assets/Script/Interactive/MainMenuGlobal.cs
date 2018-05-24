using UnityEngine;
using System.Collections;

public enum MainMenuState
{
	MS_NORMAL = 0,
	MS_SHORTCUT,
	MS_HELP,
	MS_LOGIN,
	MS_SETTING,
	MS_NEWSBOARD,
	MS_DRAG,

	MS_TUTORIAL_DRAG,
	MS_TUTORIAL_SHORTCUT,
	MS_TUTORIAL_PIN_RED,
	MS_TUTORIAL_UNPIN_RED,
	MS_TUTORIAL_PIN_GREEN,
	MS_TUTORIAL_UNPIN_GREEN,
	MS_TUTORIAL_CLOSE_SHORTCUT,
	MS_TUTORIAL_SETTING
};

public class MainMenuGlobal : MonoBehaviour {
	public static MainMenuGlobal pGlobal;
	public static MainMenuState lastState;
	public static bool IsOpenNews = false;
	public static int lastIndex = 0;
	public static TutorialObject Tutorial{
		get{return pGlobal.TutorialObj;}
	}

	public GameObject LoginObject;
	public GameObject SettingObject;
	public GameObject HelpObject;
	public GameObject NewsboardObject;
	public TutorialObject TutorialObj;

	public MainMenuState state = MainMenuState.MS_NORMAL;

	public static void StoreLastestState()
	{
		lastState = pGlobal.state;
	}

	public static MainMenuState getCurrentState()
	{
		return pGlobal.state;
	}

	public static void SetState(MainMenuState _st)
	{
		pGlobal.state = _st;
	}

	public static void SetLoginObject(bool flag)
	{
		if (flag)
		{
			SetState(MainMenuState.MS_LOGIN);
		}
		else
		{
			//check state first that it should be close or not?

			if (!IsOpenNews){
				SetNewsObject(true);
				IsOpenNews = true;
			}
			else
				SetState(MainMenuState.MS_NORMAL);
		}
		Debug.Log ("LogIn Flag : "+flag);
		pGlobal.LoginObject.SetActive(flag);
		//update LoginObject
	}

	public static void SetSettingObject(bool flag)
	{
		if (flag)
		{
			SetState(MainMenuState.MS_SETTING);
		}
		else
		{
				SetState(MainMenuState.MS_NORMAL);
		}
		Debug.Log ("Setting Open");
		pGlobal.SettingObject.SetActive(flag);
		Debug.Log ("Setting Opened");
	}

	public static void SetHelpObject(bool flag)
	{
		if (flag)
		{
			SetState(MainMenuState.MS_HELP);
		}
		else
		{
			SetState(MainMenuState.MS_NORMAL);
		}
		pGlobal.HelpObject.SetActive(flag);
	}
	
	public static void SetNewsObject(bool flag)
	{
		if (flag)
		{
			SetState(MainMenuState.MS_NEWSBOARD);
		}
		else
		{
			if (UserCommonData.IsTutorial)
				Tutorial.StartTutorial();
			else
				SetState(MainMenuState.MS_NORMAL);
		}
		pGlobal.NewsboardObject.SetActive(flag);
	}

	void Awake()
	{
		pGlobal = this;
	}

	// Use this for initialization
	void Start () {
		
		Debug.Log ("Start MainMenu Global : "+UserCommonData.IsLogin+" "+IsOpenNews);
		if (!UserCommonData.IsLogin)
		{
			SetLoginObject(true);
		}
		else if (!IsOpenNews){
			SetNewsObject(true);
			IsOpenNews = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
