using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplashScreenScript : MonoBehaviour {
	public UserAPIData api;
	bool IsChecked = false;
	
	public string lat;
	public string lon;

#if UNITY_EDITOR
	public bool IsDeleteData = true;
#endif
	// Use this for initialization
	void Start () {
		api.apiCB += CheckIMEICB;
		api.PostError += initApp;

#if UNITY_WINRT
		//PlayerPrefs.DeleteAll();
#elif !UNITY_WEBGL
		FacebookLogin.pGlobal.Initialize();
#endif

#if UNITY_EDITOR
		if (IsDeleteData)
			PlayerPrefs.DeleteAll();
#endif
//		StartCoroutine("InitializeApplication");
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		initApp();
		#else
		getLogin();
		#endif
	}

	void getLogin()
	{
		Debug.Log("getLogin");
		#if !UNITY_EDITOR
		Application.ExternalCall("request_userid",this.name);
		#else
		request_useridCB("{	\"user_id\":\"12\",\"friendlist\":[{\"id\":\"10152692953301052\",\"name\":\"Sumete Chitnumchai\"},{\"id\":\"10152347294498201\",\"name\":\"Civalice Untitled\"}],\"is_fb\":true}");
		#endif
	}

	public void request_useridCB(string param)
	{
		Debug.Log("request_useridCB : "+param);

		//decodeJSON
		JSONObject json = new JSONObject(param);
		Debug.Log("json");
		string user_id = json["user_id"].str;
		Debug.Log("user_id");
		UserCommonData.imei = user_id;
		Debug.Log("imei");
		JSONObject friendlist = json["friendlist"];
		Debug.Log("friendList");
		if (!friendlist.IsNull)
		{
			List<string> IdList = new List<string>();
			Debug.Log("IdList");

			foreach(JSONObject friend in friendlist.list)
			{
				string id = friend["id"].str;
				Debug.Log(id);
				IdList.Add(id);
			}
			FacebookLogin.pGlobal.SetFriendList(IdList);
		}
		bool is_fb = json["is_fb"].b;
		UserCommonData.IsFBLogin = is_fb;
		//call initApp
		initApp();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void initApp()
	{
		Debug.Log("Init App");
		UserCommonData.pGlobal.Load();
		PopupObject.instance.Button1.OnReleased = null;
		StopCoroutine("InitializeApplication");
		StartCoroutine("InitializeApplication");
	}

	IEnumerator InitializeApplication()
	{
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		//Check IMEI
		if (!UserCommonData.IsLogin)
		{
		UserCommonData.pGlobal.Load();
		IsChecked = false;
		WWWForm form = new WWWForm();
		form.AddField("type","getLogin");
		form.AddField("imei",UserCommonData.imei);
		api.wwwCallAPI(form);
		Debug.Log("apiCall "+UserCommonData.imei);
		while (!IsChecked)
		{
		yield return null;
		}
		if (api.msg.msg == "OK")
		{
		//pull all UserData here
		UserCommonData.SetUserData(api.msg.user[0]);
		LoadingScript.ChangeScene("MainMenu");
		}
		else
		{
		if ((api.IsError)&&(!UserCommonData.IsLogin))
		{
		//popup and break;
		PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ","กรุณาลองใหม่อีกครั้ง","ตกลง",initApp);
		yield break;
		}
		}
		}
		if (UserCommonData.IsSoundOn())
		{
		AudioListener.volume = 1.0f;
		}
		else
		{
		AudioListener.volume = 0.0f;
		}

		//		while(!FacebookLogin.IsInit)
		//		{
		//			yield return null;
		//		}
		//LoadingNewScene
		Debug.Log ("IsFacebookLogin : "+UserCommonData.IsFBLogin);
		LoadingScript.ChangeScene("MainMenu");
		#else
		//Check IMEI
		{

			IsChecked = false;
			//api.wwwCallAPI(form,false);
			string getForm = "&type=getLoginWeb&user_id="+UserCommonData.imei;
			api.wwwCallAPINoForm(getForm);
			Debug.Log("apiCall "+UserCommonData.imei);
			while (!IsChecked)
			{
				yield return null;
			}
			if (api.msg.msg == "OK")
			{
				//pull all UserData here
				UserCommonData.SetUserData(api.msg.user[0]);
			}
			else
			{
				if (api.IsError)
				{
					//popup and break;
					PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ","กรุณาลองใหม่อีกครั้ง","ตกลง",initApp);
					yield break;
				}
			}
		}
		LoadingScript.ChangeScene("GameLand");
		#endif
		yield return null;
	}

	void CheckIMEICB(APIMsg msg)
	{
		Debug.Log("CheckIMEICB : "+msg.msg);
		IsChecked = true;
	}
	IEnumerator autogetGPS() {
		if (!Input.location.isEnabledByUser)
			yield  return null;
		
		Input.location.Start();
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		if (maxWait < 1) {
			//print("Timed out");
			yield  return null;
		}
		if (Input.location.status == LocationServiceStatus.Failed) {
			//print("Unable to determine device location");
			yield  return null;
		} else {
			lat = Input.location.lastData.latitude.ToString();
			lon = Input.location.lastData.longitude.ToString();
			//print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}
		Input.location.Stop();
	}
}
