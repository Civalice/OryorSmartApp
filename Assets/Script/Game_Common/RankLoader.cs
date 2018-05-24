using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankLoader : MonoBehaviour {
	public RankingData[] rankingList;

	public string url;
	WWW loader;
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;

	public void LoadRank()
	{
		LoadingScript.ShowLoading();
		//get friend list then load rank from server
		GetFriendList();
	}

	void GetFriendList()
	{
		#if UNITY_WINRT
		
		//		if(UserCommonData.pGlobal.user.user_id ==null || UserCommonData.pGlobal.user.user_id==""){
		UnityPluginForWindowsPhone.BridgeWP.loginFacebookGetFriends();
		//		}else{
		//			UnityPluginForWindowsPhone.BridgeWP.friendsFacebookRanking ();
		//}
		#elif UNITY_WEBGL
		if (FacebookLogin.IsLogin)
			GetFriendCB("OK");
		else
			GetFriendCB("NOAUTH");
		#else
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
		#if UNITY_WINRT 
		
		if(Error !=null){
			StartCoroutine("Loading",Error);
			
		}else{
			
			LoadingScript.HideLoading();
			PopupObject.ShowAlertPopup("Facebook Error","ไม่สามารถนำรายชื่อเพื่อนจาก Facebook ได้ กรุณา Login ผ่าน Facebook ก่อน",
			                           "ปิด",CloseRanking,
			                           "Login",LoginFB,Error,null);
			
		}
		#elif UNITY_WEBGL
		if (Error == "OK")
		{
			int i = 0;
			string friendString = "";
			foreach(string friend in FacebookLogin.pGlobal.friendIDList)
			{
				if (i != 0)
					friendString += ",";
				friendString += friend;
				i++;
			}
			StartCoroutine("Loading",friendString);
		}
		else if (Error == "NOAUTH")
		{
			//no login
			//close and popup menu to ask to login
			LoadingScript.HideLoading();
			PopupObject.ShowAlertPopup("Facebook Error","ไม่สามารถนำรายชื่อเพื่อนจาก Facebook ได้ กรุณา Login ผ่าน Facebook ก่อน",
				"ปิด",CloseRanking,
				"Login",LoginFB);
		}
		else
		{
			//Unknowed error
			LoadingScript.HideLoading();
			//PopupObject.ShowAlertPopup("Facebook Error","มีปัญหาจากการเชื่อมต่อกับ Facebook",
			//                          "ปิด",);
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ",
				"ไม่สามารถตรวจสอบข้อมูลของท่านได้ กรุณาตรวจสอบอินเทอเน็ตของท่าน และลองใหม่อีกครั้ง",
				"ยกเลิก",CloseRanking,
				"เชื่อมต่อใหม่",LoadRank);
		}
		#else
		if (Error == "OK")
		{
			int i = 0;
			string friendString = "";
			foreach(FB_UserInfo friend in FacebookLogin.pGlobal.mFriendList)
			{
				if (i != 0)
					friendString += ",";
				friendString += friend.Id;
				i++;
			}
			StartCoroutine("Loading",friendString);
		}
		else if (Error == "NOAUTH")
		{
			//no login
			//close and popup menu to ask to login
			LoadingScript.HideLoading();
			PopupObject.ShowAlertPopup("Facebook Error","ไม่สามารถนำรายชื่อเพื่อนจาก Facebook ได้ กรุณา Login ผ่าน Facebook ก่อน",
			                           "ปิด",CloseRanking,
			                           "Login",LoginFB);
		}
		else
		{
			//Unknowed error
			LoadingScript.HideLoading();
			//PopupObject.ShowAlertPopup("Facebook Error","มีปัญหาจากการเชื่อมต่อกับ Facebook",
			 //                          "ปิด",);
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ",
			                           "ไม่สามารถตรวจสอบข้อมูลของท่านได้ กรุณาตรวจสอบอินเทอร์เน็ตของท่าน และลองใหม่อีกครั้ง",
			                           "ยกเลิก",CloseRanking,
			                           "เชื่อมต่อใหม่",LoadRank);
		}
		#endif
	}

	void CloseRanking()
	{
		GameLandGlobal.HideRanking();
	}

	void LoginFB()
	{
		#if UNITY_WINRT
		UnityPluginForWindowsPhone.BridgeWP.loginFacebookFriends();
		#else
		FacebookLogin.pGlobal.Login(PostLoginFB);
		#endif
	}
	
	public void callBackFacebookLoginfromWP8(string userId , string email , string firstName , string lastName){
		if (userId == null) {
			return;
		}
		
		UserCommonData.pGlobal.user.user_id = userId;
		PostLoginFB ("OK");
		
	}
	void PostLoginFB(string Error)
	{
		if (Error == "OK")
		{
			UserCommonData.IsFBLogin = true;
			UserCommonData.pGlobal.Save();
			PopupObject.ShowAlertPopup("Login Success","Login สำเร็จ",
			                           "ปิด",LoadRank);
		}
		else
		{
			PopupObject.ShowAlertPopup("Facebook Error","ไม่สามารถนำรายชื่อเพื่อนจาก Facebook ได้ กรุณา Login ผ่าน Facebook ก่อน",
			                           "ปิด",CloseRanking,
			                           "Login",LoginFB);		
		}
	}

	IEnumerator Loading(string friendString)
	{
		url = UserCommonData.GetURL () + url;
		WWWForm form = new WWWForm();
		//check ID = 7 for test
		Debug.Log("Friend String = "+friendString);
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		form.AddField("friendlist",friendString);
		loader = new WWW(url,form);
		yield return loader;
		if (loader.error != null)
		{
			Debug.Log("HTTP ERROR :"+loader.error);
			//popup Error
			LoadingScript.HideLoading();
			yield break;
		}
		Debug.Log(loader.text);
		if (loader.text == "") 
		{			
			LoadingScript.HideLoading();
			yield break;
		}

		JSONObject json = new JSONObject(loader.text);

		//Casting Data
		if (json["msg"].str == "OK")
		{
			JSONObject leaderboardList = json["leaderboard"];
			int i = 0;
			if (leaderboardList.list != null)
			{
				rankingList = new RankingData[leaderboardList.list.Count];
				foreach(JSONObject rank in leaderboardList.list)
				{
					RankingData data = new RankingData();
					data.userId = rank["user_id"].str;
					data.path = rank["user_picture"].str;
					data.user_name = rank["user_name"].str;
					data.scoreAll = int.Parse(rank["score_all"].str);
					data.score[0] = int.Parse(rank["score_each_game"]["4"].str);
					data.score[1] = int.Parse(rank["score_each_game"]["1"].str);
					data.score[2] = int.Parse(rank["score_each_game"]["3"].str);
					data.score[3] = int.Parse(rank["score_each_game"]["2"].str);
					data.score[4] = int.Parse(rank["score_each_game"]["5"].str);
					data.score[5] = int.Parse(rank["score_each_game"]["6"].str);
					rankingList[i] = data;
					i++;
				}
			}
		}

		isFinish = true;
		if (postDownloaded != null)
			postDownloaded();
		LoadingScript.HideLoading();
	}
	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
