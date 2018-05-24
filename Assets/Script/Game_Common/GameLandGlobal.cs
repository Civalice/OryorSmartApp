using UnityEngine;
using System.Collections;

public enum GameLandState
{
	GS_MAPMODE = 0,
	GS_AWARD,
	GS_RANKING,
	GS_GIFT
}

public class GameLandGlobal : MonoBehaviour {
	public static GameLandGlobal pGlobal;

	public static GameLandState state = GameLandState.GS_MAPMODE;

	public static string giftMsg = "";
	public static bool IsCallGift = false;

	public AwardPage Award;
	public RankingPage Ranking;
	public GiftPage Gift;
	public MoneyGuage money;
	public EXPGuage exp;

	public GameObject btBack;

	public static void ShowAward()
	{
		state = GameLandState.GS_AWARD;
		pGlobal.Award.LoadAward();
		pGlobal.Award.gameObject.SetActive(true);
		pGlobal.Award.SwapIn();
	}

	public static void HideAward()
	{
		pGlobal.Award.SwapOut();
		state = GameLandState.GS_MAPMODE;
	}

	public static void ShowGift()
	{
		state = GameLandState.GS_GIFT;
		pGlobal.Gift.LoadGift();
		pGlobal.Gift.gameObject.SetActive(true);
		pGlobal.Gift.SwapIn();
	}

	public static void HideGift()
	{
		pGlobal.Gift.SwapOut();
		state = GameLandState.GS_MAPMODE;
	}

	public static void ShowRanking()
	{
		state = GameLandState.GS_RANKING;
		pGlobal.Ranking.LoadRank();
		pGlobal.Ranking.gameObject.SetActive(true);
		pGlobal.Ranking.SwapIn();
	}

	public static void HideRanking()
	{
		pGlobal.Ranking.SwapOut();
		state = GameLandState.GS_MAPMODE;
	}

	public void loadGiftCB(string msg)
	{
		giftMsg = msg;
		IsCallGift = true;
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		GiftSentGlobal.Initialize();
		#else
		GiftSentGlobal.Initialize(giftMsg);
		#endif
		GiftSentGlobal.UpdateGift();
	}

	public static void SaveCookie(string msg)
	{
		Application.ExternalCall("createCookieOryor",UserCommonData.imei,msg);
	}

	// Use this for initialization
	void Awake(){
//		pGlobal = this;
//		GiftSentGlobal.Initialize();
//		GiftSentGlobal.UpdateGift();
//		Award.gameObject.SetActive(true);
//		Ranking.gameObject.SetActive(true);
//		Gift.gameObject.SetActive(true);
//		money.SetStartMoney(int.Parse(UserCommonData.pGlobal.user.user_money));
//		exp.SetCurrentProgress(UserCommonData.pGlobal.GetEXPProgress(),int.Parse(UserCommonData.pGlobal.user.user_level));
//		state = GameLandState.GS_MAPMODE;
	}

	void Start () {
		pGlobal = this;

		#if !UNITY_WEBGL && !DISABLE_WEBVIEW 
		GiftSentGlobal.Initialize();
		GiftSentGlobal.UpdateGift();
		#elif UNITY_EDITOR
		#else
		Application.ExternalCall("checkGiftOryor",UserCommonData.imei);
		btBack.SetActive(false);
		#endif

		Award.gameObject.SetActive(true);
		Ranking.gameObject.SetActive(true);
		Gift.gameObject.SetActive(true);
		money.SetStartMoney(int.Parse(UserCommonData.pGlobal.user.user_money));
		exp.SetCurrentProgress(UserCommonData.pGlobal.GetEXPProgress(),int.Parse(UserCommonData.pGlobal.user.user_level));
		state = GameLandState.GS_MAPMODE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
