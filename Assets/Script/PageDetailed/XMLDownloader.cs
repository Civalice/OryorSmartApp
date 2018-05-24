using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using TMPro;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class ContentData
{
	[XmlAttribute]
	public int dataid;
	[XmlAttribute]
	public int type;
	[XmlAttribute]
	public string title;
	[XmlArray]
	public string[] detail;
	[XmlElement]
	public string imgthumbnail;
	[XmlArray]
	public string[] img;
	[XmlElement]
	public string vdourl;
	[XmlElement]
	public string audiourl;
	[XmlElement]
	public string pdfurl;
	[XmlElement]
	public string gameurl;
	[XmlElement]
	public int view;
	[XmlElement]
	public int like;
	[XmlElement]
	public string publishdate;
	[XmlElement]
	public string publishby;
	[XmlElement]
	public int recommend;
	[XmlElement]
	public string weburl;
	
	public bool IsLike = false;
	public bool IsFav = false;
	
	public static ContentData Clone(ContentData a)
	{
		ContentData ret = new ContentData ();
		ret.audiourl = a.audiourl;
		ret.dataid = a.dataid;
		ret.detail = a.detail;
		ret.gameurl = a.gameurl;
		if ((a.img != null) && (a.img.Length > 0))
			ret.img = new string[a.img.Length];
		
		for (int i = 0; (a.img != null) && (i < a.img.Length); i++) {
			ret.img[i] = a.img[i];
		}
		ret.imgthumbnail = a.imgthumbnail;
		ret.like = a.like;
		ret.pdfurl = a.pdfurl;
		ret.publishby = a.publishby;
		ret.publishdate = a.publishdate;
		ret.recommend = a.recommend;
		ret.title = a.title;
		ret.type = a.type;
		ret.vdourl = a.vdourl;
		ret.view = a.view;
		ret.weburl = a.weburl;
		return ret;
	}
}

public class XMLDownloader : MonoBehaviour {
	public string url = "http://www.oryor.com/newweb/webservice_oryor/service_year2v1.php?task=getListInfo&type=9";
	public int offset = 0;
	public int limit;
	public string sort = "date";
	public int cat = 0;
	public string filter = "";
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	public ContentData[] contentList;
	private WWW www = null;
	// Use this for initialization
	public int maxPage = 0;
	
	void Start () {
		limit = PageDetailGlobal.pGlobal.MaxContentPerPage;
	}
	
	public void SetLink(string nUrl)
	{
		if (nUrl == "")
			return;
		url = nUrl;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void StartDownload(int offset)
	{
		if (url == "")
			return;
		string LoadedUrl = url;
		string offsetString = "&offset=" + offset;
		LoadedUrl += offsetString;
		string limitString = "&limit=" + limit;
		LoadedUrl += limitString;
		string sortString = "&sort=" + sort;
		LoadedUrl += sortString;
		string catString = "&cat=" + cat;
		LoadedUrl += catString;
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
		WWWForm form = new WWWForm();
		Debug.Log("Search "+filter);
		form.AddField("filter",filter);
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		www = new WWW(url,form);
		Debug.Log ("Start Download JSON : " + url);
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
			if (j["MAX"].str != null)
				maxPage = int.Parse(j["MAX"].str);
			else
				maxPage = 0;
			JSONObject arr = j["Result"];
			Debug.Log(arr);
			if (arr.list != null)
				contentList = new ContentData[arr.list.Count];
			else
				contentList = null;
			//loop for Result
			int i = 0;
			foreach(JSONObject content in arr.list){
				ContentData data = new ContentData();
				data.dataid = int.Parse(content["dataid"].str);
				//				data.title = ParseUnicodeEscapes(content["title"].str);
				data.title = content["title"].str.Replace(@"\/",@"/");
				data.type = int.Parse(content["type"].str);
				JSONObject detailList = content["detail"];
				data.detail = new string[detailList.list.Count];
				int k = 0;
				foreach(JSONObject detail in detailList.list)
				{
					//					string detailText = ParseUnicodeEscapes(detail.str);
					string detailText;
					if(detail.str!=null){
						detailText = detail.str.Replace(@"< br \/ >",@"\n").Replace(@"\r",@"").Replace (@"& quot ;",@"'");
					}
					else{
						detailText = detail.str;
					}
					
					data.detail[k] = detailText.Replace(@"\/",@"/");
					k++;
				}
				data.imgthumbnail = content["imgthumbnail"].str.Replace(@"\/",@"/");
				JSONObject imgList = content["img"];
				data.img = new string[imgList.list.Count];
				k = 0;
				if(imgList!=null){
					foreach(JSONObject img in imgList.list)
					{
						string imgURL = img.str.Replace(@"\/",@"/");
						data.img[k] = imgURL;
						k++;
					}
				}
				data.vdourl = StringUtil.ParseUnicodeEscapes(content["vdourl"].str.Replace(@"\/",@"/"));
				//audiourl not use now.
				data.audiourl = StringUtil.ParseUnicodeEscapes(content["audiourl"].str.Replace(@"\/",@"/"));
				data.pdfurl = StringUtil.ParseUnicodeEscapes(content["pdfurl"].str.Replace(@"\/",@"/"));
				//gameurl not use now.
				data.gameurl = content["gameurl"].str;
				data.view = int.Parse(content["view"].str);
				data.like = int.Parse(content["like"].str);
				data.publishdate = content["publishdate"].str;
				data.publishby = content["publishby"].str;
				data.recommend = (int)content["recommend"].n;
				data.weburl = StringUtil.ParseUnicodeEscapes(content["weburl"].str.Replace(@"\/",@"/"));
				contentList[i] = data;
				i++;
			}
			isFinish = true;
			if (postDownloaded != null)
				postDownloaded();
		}
	}
	
	void accessData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log(key);
				accessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list){
				accessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log(obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log(obj.n);
			break;
		case JSONObject.Type.BOOL:
			Debug.Log(obj.b);
			break;
		case JSONObject.Type.NULL:
			Debug.Log("NULL");
			break;
			
		}
	}
}
