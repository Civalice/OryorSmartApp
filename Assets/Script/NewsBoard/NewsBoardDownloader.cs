using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class NewsboardDetail{
	[XmlAttribute]
	public string title;
	[XmlAttribute]
	public string[] desc;
	[XmlAttribute]
	public string[] img;
	[XmlAttribute]
	public string status;
	[XmlAttribute]
	public string type;
	[XmlAttribute]
	public string date;
	[XmlAttribute]
	public string link_web;
	[XmlAttribute]
	public string hot;
	[XmlAttribute]
	public string pin;
}

public class NewsBoardDownloader : MonoBehaviour {
	public string url = "http://www.oryor.com/oryor_smart_app_year2/news_list.php?type=10";
	
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	public NewsboardDetail[] contentList;
	private WWW www = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartDownload()
	{
		if (url == "")
			return;
		string LoadedUrl = url;
		StopCoroutine ("DownloadJSON");
		StartCoroutine ("DownloadJSON", LoadedUrl);
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
		url = UserCommonData.GetURL () + url + "&type=0&offset=0&limit=9&sort=0&cat=0";
		www = new WWW(url);
		Debug.Log ("Start Download News Board JSON : " + url);
		yield return new WaitForSeconds (3f);
		while(!www.isDone)
		{
			yield return null;
		}
		yield return www;
		
		/* EDIT: */
		if (!string.IsNullOrEmpty(www.error)){
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ",
			                           "ไม่สามารถตรวจสอบข้อมูลของท่านได้ กรุณาตรวจสอบอินเทอร์เน็ตของท่าน และลองใหม่อีกครั้ง",
			                           "ตกลง",null);
			Debug.LogWarning("LOCAL FILE ERROR: "+www.error);
		} else {
			JSONObject j = new JSONObject(www.text);
			Debug.Log("JSONObject : "+www.text);
			JSONObject arr = j["Result"];
			Debug.Log("All News : "+arr.list.Count);
			contentList = new NewsboardDetail[arr.list.Count];
			//loop for Result
			int i = 0;
			foreach(JSONObject content in arr.list){
				NewsboardDetail data = new NewsboardDetail();
				data.title = StringUtil.ParseUnicodeEscapes(content["title"].str);
//				data.desc = content["desc"].str;
				JSONObject descList = content["desc"];
				data.desc = new string[descList.list.Count];
				int k = 0;
				if(descList!=null){
					foreach(JSONObject desc in descList.list)
					{
						string tempdesc = desc.str.Replace(@"\/",@"/");
						data.desc[k] = tempdesc;
						k++;
					}
				}
				JSONObject imgList = content["img"];
				data.img = new string[imgList.list.Count];
				k = 0;
				if(imgList!=null){
					foreach(JSONObject img in imgList.list)
					{
						string tempimg = img.str.Replace(@"\/",@"/");
						data.img[k] = tempimg;
						k++;
					}
				}
				data.status = content["status"].str;
				data.type = content["type"].str;
				data.date = content["date"].str;
				data.link_web = content["link_web"].str;
				data.hot = content["hot"].str;
				data.pin = content["pin"].str;
				contentList[i] = data;
				i++;
			}
			isFinish = true;
			if (postDownloaded != null)
				postDownloaded();
		}
	}
}
