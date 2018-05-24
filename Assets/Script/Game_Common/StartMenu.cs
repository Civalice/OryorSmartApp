using UnityEngine;
using System.Collections;
using TMPro;

public class StartMenu : MonoBehaviour {
	public delegate void EventAction ();
	public event EventAction startButton;
	public event EventAction exitButton;
	public event EventAction TutorialEvent;
	
	public GameObject GameLogo;
	public TextMeshPro GameNameTxt;
	public TextMeshPro ItemDescTxt;
	public MoneyGuage Money;
	public GamePlayButton StartButton;
	public GamePlayButton TutorialButton;
	public TutorialContent tutorial;
	//variable for set in-game Properties
	public ItemFlag Item1;
	public ItemFlag Item2;
	public ItemFlag Item3;
	public ItemFlag Item4;

	public int moneyUsed = 0;

	public GameObject SwapOutPoint;

	public void SetGameLogo(Sprite icon,string name)
	{
		GameLogo.GetComponent<SpriteRenderer>().sprite = icon;
		GameNameTxt.text = name;
	}

	public void Show()
	{
		moneyUsed = 0;
		Money.SetStartMoney (int.Parse(UserCommonData.pGlobal.user.user_money));
		Item1.SetItemCount(int.Parse(UserCommonData.pGlobal.user.user_item1));
		Item2.SetItemCount(int.Parse(UserCommonData.pGlobal.user.user_item2));
		Item3.SetItemCount(int.Parse(UserCommonData.pGlobal.user.user_item3));
		Item1.SetLock(int.Parse(UserCommonData.pGlobal.user.user_unlock_item1)==0);
		Item2.SetLock(int.Parse(UserCommonData.pGlobal.user.user_unlock_item2)==0);
		Item3.SetLock(int.Parse(UserCommonData.pGlobal.user.user_unlock_item3)==0);
		Item4.SetLock(int.Parse(UserCommonData.pGlobal.user.user_hardmode)==0);
		Item1.UnSelect();
		Item2.UnSelect();
		Item3.UnSelect();
		Item4.UnSelect();
		StartCoroutine ("PopInMenu", new Vector3(0,0,0));
	}

	public void Hide()
	{
		StartCoroutine ("PopInMenu", SwapOutPoint.transform.position);
	}
	// Use this for initialization
	void Awake () {
		Item1.cb += setGameDescText;
		Item2.cb += setGameDescText;
		Item3.cb += setGameDescText;
		Item4.cb += setGameDescText;
		StartButton.OnReleased += StartAction;
		TutorialButton.OnReleased += TutorialAction;
		transform.localPosition = SwapOutPoint.transform.localPosition;
	}

	void Start() 
	{
		Show ();
	}

	// Update is called once per frame
	void Update () {
	}

	void StartAction()
	{
		if (Money.money >= 0)
		{
			Hide ();
			if (startButton != null)
				startButton();
		}
		else
		{
			//set Error text that you can't start game
		}
	}

	void TutorialAction()
	{
		tutorial.OpenTutorial();
	}

	IEnumerator PopInMenu(Vector3 pos)
	{
		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = pos;
		//lerp position
		while (Vector3.Distance(currentPosition,targetPosition) > 0.05f) {
			currentPosition = Vector3.Lerp(currentPosition,targetPosition,Time.deltaTime*5);
			transform.position = currentPosition;
			yield return null;
		}
		transform.position = targetPosition;
	}

	void setGameDescText(string txt,int price)
	{
		ItemDescTxt.text = txt;
		Money.ChangeMoney (price);
		moneyUsed += price;
	}
}
