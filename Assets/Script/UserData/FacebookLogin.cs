using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum FacebookState
{
	FB_NONE = 0,
	FB_INIT,
	FB_AUTH,
	FB_GET_USER_DATA
}

public class FacebookLogin : MonoBehaviour {
	public static FacebookLogin pGlobal;

	public string[] friendIDList;
	
	private static bool IsUserInfoLoaded = false;
	private static bool IsFrindsInfoLoaded = false;
	public static bool IsInit = false;
	
	private FacebookState state = FacebookState.FB_NONE;
	
	public FB_UserInfo myInfo;
	public FB_UserInfo[] mFriendList = null;
	
	public static bool IsLogin{
		get{ return UserCommonData.IsFBLogin;}
	}
	public static string UserID {
		get{return pGlobal.myInfo.Id;}
	}
	
	public void Initialize()
	{
		Debug.Log ("FacebookLogin Initialize !!!");
		pGlobal.Init();
	}
	
	public delegate void EventAction(string Error);
	public event EventAction PostLogin;
	public event EventAction GetFriend;
	public event EventAction ShareFB;

	public void SetFriendList(List<string> fList)
	{
		friendIDList = fList.ToArray();
	}
	
	public void Login(EventAction cb)
	{
		PostLogin = cb;
		if(SPFacebook.instance.IsLoggedIn) {
			//			OnAuth();
			Debug.Log ("Facebook already Login/Register : "+SPFacebook.instance.IsLoggedIn);
			LoadUserData();
		} 
		else
		{
			Debug.Log ("Facebook Login Process");
			SPFacebook.instance.Login();
//			FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, PostLogin);
		}
	}
	
	public void LoadUserData()
	{
//		SPFacebook.OnUserDataRequestCompleteAction  += OnUserDataLoaded;
		SPFacebook.instance.LoadUserData();
	}
	
	public void LoadFriendList(EventAction cb)
	{
		GetFriend = cb;
		mFriendList = null;
		if (SPFacebook.instance.IsLoggedIn) 
		{
			SPFacebook.instance.LoadFrientdsInfo(5000);
		}
		else
		{
			GetFriend("NOAUTH");
		}
	}
	
	public void PostFacebook(EventAction cb,string name,string Caption,string desc,string picturePath,string website = "http://www.oryor.com/oryor2015/brochure.php")
	{
		ShareFB = cb;
		SPFacebook.Instance.FeedShare (
			link: website,
			linkName: name,
			linkCaption: Caption,
			linkDescription: desc,
			picture: picturePath
			);
	}
	
	// Use this for initialization
	void Awake () {
		pGlobal = this;
		DontDestroyOnLoad(this);
	}
	
	void Start() {
	}
	
	public void Init()
	{
		state = FacebookState.FB_INIT;
		//Initialize()
		//		SPFacebook.instance.addEventListener(FacebookEvents.FACEBOOK_INITED, 			 OnInit);
		SPFacebook.OnInitCompleteAction += OnInit;
		SPFacebook.OnFocusChangedAction += OnFocusChanged;
		//Login()
		//		SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnAuth);
		//		SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_FAILED,OnFailedAuth);
		SPFacebook.OnAuthCompleteAction += OnAuth;
		//LoadUserData()
		//		SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_LOADED,  			OnUserDataLoaded);
		//		SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_FAILED_TO_LOAD,   OnUserDataLoadFailed);
		SPFacebook.OnUserDataRequestCompleteAction += OnUserDataLoaded;
		
		//		SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_DATA_LOADED,	OnFriendLoaded);
		//		SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_FAILED_TO_LOAD,	OnFriendLoadedFailed);
		SPFacebook.OnFriendsDataRequestCompleteAction += OnFriendLoaded;
		
		//		SPFacebook.instance.addEventListener(FacebookEvents.POST_SUCCEEDED, OnPost);
		//		SPFacebook.instance.addEventListener(FacebookEvents.POST_FAILED, OnPostFailed);
		SPFacebook.OnPostingCompleteAction += OnPost;
		
		SPFacebook.instance.Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnInit() {
		Debug.Log("Init Done");
		IsInit = true;
	}
	
	
	private void OnAuth(FB_Result result) {
		if(SPFacebook.instance.IsLoggedIn) {
			LoadUserData();
		} else {
			OnFailedAuth();
		}
		
	}
	
	private void OnFailedAuth()
	{
		Debug.Log("Fail Authen");
		if (PostLogin != null)
			PostLogin("UNKNOWED");
	}
	
	private void OnUserDataLoaded(FB_Result result) {
		SPFacebook.OnUserDataRequestCompleteAction -= OnUserDataLoaded;
		
		if (result.IsSucceeded) {
			
			SA_StatusBar.text = "User data loaded";
			IsUserInfoLoaded = true;
			myInfo = SPFacebook.instance.userInfo;
			Debug.Log("Email = "+myInfo.Email);
			Debug.Log(myInfo.ProfileUrl);
			if (PostLogin != null)
				PostLogin("OK");
			
		} else {
			OnUserDataLoadFailed();
		}
	}
	
	private void OnUserDataLoadFailed() {
		PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ",
		                           "ไม่สามารถตรวจสอบข้อมูลของท่านได้ กรุณาตรวจสอบอินเทอร์เน็ตของท่าน และลองใหม่อีกครั้ง",
		                           "ยกเลิก",null,
		                           "เชื่อมต่อใหม่",LoadUserData);
		
		Debug.Log("Load UserData failed.");
		if (PostLogin != null)
			PostLogin("UNKNOWED");
	}
	
	private void OnFriendLoaded(FB_Result res)
	{
		if (res.Error == null) {
			Debug.Log("You get Friend List");
			mFriendList = SPFacebook.instance.friendsList.ToArray();
			if (GetFriend != null)
				GetFriend("OK");
		} else {
			OnFriendLoadedFailed();
		}
	}
	
	private void OnFriendLoadedFailed()
	{
		Debug.Log("Error Get Friend List");
		if (GetFriend != null)
			GetFriend("UNKNOWED");
	}
	
	private void OnPost(FB_PostResult res)
	{
		if (res.IsSucceeded) {
			if (ShareFB != null)
				ShareFB ("OK");
		} else {
			OnPostFailed();
		}
	}
	
	private void OnPostFailed()
	{
		if (ShareFB != null)
			ShareFB("UNKNOWED");
	}
	private void OnFocusChanged(bool focus) {
		
		Debug.Log("FB OnFocusChanged: " + focus);
		  
	}
}
