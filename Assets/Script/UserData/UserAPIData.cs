using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct APIMsg
{
	public string msg;
	public UserData[] user;
};
[System.Serializable]

public class UserData
{
	public string user_id{
		get{
			return mUsrID;
		}
		set{
			mUsrID = value;
		}
	}
	private string mUsrID;
	public string user_address;
	public string user_picture;
	public string user_email;
	public string user_firstname;
	public string user_surname;
	public string user_location;
	public string user_tel;

	public string user_money;
	public string user_level;

	public string user_item1;
	public string user_item2;
	public string user_item3;

	public string user_int_exp;
	public string user_exp;
	public string user_next_exp;

	public string user_exp_plus;
	public string user_coin_plus;
	public string user_score_plus;

	public string user_hardmode;
	public string user_unlock_item1;
	public string user_unlock_item2;
	public string user_unlock_item3;

	public string user_date_regis;
	public string user_date_update;
	
	public string user_schoolactivity;
	public string user_oryoractivity;
};

public class UserAPIData : MonoBehaviour {
	public APIMsg msg;
	public delegate void ErrorCB();
	public event ErrorCB PostError;
	public delegate void APICallback(APIMsg msg);
	public event APICallback apiCB;
	private string baseURL;
	private string page = "?task=getLogInfo";
	public bool IsDone = true;
	public bool IsError = false;
	// Use this for initialization
	void Start () {

	}

	public void wwwCallAPI(WWWForm form)
	{
		IsDone = false;
		IsError = false;
		StartCoroutine("ICallAPI",form);
	}
	public void wwwCallAPINoForm(string getForm)
	{
		IsDone = false;
		IsError = false;
		StartCoroutine("ICallAPINoForm",getForm);
	}

	void errorMethod()
	{
		if (PostError != null)
			PostError();
	}

	IEnumerator ICallAPINoForm(string getForm)
	{
		//create postform
		baseURL = UserCommonData.GetURL ();
		WWW api;
		api = new WWW(baseURL+page+getForm);
		Debug.Log("Call API NO Form : "+api.url);
		while (!api.isDone)
		{
			//waiting
			yield return null;
		}
		IsDone = true;
		if (api.error != null)
		{
			IsError = true;
			Debug.Log("Error : "+api.error);
			//			PopupObject.ShowAlertPopup("Error",api.error,"ปิด",errorMethod);
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ","กรุณาเชื่อมต่อ  อินเทอเน็ตเพื่อเข้าใช้งาน  Oryor  Smart  Application","เชื่อมต่อ",errorMethod);
			yield break;
		}
		//decrypt msg
		Debug.Log(api.text);
		JSONObject json = new JSONObject(api.text);
		if (json["msg"] != null)
		{
			msg.msg = json["msg"].str;
			if (msg.msg == "OK")
			{
				JSONObject userArr = json["user"];
				if (userArr != null)
				{
					List<UserData> userList = new List<UserData>();
					foreach(JSONObject user in userArr.list)
					{
						UserData dat = new UserData();
						dat.user_id = user["user_id"].str;
						dat.user_address = user["user_address"].str;
						dat.user_picture = user["user_picture"].str;
						dat.user_email = user["user_email"].str;
						dat.user_firstname = user["user_firstname"].str;
						dat.user_surname = user["user_surname"].str;
						dat.user_location = user["user_location"].str;
						dat.user_tel = user["user_tel"].str;

						dat.user_money = user["user_money"].str;

						dat.user_level = user["user_level"].str;
						//naming notice 
						//coin means item plus score
						//score means item plus exp
						//FUCK THE NAMING SERVER!!!
						dat.user_item1 = user["user_item_heart_no"].str;
						dat.user_item2 = user["user_item_coin_no"].str;
						dat.user_item3 = user["user_item_score_no"].str;

						dat.user_int_exp = user["user_int_exp"].str;
						dat.user_exp = user["user_exp"].str;
						dat.user_next_exp = user["user_next_exp"].str;

						dat.user_exp_plus = user["user_exp_plus"].str;
						dat.user_coin_plus = user["user_coin_plus"].str;
						dat.user_score_plus = user["user_score_plus"].str;
						dat.user_hardmode = user["user_hardmode"].str;
						dat.user_unlock_item1 = user["user_unlock_item1"].str;
						dat.user_unlock_item2 = user["user_unlock_item2"].str;
						dat.user_unlock_item3 = user["user_unlock_item3"].str;

						dat.user_date_regis = user["user_date_regis"].str;
						dat.user_date_update = user["user_date_update"].str;

						userList.Add(dat);
					}
					msg.user = userList.ToArray();
				}
			}
		}
		if (apiCB != null)
			apiCB(msg);

	}

	IEnumerator ICallAPI(WWWForm form)
	{
		//create postform
		baseURL = UserCommonData.GetURL ();
		WWW api = new WWW(baseURL+page,form);
		Debug.Log("Call API : "+api.url);
		while (!api.isDone)
		{
			//waiting
			yield return null;
		}
		Debug.Log ("IsDone = true");
		IsDone = true;
		if (api.error != null)
		{
			IsError = true;
//			PopupObject.ShowAlertPopup("Error",api.error,"ปิด",errorMethod);
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ","กรุณาเชื่อมต่อ  อินเทอร์เน็ตเพื่อเข้าใช้งาน  Oryor  Smart  Application","เชื่อมต่อ",errorMethod);
			yield break;
		}
		//decrypt msg
		Debug.Log(api.text);
		JSONObject json = new JSONObject(api.text);
		if (json["msg"] != null)
		{
			msg.msg = json["msg"].str;
			if (msg.msg == "OK" || msg.msg == "user is already registered")
			{
				JSONObject userArr = json["user"];
				if (userArr != null)
				{
					List<UserData> userList = new List<UserData>();
					foreach(JSONObject user in userArr.list)
					{
						UserData dat = new UserData();
						dat.user_id = user["user_id"].str;
						dat.user_address = user["user_address"].str;
						dat.user_picture = user["user_picture"].str;
						dat.user_email = user["user_email"].str;
						dat.user_firstname = user["user_firstname"].str;
						dat.user_surname = user["user_surname"].str;
						dat.user_location = user["user_location"].str;
						dat.user_tel = user["user_tel"].str;

						dat.user_money = user["user_money"].str;

						dat.user_level = user["user_level"].str;
						//naming notice 
						//coin means item plus score
						//score means item plus exp
						//FUCK THE NAMING SERVER!!!
						dat.user_item1 = user["user_item_heart_no"].str;
						dat.user_item2 = user["user_item_coin_no"].str;
						dat.user_item3 = user["user_item_score_no"].str;

						dat.user_int_exp = user["user_int_exp"].str;
						dat.user_exp = user["user_exp"].str;
						dat.user_next_exp = user["user_next_exp"].str;

						dat.user_exp_plus = user["user_exp_plus"].str;
						dat.user_coin_plus = user["user_coin_plus"].str;
						dat.user_score_plus = user["user_score_plus"].str;
						dat.user_hardmode = user["user_hardmode"].str;
						dat.user_unlock_item1 = user["user_unlock_item1"].str;
						dat.user_unlock_item2 = user["user_unlock_item2"].str;
						dat.user_unlock_item3 = user["user_unlock_item3"].str;

						dat.user_date_regis = user["user_date_regis"].str;
						dat.user_date_update = user["user_date_update"].str;
						
						dat.user_schoolactivity = user["user_school_cat"].str;
						dat.user_oryoractivity = user["user_oryor_cat"].str;

						userList.Add(dat);
					}
					msg.user = userList.ToArray();
				}
			}
		}
		if (apiCB != null)
			apiCB(msg);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
