using UnityEngine;
using System.Collections;
using TMPro;

public class LoginPageControl : MonoBehaviour {
	public ButtonObject btLogin;
	public ButtonObject btLoginFB;
	public ButtonObject btRegister;
	public ButtonObject btGuest;
	
	public ButtonObject btRegisterConfirm;
	public ButtonObject btRegisterCancel;
	public ButtonObject btUpdateConfirm;
	public ButtonObject btUpdateCancel;

	public UserAPIData GuestLoginAPI;
	public UserAPIData NormalLoginAPI;
	public UserAPIData NormalRegisAPI;
	public UserAPIData FBLoginAPI;
	public UserAPIData FBUpdateAPI;
	public UserAPIData UpdateAPI;

	public TextMeshPro username;
	public TextMeshPro password;

	public ProfilePage RegisterData;
	public ProfilePage UpdateData;

	public GameObject LoginPage;
	public GameObject RegisterPage;
	public GameObject UpdatePage;
	public GameObject LoginBottom;
	public GameObject RegisterBottom;
	public GameObject UpdateBottom;

	public ButtonObject btLoginClose;
	public ButtonObject btRegisterClose;
	public ButtonObject btUpdateClose;

	// Use this for initialization
	void Start () {
		GuestLoginAPI.apiCB += GuestLoginCB;
		NormalLoginAPI.apiCB += NormalLoginCB;
		NormalRegisAPI.apiCB += NormalRegisterCB;
		UpdateAPI.apiCB += UpdateCB;
		FBLoginAPI.apiCB += FBLoginCB;

		GuestLoginAPI.PostError += PostError;
		NormalLoginAPI.PostError += PostError;
		NormalRegisAPI.PostError += PostError;
		UpdateAPI.PostError += PostError;
		FBLoginAPI.PostError += PostError;

		btRegister.OnReleased += MemberRegister;
		btLogin.OnReleased += UserLoginProcess;
		btRegisterCancel.OnReleased += MainLogin;

		btGuest.OnReleased += GuestLoginProcess;
		btLoginFB.OnReleased += FacebookLoginProcess;
		btRegisterConfirm.OnReleased += Register;
		btUpdateCancel.OnReleased += Reset;

		btUpdateConfirm.OnReleased += UpdateProcess;

		btLoginClose.OnReleased += LoginCloseCB;
		btRegisterClose.OnReleased += RegisterCloseCB;
		btUpdateClose.OnReleased += UpdateCloseCB;
		MainLogin();
	}

	void PostError()
	{
		LoadingScript.HideLoading();
	}

	public void FacebookLoginProcess()
	{
		#if UNITY_WINRT
		UnityPluginForWindowsPhone.BridgeWP.loginFacebook();
		#else
		Debug.Log("FacebookLoginProcess");
		FacebookLogin.pGlobal.Login(PostLogin);
		
		#endif
		LoadingScript.ShowLoading();
	}

	public void GuestLoginProcess()
	{
		LoadingScript.ShowLoading();
		WWWForm form = new WWWForm();
		form.AddField("type","userRegisterGuest");
		form.AddField("imei",UserCommonData.imei);
		GuestLoginAPI.wwwCallAPI(form);
	}

	public void UserLoginProcess()
	{
		LoadingScript.ShowLoading();
		WWWForm form = new WWWForm();
		form.AddField("type","userLoginOryor");
		form.AddField("username",username.text);
		form.AddField("password",password.text);
		form.AddField("imei",UserCommonData.imei);
		NormalLoginAPI.wwwCallAPI(form);
	}

	public void UpdateProcess()
	{
		LoadingScript.ShowLoading();
		WWWForm form = new WWWForm();		
		//		password", "imei", "name", "surname" , "tel", "address
		form.AddField("type","userUpdate");
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		form.AddField("username",UpdateData.username);
		form.AddField("password",UpdateData.password);
		form.AddField("name",UpdateData.name);
		form.AddField("surname",UpdateData.surname);
		form.AddField("tel",UpdateData.tel);
		form.AddField("address",UpdateData.address);
		form.AddField("imei",UserCommonData.imei);
		UpdateAPI.wwwCallAPI(form);
	}

	void NormalLoginCB(APIMsg msg)
	{
		Debug.Log(msg.msg);
		if (msg.msg == "OK")
		{
			UserCommonData.SetUserData(msg.user[0]);
			userLogin();
			LoadingScript.HideLoading();
			MainMenuGlobal.SetLoginObject(false);
		}
		else
		{
			//popup LoginFail
			LoadingScript.HideLoading();
		}
	}
	public class WP8FBUser
	{
		public string userId;
		public string email;
		public string firstName;
		public string lastName;
	}
	private WP8FBUser mWP8FBUser = new WP8FBUser();
	public void callBackFacebookLoginfromWP8(string userId , string email , string firstName , string lastName){
		mWP8FBUser.userId = userId;
		mWP8FBUser.email = email;
		mWP8FBUser.firstName = firstName;
		mWP8FBUser.lastName = lastName;
		
		LoginProcess ();
		
	}
	void FBLoginCB(APIMsg msg)
	{
		Debug.Log(msg.msg);
		facebookLogin();
		//get msg to userCommon
		UserCommonData.SetUserData(msg.user[0]);
		UpdateDataFormFB();
		ResetUpdateDataForm();
		ResetRegisterDataForm();

		LoadingScript.HideLoading();
	}

	void FBUpdateCB(APIMsg msg)
	{
		facebookLogin();
		UpdateDataFormFB();
		ResetUpdateDataForm();
		ResetRegisterDataForm();
		LoadingScript.HideLoading();
	}

	void UpdateCB(APIMsg msg)
	{
		//done update
		//check msg first
		if (msg.msg == "OK")
		{
			//update UserCommonData and save
			UserCommonData.pGlobal.user = msg.user[0];
			UserCommonData.pGlobal.Save();
		}
		else
		{
			//update error
			PopupObject.ShowAlertPopup("Update Error","ไม่สามารถปรับปรุงข้อมูลได้","ปิด");
			//reset data
			ResetUpdateDataForm();
		}
		LoadingScript.HideLoading();
	}

	void UpdateDataFormFB()
	{
		#if UNITY_WINRT
		if(UserCommonData.pGlobal.user == null){
			UserCommonData.pGlobal.user  = new UserData();
			
		}
		
		UserCommonData.pGlobal.user.user_email = mWP8FBUser.email;
		UserCommonData.pGlobal.user.user_firstname = mWP8FBUser.firstName;
		UserCommonData.pGlobal.user.user_surname = mWP8FBUser.lastName;
		#else
		Debug.Log("Facebook Email = "+FacebookLogin.pGlobal.myInfo.Email);
		UserCommonData.pGlobal.user.user_email = FacebookLogin.pGlobal.myInfo.Email;
		UserCommonData.pGlobal.user.user_firstname = FacebookLogin.pGlobal.myInfo.FirstName;
		UserCommonData.pGlobal.user.user_surname = FacebookLogin.pGlobal.myInfo.LastName;
		#endif

	}
	void ResetUpdateDataForm()
	{
		UpdateData.username = UserCommonData.pGlobal.user.user_email;
		UpdateData.name = UserCommonData.pGlobal.user.user_firstname;
		UpdateData.surname = UserCommonData.pGlobal.user.user_surname;
		UpdateData.tel = UserCommonData.pGlobal.user.user_tel;
		UpdateData.address = UserCommonData.pGlobal.user.user_address;
	}

	void ResetRegisterDataForm()
	{
		RegisterData.username = UserCommonData.pGlobal.user.user_email;
		RegisterData.password = "";
		RegisterData.name = UserCommonData.pGlobal.user.user_firstname;
		RegisterData.surname = UserCommonData.pGlobal.user.user_surname;
		RegisterData.tel = UserCommonData.pGlobal.user.user_tel;
		RegisterData.address = UserCommonData.pGlobal.user.user_address;
	}

	void NormalRegisterCB(APIMsg msg)
	{
		if (msg.msg == "OK")
		{
			UserCommonData.SetUserData(msg.user[0]);
			ResetUpdateDataForm ();
			ResetRegisterDataForm ();
			LoadingScript.HideLoading();
			RegisterDone();
		}
		else
		{
			Debug.Log(msg.msg);
			//popup Register Fail by xxxxx
			LoadingScript.HideLoading();
		}
	}
	void GuestLoginCB(APIMsg msg)
	{
		UserCommonData.SetUserData(msg.user[0]);
		GuestLogin();
		LoadingScript.HideLoading();
	}

	void LoginCloseCB()
	{
		GuestLoginProcess();
	}

	void RegisterCloseCB()
	{
		LoginPage.SetActive(true);
		LoginBottom.SetActive(true);
		UpdatePage.SetActive(false);
		UpdateBottom.SetActive(false);
		RegisterPage.SetActive(false);
		RegisterBottom.SetActive(false);
		btLoginFB.gameObject.SetActive(true);
	}

	void UpdateCloseCB()
	{
		MainMenuGlobal.SetLoginObject(false);
	}

	// Update is called once per frame
	void Update () {
	}

	public void userLogin(){
		Debug.Log("User Login");

		LoginPage.SetActive(false);
		LoginBottom.SetActive(false);
		UpdatePage.SetActive(true);
		UpdateBottom.SetActive(true);

		if (FacebookLogin.IsLogin)
			btLoginFB.gameObject.SetActive(false);
		else
			btLoginFB.gameObject.SetActive(true);
	}

	public void MainLogin()
	{
		ResetRegisterDataForm();

		ResetUpdateDataForm();

		if (UserCommonData.IsLogin)
		{
			LoginPage.SetActive(false);
			LoginBottom.SetActive(false);
			UpdatePage.SetActive(true);
			UpdateBottom.SetActive(true);
		}
		else
		{
			LoginPage.SetActive(true);
			LoginBottom.SetActive(true);
			UpdatePage.SetActive(false);
			UpdateBottom.SetActive(false);
		}
		RegisterPage.SetActive(false);
		RegisterBottom.SetActive(false);

		if (FacebookLogin.IsLogin)
			btLoginFB.gameObject.SetActive(false);
		else
			btLoginFB.gameObject.SetActive(true);
	}

	public void facebookLogin(){
		Debug.Log("Facebook Login");

		LoginPage.SetActive(false);
		LoginBottom.SetActive(false);
		UpdatePage.SetActive(true);
		UpdateBottom.SetActive(true);
		btLoginFB.gameObject.SetActive(false);
		MainMenuGlobal.SetLoginObject(false);
	}
	public void MemberRegister(){
		Debug.Log("Member Register");

		LoginPage.SetActive(false);
		LoginBottom.SetActive(false);
		RegisterPage.SetActive(true);
		RegisterBottom.SetActive(true);
		btLoginFB.gameObject.SetActive(false);
	}
	public void GuestLogin(){
		Debug.Log("Guest Login");

		LoginPage.SetActive(false);
		LoginBottom.SetActive(false);
		UpdatePage.SetActive(true);
		UpdateBottom.SetActive(true);
		if (FacebookLogin.IsLogin)
			btLoginFB.gameObject.SetActive(false);
		else
			btLoginFB.gameObject.SetActive(true);
		MainMenuGlobal.SetLoginObject(false);
	}

	public void Register()
	{
		//goto update after register
		LoadingScript.ShowLoading();
		WWWForm form = new WWWForm();
		form.AddField("type","userRegisterOryor");
		form.AddField("username",RegisterData.username);
		form.AddField("password",RegisterData.password);
		form.AddField("name",RegisterData.name);
		form.AddField("surname",RegisterData.surname);
		form.AddField("tel",RegisterData.tel);
		form.AddField("address",RegisterData.address);
		form.AddField("imei",UserCommonData.imei);
		NormalRegisAPI.wwwCallAPI(form);
	}
	public void Reset()
	{
		
		ResetRegisterDataForm();
		
		ResetUpdateDataForm();
		
		MainMenuGlobal.SetLoginObject(false);
	}
	public void RegisterDone()
	{
		LoginPage.SetActive(false);
		LoginBottom.SetActive(false);
		UpdatePage.SetActive(true);
		UpdateBottom.SetActive(true);
		if (FacebookLogin.IsLogin)
			btLoginFB.gameObject.SetActive(false);
		else
			btLoginFB.gameObject.SetActive(true);
		MainMenuGlobal.SetLoginObject(false);
	}

	void PostLogin(string error)
	{
		Debug.Log ("LoginPageControl PostLogin : "+error);
		if (error != "OK")
		{
			PopupObject.ShowAlertPopup("Login Error","เกิดข้อผิดพลาดในการ Login ด้วย Facebook","ปิด");
			LoadingScript.HideLoading();
		}
		else
		{
			LoginProcess();
		}
	}

	void LoginProcess()
	{
		Debug.Log("Start LoginProcess");
		StopCoroutine("FBLoginAPIProcess");
		StartCoroutine("FBLoginAPIProcess");
	}

	IEnumerator FBLoginAPIProcess()
	{
		if (!UserCommonData.IsLogin)
		{
			#if UNITY_WINRT
			
			
			WWWForm form = new WWWForm();
			form.AddField("type","userRegisterWithFB");
			form.AddField("fbid",mWP8FBUser.userId);
			form.AddField("imei",UserCommonData.imei);
			form.AddField("username",mWP8FBUser.email);
			form.AddField("name",mWP8FBUser.firstName);
			form.AddField("surname",mWP8FBUser.lastName);
			form.AddField("tel","");
			form.AddField("address","");
			FBLoginAPI.wwwCallAPI(form);
			#else
			Debug.Log("Register FB : "+FacebookLogin.UserID);
			//			Debug.Log("User Email : "+FacebookLogin.pGlobal.myInfo.email);
			//register with FB
			WWWForm form = new WWWForm();
			form.AddField("type","userRegisterWithFB");
			form.AddField("fbid",FacebookLogin.UserID);
			form.AddField("imei",UserCommonData.imei);
			form.AddField("username",FacebookLogin.pGlobal.myInfo.Email);
			form.AddField("name",FacebookLogin.pGlobal.myInfo.FirstName);
			form.AddField("surname",FacebookLogin.pGlobal.myInfo.LastName);
			form.AddField("tel","");
			form.AddField("address","");
			FBLoginAPI.wwwCallAPI(form);
			
			#endif


//			UpdateDataFormFB();
//			ResetUpdateDataForm();
//			ResetRegisterDataForm();
			while(!FBLoginAPI.IsDone)
			{
//				LoadingScript.HideLoading();
				yield return null;
			}
			Debug.Log("Done Login");
			//decrypt all data to UserCommonData
			if (FBLoginAPI.IsError)
			{
				LoadingScript.HideLoading();
				PopupObject.ShowAlertPopup("Server Error","ไม่สามารถเชื่อมต่อกับ Server อย. ได้",
				                           "ลองใหม่",LoginProcess);
				yield break;
			}
			else if (FBLoginAPI.msg.msg != "OK")
			{
				LoadingScript.HideLoading();
				PopupObject.ShowAlertPopup("Server Error","มีข้อผิดพลาดกับการส่งข้อมูลกับทาง Server",
				                           "ปิด");
//				PopupObject.ShowAlertPopup(FacebookLogin.UserID,UserCommonData.imei+" "+FacebookLogin.pGlobal.myInfo.email,
//				                           "ปิด");
				yield break;
			}
			else
			{
				Debug.Log("No Error Register Facebook");
				UserCommonData.pGlobal.user = FBLoginAPI.msg.user[0];
				UserCommonData.IsLogin = true;
				UserCommonData.IsFBLogin = true;
				if (FacebookLogin.IsLogin)
					btLoginFB.gameObject.SetActive(false);
				else
					btLoginFB.gameObject.SetActive(true);
				//setup User here
				UpdateDataFormFB();
				ResetUpdateDataForm();
				ResetRegisterDataForm();
				UserCommonData.pGlobal.Save();
				LoadingScript.HideLoading();
			}
		}
		else
		{
			#if UNITY_WINRT
			WWWForm form = new WWWForm();
			form.AddField("type","userUpdate");
			form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
			form.AddField("fbid",mWP8FBUser.userId);
			form.AddField("imei",UserCommonData.imei);
			form.AddField("username",mWP8FBUser.email);
			form.AddField("name",mWP8FBUser.firstName);
			form.AddField("surname",mWP8FBUser.lastName);
			form.AddField("tel",UserCommonData.pGlobal.user.user_tel);
			form.AddField("address",UserCommonData.pGlobal.user.user_address);
			FBUpdateAPI.wwwCallAPI(form);
			#else
			Debug.Log("Update FB : "+FacebookLogin.UserID);
			//update with FB
			WWWForm form = new WWWForm();
			form.AddField("type","userUpdate");
			form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
			form.AddField("fbid",FacebookLogin.UserID);
			form.AddField("imei",UserCommonData.imei);
			form.AddField("username",FacebookLogin.pGlobal.myInfo.Email);
			form.AddField("name",FacebookLogin.pGlobal.myInfo.FirstName);
			form.AddField("surname",FacebookLogin.pGlobal.myInfo.LastName);
			form.AddField("tel",UserCommonData.pGlobal.user.user_tel);
			form.AddField("address",UserCommonData.pGlobal.user.user_address);
			FBUpdateAPI.wwwCallAPI(form);
			#endif


//			UpdateDataFormFB();
//			ResetUpdateDataForm();
//			ResetRegisterDataForm();
			while(!FBUpdateAPI.IsDone)
			{
				yield return null;
			}
			//decrypt all data to UserCommonData
			if (FBUpdateAPI.IsError)
			{
				LoadingScript.HideLoading();
				PopupObject.ShowAlertPopup("Server Error","ไม่สามารถเชื่อมต่อกับ Server อย. ได้",
				                           "ลองใหม่",LoginProcess);
				yield break;
			}
			else if (FBUpdateAPI.msg.msg != "OK")
			{
				LoadingScript.HideLoading();
				PopupObject.ShowAlertPopup("Server Error","มีข้อผิดพลาดกับการส่งข้อมูลกับทาง Server",
				                           "ปิด");
//				PopupObject.ShowAlertPopup(FacebookLogin.UserID,UserCommonData.imei+" "+FacebookLogin.pGlobal.myInfo.email+" "+FBUpdateAPI.msg.msg,
//				                           "ปิด");
				yield break;
			}
			else
			{
				UserCommonData.pGlobal.user = FBUpdateAPI.msg.user[0];
				UserCommonData.IsLogin = true;
				UserCommonData.IsFBLogin = true;
				if (FacebookLogin.IsLogin)
					btLoginFB.gameObject.SetActive(false);
				else
					btLoginFB.gameObject.SetActive(true);
				//setup User here
				UpdateDataFormFB();
				ResetUpdateDataForm();
				ResetRegisterDataForm();
				UserCommonData.pGlobal.Save();
				LoadingScript.HideLoading();
			}

//			LoadingScript.HideLoading();
			
		}
		ResetUpdateDataForm ();
		ResetRegisterDataForm ();
	}
	
}
