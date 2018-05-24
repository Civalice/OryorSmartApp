using UnityEngine;
using System.Collections;
using TMPro;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class OryorActivityData
{
	[XmlAttribute]
	public string result;
}

public class RegisOryorActivity : MonoBehaviour {
	public string checkurl = "http://www.oryor.com/oryor_smart_app_year2/activity_oryor.php?";
	public string url = "http://www.oryor.com/oryor_smart_app_year2/activity_oryor.php?";
	private WWW www = null;
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	public OryorActivityData contentList;
	public SettingControl settingControl;
	public string rtype="";
	public string rnumber="";
	public string resultRegis="";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartDownload(string type,string number)
	{
		rtype = type;
		rnumber = number;
		Debug.Log ("Start Function : "+type+", "+number);
		if (type == "school") {
			checkurl = UserCommonData.GetURL ()+"?task=activityRegister&type=regisSchoolActivity";
			url = UserCommonData.GetURL ()+"?task=activityRegister&type=regisSchoolActivity";
		} else {
			checkurl = UserCommonData.GetURL ()+"?task=activityRegister&type=regisOryorActivity";
			url = UserCommonData.GetURL ()+"?task=activityRegister&type=regisOryorActivity";
		}
		StartCoroutine ("DownloadJSON", url);
	}
	public void StopDownload()
	{
		if (www != null) {
			Debug.Log("XMLDownloader Dispose");
			www.Dispose ();
		}
		www = null;
		StopCoroutine ("DownloadJSON");
	}
	
	
	public IEnumerator DownloadJSON(string url) 
	{
		WWWForm form = new WWWForm();
		Debug.Log("Search "+rtype+", "+rnumber+", "+UserCommonData.pGlobal.user.user_id);
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		form.AddField("code",rnumber);
		www = new WWW(url,form);
		Debug.Log ("Start Download JSON Register : " + url);
		yield return new WaitForSeconds (3f);
		while(!www.isDone)
		{
			yield return null;
		}
		yield return www;
		
		/* EDIT: */
		if (!string.IsNullOrEmpty(www.error)){
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ",
			                           "ไม่สามารถลงทะเบียนได้ กรุณาตรวจสอบอินเทอร์เน็ตของท่าน และลองใหม่อีกครั้ง",
			                           "ตกลง",null);
			Debug.LogWarning("LOCAL FILE ERROR: "+www.error);
		} else {
			//JSONObject j = new JSONObject(www.text);
			Debug.Log (www.text);
			
			if (www.text != null)
				resultRegis = www.text;
			else
				resultRegis = "0";
			isFinish = true;
			OryorActivityData data = new OryorActivityData();
			data.result =  www.text;
			Debug.Log("Register Activity Result "+rtype+" : "+data.result);
			contentList = data;
			if (postDownloaded != null){
				Debug.Log ("postDownloaded");
				postDownloaded();
			}
		}
	}
}
