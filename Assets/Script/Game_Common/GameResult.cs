using UnityEngine;
using System.Collections;
using TMPro;

public class GameResult : MonoBehaviour {
	public delegate void EventAction ();
	public event EventAction shareButton;
	public event EventAction replayButton;
	public event EventAction exitButton;

	public TextMeshPro ScoreText;
	public TextMeshPro ScoreBonusText;
	public GameObject HighScore;
	public OnlineResult OnlineRes;

	public GameObject Button;
	public GamePlayButton ReplayButton;
	public GamePlayButton ExitButton;
	public GamePlayButton ShareButton;

	public GameObject SwapOutPoint;

	public GameUpdateAPI updateAPI;

	private MorphObject ScoreMorph = new MorphObject ();
	private MorphObject ScoreBonusMorph= new MorphObject ();

	private string[] awardList;
	private int awardIdx;

	private Sprite GameIcon;
	private int Score;
	private int ScoreBonus;
	private int ScoreMax;
	private int moneyGain;
	private int comboMax;
	private int expGain;
	private int moneyUsed;
	private bool item1;
	private bool item2;
	private bool item3;
	private int usedTime;
	private int playHit;
	private string GameID;
	private bool FriendIsDone = false;



	public void SetGameID(string id)
	{
		GameID = id;
		Debug.Log ("GameResult SetGameID : "+GameID);
	}

	public void SetGameLogo(Sprite icon)
	{
		GameIcon = icon;
		OnlineRes.SetGameLogo(icon);
	}

	public void SetGameResultData(int score,int bonus,int combo,float money,float exp,int hit)
	{
		Score = score;
		ScoreBonus = bonus;
		ScoreMax = (int)(score * (1+bonus/100.0f));
		comboMax = combo;
		moneyGain = (int)money;
		expGain = (int)exp;
		playHit= hit;

//		OnlineRes.SetResult(data);
	}
	
	public void ResetResult(int startLevel,int startProgress,int startMoney,bool useitem1,bool useitem2,bool useitem3,int used,int time)
	{
		Score = 0;
		usedTime = time;
		ScoreBonus = 0;
		moneyGain = 0;
		expGain = 0;
		ScoreText.text = "0";
		ScoreBonusText.text = "0%";
		moneyUsed = used;
		item1 = useitem1;
		item2 = useitem2;
		item3 = useitem3;
		int currentMoney = startMoney;
						
		int currentLevel = startLevel;

		OnlineRes.SetStartResult (currentMoney,
		                          UserCommonData.pGlobal.GetEXPProgress(),
		                          currentLevel);

		HighScore.SetActive (false);
	}

	public void Show()
	{
		Button.SetActive(false);
		Button.transform.localScale = new Vector3(0,0,1);
		GetFriendList();
		StartCoroutine ("PopInMenu", new Vector3(0,0,0));
		StartCoroutine ("ResultRunning");
	}
	
	public void Hide()
	{
		StartCoroutine ("PopInMenu", SwapOutPoint.transform.position);
	}

	void Awake () {
		ReplayButton.OnReleased += ReplayAction;
		ShareButton.OnReleased += ShareAction;
	}

	// Use this for initialization
	void Start () {
		transform.localPosition = SwapOutPoint.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void ReplayAction()
	{
		Hide ();
		if (replayButton != null)
			replayButton();
	}

	void ShareAction()
	{
		//show share popup
		string path = "";
		if (GameID == "4")
		{
			path = "http://db.oryor.com/databank/oryor_mobile_api/icongame/Icon_mole_lesson.png";
		}
		else if (GameID == "3")
		{
			path = "http://db.oryor.com/databank/oryor_mobile_api/icongame/Icon_dropdrag.png";
		}
		else if (GameID == "2")
		{
			path = "http://db.oryor.com/databank/oryor_mobile_api/icongame/Icon_oryor_collector.png";
		}
		else if (GameID == "1")
		{
			path = "http://db.oryor.com/databank/oryor_mobile_api/icongame/Icon_gda_rush.png";
		}
		else if (GameID == "5")
		{
			path = "http://db.oryor.com/databank/oryor_mobile_api/icongame/Icon_oryor_school.png";
		}
		else if (GameID == "6")
		{
			path = "http://db.oryor.com/databank/oryor_mobile_api/icongame/Icon_bok_bok_bang.png";
		}
		//เราได้คะแนน x,xxx,xxx เกมสนุกมากๆ มาเล่นกับเราหน่อยน้า สนุก ได้ความรู้ และอาจได้ของรางวัลจาก อย. ด้วยนะ คลิ๊กเลย!
		string Desc = "เราได้คะแนน "+ Score.ToString("#,##0") + " เกมสนุกมากๆ มาเล่นกับเราหน่อยน้า สนุก ได้ความรู้ และอาจได้ของรางวัลจาก อย. ด้วยนะ คลิ๊กเลย!";
		#if UNITY_WINRT
		string link = "http://www.oryor.com/index.php/th/smart-application";
		UnityPluginForWindowsPhone.BridgeWP.shareFacebook("เพื่อนๆ มาเล่นกัน!","Game",Desc,link,path);
		#else
		FacebookLogin.pGlobal.PostFacebook(null,"เพื่อนๆ มาเล่นกัน!","Game",Desc,path);
		
		#endif
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

	IEnumerator ResultRunning()
	{
		ScoreMorph.morphEasein (0, Score, 60);
		ScoreBonusMorph.morphEasein(0, ScoreBonus,60);
		GetComponent<AudioSource>().Play();
		while ((!ScoreMorph.IsFinish())||(!ScoreBonusMorph.IsFinish())) {
			ScoreMorph.Update();
			ScoreBonusMorph.Update();
			int score = (int)ScoreMorph.val;
			int scorebonus = (int)ScoreBonusMorph.val;
			ScoreText.text = score.ToString("#,##0");
			ScoreBonusText.text = scorebonus.ToString()+"%";
			yield return null;
		}
		GetComponent<AudioSource>().Stop();
		yield return new WaitForSeconds(0.5f);
		if (ScoreBonus > 0)
		{
			ScoreMorph.morphEasein (Score + Score * ScoreBonus / 100, 60);
			GetComponent<AudioSource>().Play();
			while (!ScoreMorph.IsFinish()) {
				ScoreMorph.Update();
				int score = (int)ScoreMorph.val;
				ScoreText.text = score.ToString("#,##0");
				yield return null;
			}
			GetComponent<AudioSource>().Stop();
			yield return new WaitForSeconds (1.0f);
		}
		//load friend list
		StartCoroutine("ShowOnlineResult");
	}

	IEnumerator ShowOnlineResult()
	{
		while (!updateAPI.IsDone||!FriendIsDone)
		{
			//waiting
			yield return null;
		}
		//decrypt data done
		if (updateAPI.msg.msg == "OK")
		{
			//save data to user common
			int money = int.Parse(updateAPI.msg.user_money);
			int level = int.Parse(updateAPI.msg.user_level);

			UserCommonData.pGlobal.user.user_money = money.ToString();
			UserCommonData.pGlobal.user.user_level = level.ToString();
			UserCommonData.pGlobal.user.user_exp = updateAPI.msg.user_exp;
			UserCommonData.pGlobal.user.user_int_exp = updateAPI.msg.user_exp_init;
			UserCommonData.pGlobal.user.user_next_exp = updateAPI.msg.user_exp_next;
			UserCommonData.pGlobal.user.user_item1 = updateAPI.msg.user_item_heart_no.ToString();
			UserCommonData.pGlobal.user.user_item2 = updateAPI.msg.user_item_coin_no.ToString();
			UserCommonData.pGlobal.user.user_item3 = updateAPI.msg.user_item_score_no.ToString();
			UserCommonData.pGlobal.user.user_unlock_item1 = updateAPI.msg.user_unlock_item1;
			UserCommonData.pGlobal.user.user_unlock_item2 = updateAPI.msg.user_unlock_item2;
			UserCommonData.pGlobal.user.user_unlock_item3 = updateAPI.msg.user_unlock_item3;

			UserCommonData.pGlobal.Save();

			OnlineRes.SetResult(money,UserCommonData.pGlobal.GetEXPProgress(),level,
			                    updateAPI.msg.ini_game_rank,
			                    updateAPI.msg.update_game_rank,
			                    updateAPI.msg.ini_all_rank,
			                    updateAPI.msg.update_all_rank,
			                    updateAPI.msg.awardStringList != null);
			OnlineRes.ShowResult();
			while (!OnlineRes.IsDone);
			//show Button
			StartCoroutine("ShowButton");
			//add Award list to popup
			awardIdx = 0;
			awardList = updateAPI.msg.awardStringList;
			ShowAwardPopup();
		}
		else
		{
//			OnlineRes.ShowError();
			StartCoroutine("ShowButton");
		}

	}

	void ShowAwardPopup()
	{
		if (awardList == null) return;
		if (awardIdx < awardList.Length)
		{
		    PopupObject.ShowAlertPopup("Award",StringUtil.ParseUnicodeEscapes(awardList[awardIdx++]),"ปิด",ShowAwardPopup);
		}
	}

	IEnumerator ShowButton()
	{
		Button.SetActive(true);
		Vector3 targetScale = new Vector3(1,1,1);

		while (Vector3.Distance(Button.transform.localScale,targetScale) > 0.05f)
		{
			Button.transform.localScale = Vector3.Lerp(Button.transform.localScale,targetScale,Time.deltaTime*10);
			yield return null;
		}
		Button.transform.localScale = targetScale;
		yield return null;
	}

	void GetFriendList()
	{
		#if UNITY_WINRT
		UnityPluginForWindowsPhone.BridgeWP.friendsFacebook();
		#else
		FriendIsDone = false;
		FacebookLogin.pGlobal.LoadFriendList(GetFriendCB);
		#endif
	}
	public void callbackFriendsFacebook(string friendsId){
		string error = null;
		if (friendsId != null) {
			error = friendsId;
		} 
		
		GetFriendCB(error);
	}
	
	void GetFriendCB(string Error)
	{
		FriendIsDone = true;
		#if UNITY_WINRT 
		Debug.Log("getFrendCB");
		if(Error !=null){
			Loading (Error);
			
		}else{
			
			
			GetFriendClose();
		}
		
		#else
		if (Error == "OK")
		{
			FriendIsDone = true;
			int i = 0;
			string friendString = "";
			foreach(FB_UserInfo friend in FacebookLogin.pGlobal.mFriendList)
			{
				if (i != 0)
					friendString += ",";
				friendString += friend.Id;
				i++;
			}
			Loading (friendString);
		}
		else if (Error == "NOAUTH")
		{
			//no login
			//close and popup menu to ask to login
			GetFriendClose();
		}
		else
		{
			//Unknowed error
			PopupObject.ShowAlertPopup("Facebook Error","มีปัญหาจากการเชื่อมต่อกับ Facebook เชื่อมต่ออีกครั้งหรือไม่?",
			                           "ปิด",GetFriendClose,
			                           "ลองใหม่",GetFriendList);
		}
		#endif
	}

	void GetFriendClose()
	{
		FriendIsDone = true;
		Loading ("");
	}

	void Loading(string FriendString)
	{
		WWWForm form = new WWWForm();
		form.AddField ("type","gameUpdate");
		form.AddField("game_id",GameID);
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		form.AddField("score_max",ScoreMax);
		form.AddField("time_used",usedTime);
		form.AddField("hit",playHit);
		form.AddField("combo_max",comboMax);
		form.AddField("combo_count",playHit);
		form.AddField("exp",expGain);
		form.AddField("game_money",moneyGain);
		form.AddField("game_use",-moneyUsed);
		form.AddField("user_item1",(item1)?1:0);
		form.AddField("user_item2",(item2)?1:0);
		form.AddField("user_item3",(item3)?1:0);
		Debug.Log ("type:"+"gameUpdate");
		Debug.Log ("game_id:"+GameID);
		Debug.Log ("user_id:"+UserCommonData.pGlobal.user.user_id);
		Debug.Log ("score_max:"+ScoreMax);
		Debug.Log ("time_used:"+usedTime);
		Debug.Log ("hit:"+playHit);
		Debug.Log ("combo_max:"+comboMax);
		Debug.Log ("combo_count:"+playHit);
		Debug.Log ("exp:"+expGain);
		Debug.Log ("game_money:"+moneyGain);
		Debug.Log ("game_use:"+-moneyUsed);
		Debug.Log ("user_item1:0");
		Debug.Log ("user_item2:0");
		Debug.Log ("user_item3:0");
		updateAPI.wwwCallAPI(form);
	}
}
