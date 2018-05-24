using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;

public class UserCommonData : MonoBehaviour {
	public static UserCommonData pGlobal;
	public static string imei;
	public static string ShortcutString = "";
	public UserData user;
	public static bool IsLogin = false;
	public static bool IsFBLogin = false;
	public static bool IsTutorial = true;

	#if !UNITY_WEBGL && !DISABLE_WEBVIEW
	public static string url = "https://db.oryor.com/databank/oryor_mobile_api/ws_client_year3.php";
	#else
	public static string url = "http://www.oryor.com/oryor_mobile_api/ws_client_year3.php";
	#endif

	public string infomac = "";
	
	public static string GetURL()
	{
		return url;
	}

	public static void SetUserData(UserData _user)
	{
		IsLogin = true;
		pGlobal.user = _user;
		pGlobal.Save();
	}

	public static bool IsSoundOn()
	{
		if (PlayerPrefs.HasKey("Sound"))
		{
			return PlayerPrefs.GetInt("Sound")==1;
		}
		return true;
	}

	public static void SetSound(bool flag)
	{
		PlayerPrefs.SetInt("Sound",(flag)?1:0);
		PlayerPrefs.Save();
	}

	public static bool IsPhoneOn()
	{
		if (PlayerPrefs.HasKey("getPhone"))
		{
			return PlayerPrefs.GetInt("getPhone")==1;
		}
		return false;
	}
	
	public static void SetPhone(bool flag)
	{
		PlayerPrefs.SetInt("getPhone",(flag)?1:0);
		PlayerPrefs.Save();
	}

	public static bool IsGPSOn()
	{
		if (PlayerPrefs.HasKey("getGPS"))
		{
			return PlayerPrefs.GetInt("getGPS")==1;
		}
		return false;
	}
	
	public static void SetGPS(bool flag)
	{
		PlayerPrefs.SetInt("getGPS",(flag)?1:0);
		PlayerPrefs.Save();
	}

	public static string IsActivityOn()
	{
		return PlayerPrefs.GetString("SchoolActivityText");
	}
	
	public static void SetActivity(bool flag, string str)
	{
		PlayerPrefs.SetInt("SchoolActivity",(flag)?1:0);
		PlayerPrefs.SetString("SchoolActivityText",str);
		PlayerPrefs.Save();
	}

	public static string IsOryorOn()
	{
		return PlayerPrefs.GetString("OryorActivityText");
	}
	
	public static void SetOryor(bool flag, string str)
	{
		PlayerPrefs.SetInt("OryorActivity",(flag)?1:0);
		PlayerPrefs.SetString("OryorActivityText",str);
		PlayerPrefs.Save();
	}
	
	public static bool IsAGOn()
	{
		if (PlayerPrefs.HasKey("getAG"))
		{
			return PlayerPrefs.GetInt("getAG")==1;
		}
		return false;
	}
	
	public static void SetAG(bool flag)
	{
		PlayerPrefs.SetInt("getAG",(flag)?1:0);
		PlayerPrefs.Save();
	}

	public static int[] LoadShortcut()
	{
//		PlayerPrefs.DeleteAll();

		if (!PlayerPrefs.HasKey("ShortcutString")) return null;
		ShortcutString = PlayerPrefs.GetString("ShortcutString");
		string[] arrList = ShortcutString.Split(',');
		if (arrList.Length <= 0) return null;
		int[] arr = new int[arrList.Length];
		int i = 0;
		foreach(string s in arrList)
		{
			if (s == "") continue;
			arr[i] = int.Parse(s);
			i++;
		}
		return arr;
	}

	public static void AddBookmark(int idx)
	{
		string[] arrList = ShortcutString.Split(',');
		if (arrList.Length <= 0) return;
		int[] arr = new int[arrList.Length];
		int i = 0;
		foreach(string s in arrList)
		{
			if (s == "") continue;
			if (int.Parse(s) == idx) return;
			arr[i] = int.Parse(s);
			i++;
		}
		//addBoomark here

		if (ShortcutString != "")
			ShortcutString += ","+idx;
		else
			ShortcutString += idx;
		//write to file
		PlayerPrefs.SetString("ShortcutString",ShortcutString);
		PlayerPrefs.Save();
	}

	public static void RemoveBookmark(int idx)
	{
		if (!PlayerPrefs.HasKey("ShortcutString")) return;
		ShortcutString = PlayerPrefs.GetString("ShortcutString");
		string[] arrList = ShortcutString.Split(',');
		if (arrList.Length <= 0) return;
		int[] arr = new int[arrList.Length-1];
		int i = 0;
		foreach(string s in arrList)
		{
			if (s == "") continue;
			if (int.Parse(s) == idx) continue;
			arr[i] = int.Parse(s);
			i++;
		}
		//convert arr to string again
		ShortcutString = "";
		foreach(int a in arr)
		{
			if (ShortcutString == "")
				ShortcutString += a;
			else
				ShortcutString += ","+a;
		}
		//write to file
		PlayerPrefs.SetString("ShortcutString",ShortcutString);
		PlayerPrefs.Save();
	}

	public int GetEXPProgress()
	{
		int cExp = int.Parse(user.user_exp) - int.Parse(user.user_int_exp);
		int cExpMax = int.Parse(user.user_next_exp) - int.Parse(user.user_int_exp);
		float currentEXPPercent = (cExp/(float)cExpMax)*100;
		return (int)currentEXPPercent;
	}

	public void Save()
	{
		PlayerPrefs.SetInt("IsLogin",(IsLogin)?1:0);
		PlayerPrefs.SetInt("IsFBLogin",(IsFBLogin)?1:0);
		PlayerPrefs.SetInt("IsTutorial",(IsTutorial)?1:0);
		PlayerPrefs.SetString("user_id",user.user_id);
		PlayerPrefs.SetString("user_address",user.user_address);
		PlayerPrefs.SetString("user_picture",user.user_picture);
		PlayerPrefs.SetString("user_email",user.user_email);
		PlayerPrefs.SetString("user_firstname",user.user_firstname);
		PlayerPrefs.SetString("user_surname",user.user_surname);
		PlayerPrefs.SetString("user_location",user.user_location);
		PlayerPrefs.SetString("user_tel",user.user_tel);

		PlayerPrefs.SetString("user_money",user.user_money);
		PlayerPrefs.SetString("user_level",user.user_level);

		PlayerPrefs.SetString("user_item1",user.user_item1);
		PlayerPrefs.SetString("user_item2",user.user_item2);
		PlayerPrefs.SetString("user_item3",user.user_item3);

		PlayerPrefs.SetString("user_int_exp",user.user_int_exp);
		PlayerPrefs.SetString("user_exp",user.user_exp);
		PlayerPrefs.SetString("user_next_exp",user.user_next_exp);

		PlayerPrefs.SetString("user_exp_plus",user.user_exp_plus);
		PlayerPrefs.SetString("user_coin_plus",user.user_coin_plus);
		PlayerPrefs.SetString("user_score_plus",user.user_score_plus);

		PlayerPrefs.SetString("user_hardmode",user.user_hardmode);
		PlayerPrefs.SetString("user_unlock_item1",user.user_unlock_item1);
		PlayerPrefs.SetString("user_unlock_item2",user.user_unlock_item2);
		PlayerPrefs.SetString("user_unlock_item3",user.user_unlock_item3);

		PlayerPrefs.SetString("user_date_regis",user.user_date_regis);
		PlayerPrefs.SetString("user_date_update",user.user_date_update);

		int scResult = 0;		
		int orResult = 0;		
		if (user.user_schoolactivity != "") {
			scResult = 1;		
		}
		if (user.user_oryoractivity != "") {
			orResult = 1;		
		}
		PlayerPrefs.SetInt("SchoolActivity",scResult);
		PlayerPrefs.SetInt("OryorActivity",orResult);
		PlayerPrefs.Save();

	}

	public void Load()
	{
		if (!PlayerPrefs.HasKey("IsLogin")) return;
		IsLogin = PlayerPrefs.GetInt("IsLogin") != 0;
		if (PlayerPrefs.HasKey("IsFBLogin"))
			IsFBLogin = PlayerPrefs.GetInt("IsFBLogin") != 0;
		if (PlayerPrefs.HasKey("IsTutorial"))
			IsTutorial = PlayerPrefs.GetInt("IsTutorial") != 0;
		if(IsLogin)
		{
			user = new UserData();
			user.user_id = PlayerPrefs.GetString("user_id");
			user.user_address = PlayerPrefs.GetString("user_address");
			user.user_picture = PlayerPrefs.GetString("user_picture");
			user.user_email = PlayerPrefs.GetString("user_email");
			user.user_firstname = PlayerPrefs.GetString("user_firstname");
			user.user_surname = PlayerPrefs.GetString("user_surname");
			user.user_location = PlayerPrefs.GetString("user_location");
			user.user_tel = PlayerPrefs.GetString("user_tel");

			user.user_money = PlayerPrefs.GetString("user_money");
			user.user_level = PlayerPrefs.GetString("user_level");

			user.user_item1 = PlayerPrefs.GetString("user_item1");
			user.user_item2 = PlayerPrefs.GetString("user_item2");
			user.user_item3 = PlayerPrefs.GetString("user_item3");

			user.user_int_exp = PlayerPrefs.GetString("user_int_exp");
			user.user_exp = PlayerPrefs.GetString("user_exp");
			user.user_next_exp = PlayerPrefs.GetString("user_next_exp");

			user.user_exp_plus = PlayerPrefs.GetString("user_exp_plus");
			user.user_coin_plus = PlayerPrefs.GetString("user_coin_plus");
			user.user_score_plus = PlayerPrefs.GetString("user_score_plus");

			user.user_hardmode = PlayerPrefs.GetString("user_hardmode");
			user.user_unlock_item1 = PlayerPrefs.GetString("user_unlock_item1");
			user.user_unlock_item2 = PlayerPrefs.GetString("user_unlock_item2");
			user.user_unlock_item3 = PlayerPrefs.GetString("user_unlock_item3");

			user.user_date_regis = PlayerPrefs.GetString("user_date_regis");
			user.user_date_update = PlayerPrefs.GetString("user_date_update");

			if (PlayerPrefs.HasKey("SchoolActivity"))
			{
				user.user_schoolactivity = PlayerPrefs.GetInt("SchoolActivity").ToString();
			}
			if (PlayerPrefs.HasKey("OryorActivity"))
			{
				user.user_oryoractivity = PlayerPrefs.GetInt("OryorActivity").ToString();
			}

		}
	}
	

	void Awake () {
		DontDestroyOnLoad(gameObject);
		pGlobal = this;
		#if !UNITY_EDITOR
		imei = SystemInfo.deviceUniqueIdentifier;
		#else
		imei = "85EE2DA2-54E0-49F4-B298-5BA2161F32A68";
		#endif
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
