using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using TMPro;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class ContentCheckData
{
	[XmlAttribute]
	public string cmttype;
	[XmlAttribute]
	public string prdno;
	[XmlAttribute]
	public string cattype;
	[XmlAttribute]
	public string rid;
	[XmlAttribute]
	public string branth;
	[XmlAttribute]
	public string branen;
	[XmlAttribute]
	public string prdth;
	[XmlAttribute]
	public string prden;
	[XmlAttribute]
	public string addr;
	[XmlAttribute]
	public string frg;
	[XmlAttribute]
	public string ctm;
	[XmlAttribute]
	public string cnt;
}
[System.Serializable]
public class ContentCheckDangerRegisterData
{
	[XmlAttribute]
	public string txcrgttpnm;
	[XmlAttribute]
	public string txcrgtno;
	[XmlAttribute]
	public string txclctnm;
	//[XmlAttribute]
	//public string txclctcode;
	[XmlAttribute]
	public string txcaddr;
	[XmlAttribute]
	public string txcusednm;
	[XmlAttribute]
	public string txcfrmtnm;
	[XmlAttribute]
	public string txcappvdate;
	[XmlAttribute]
	public string txcexpdate;
	[XmlAttribute]
	public string ttxcnm;
	[XmlAttribute]
	public string etxcn;
	[XmlAttribute]
	public string txclcnsnm;
	[XmlAttribute]
	public string txcpdlctnm;
	[XmlAttribute]
	public string txcpdaddr;
	[XmlAttribute]
	public string[] txcpsnnm;
	
}
[System.Serializable]
public class ContentCheckDangerLicenseData
{
	[XmlAttribute]
	public string txclcntpnm;
	[XmlAttribute]
	public string txclcnno;
	[XmlAttribute]
	public string txclctnm;
	[XmlAttribute]
	public string txclctcode;
	[XmlAttribute]
	public string txcaddr;
	[XmlAttribute]
	public string txcttrdnm;
	[XmlAttribute]
	public string txcetrdnm;
	[XmlAttribute]
	public string txcrgno;
	[XmlAttribute]
	public string txckplctnm;
	[XmlAttribute]
	public string txckpaddr;
	[XmlAttribute]
	public string txcusednm;
	[XmlAttribute]
	public string txcfrmtnm;
	[XmlAttribute]
	public string txclcnsnm;
	[XmlAttribute]
	public string[] txcpsnnm;
	
}
[System.Serializable]
public class ContentCheckFoodData
{
	[XmlAttribute]
	public string fnregntftpcd;
	[XmlAttribute]
	public string fnregntftpnm;
	[XmlAttribute]
	public string fnfdtypenm;
	[XmlAttribute]
	public string fnfdlcnno;
	[XmlAttribute]
	public string fnfdlcnsnm;
	[XmlAttribute]
	public string fnfdlctnm;
	[XmlAttribute]
	public string fnlctaddr;
	[XmlAttribute]
	public string fnprdnmt;
	[XmlAttribute]
	public string fnprdnme;
	[XmlAttribute]
	public string fnfdpdtnotxt;
}
[System.Serializable]
public class ContentCheckDrugData
{
	[XmlAttribute]
	public string pvncd;
	[XmlAttribute]
	public string lcnno;
	[XmlAttribute]
	public string lcnsid;
	[XmlAttribute]
	public string rgtno;
	[XmlAttribute]
	public string lcntpcd;
	[XmlAttribute]
	public string rgttpcd;
	[XmlAttribute]
	public string drgtpcd;
	[XmlAttribute]
	public string appdate;
	[XmlAttribute]
	public string thadrgnm;
	[XmlAttribute]
	public string engdrgnm;
	[XmlAttribute]
	public string engfrgnnm;
	[XmlAttribute]
	public string cntcd;
	[XmlAttribute]
	public string engdrgtpnm;
	[XmlAttribute]
	public string itemno;
	[XmlAttribute]
	public string frn_no;
	[XmlAttribute]
	public string expdate;
	[XmlAttribute]
	public string Ranking;
	[XmlAttribute]
	public string GROUPNAME;
	[XmlAttribute]
	public string Newcode;
	[XmlAttribute]
	public string fulladdr;
}
[System.Serializable]
public class ContentCheckToolData
{
	[XmlAttribute]
	public string pvncd;
	[XmlAttribute]
	public string lcnno;
	[XmlAttribute]
	public string lcnsid;
	[XmlAttribute]
	public string rgttpcd;
	[XmlAttribute]
	public string thamdnm;
	[XmlAttribute]
	public string engmdnm;
	[XmlAttribute]
	public string engfrgnnm;
	[XmlAttribute]
	public string cntcd;
	[XmlAttribute]
	public string appvdate;
	[XmlAttribute]
	public string expdate;
	[XmlAttribute]
	public string appno;
	[XmlAttribute]
	public string productnm;
	[XmlAttribute]
	public string itemno;
	[XmlAttribute]
	public string desc;
	[XmlAttribute]
	public string GroupName;
	[XmlAttribute]
	public string Ranking;
	[XmlAttribute]
	public string newcode;
	[XmlAttribute]
	public string fulladdr;
}

public class CheckDownloader : MonoBehaviour {
	public string checkurl = "http://www.oryor.com/oryor_smart_app_year2/ws_client_year2.php?task=checkInfo&";
	public string url = "http://www.oryor.com/oryor_smart_app_year2/ws_client_year2.php?task=checkInfo&";
	private WWW www = null;
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	public ContentCheckData[] contentList;
	public ContentCheckDangerRegisterData[] contentListDangerRegister;
	public ContentCheckDangerLicenseData[] contentListDangerLicense;
	public ContentCheckFoodData[] contentListFood;
	public ContentCheckDrugData[] contentListDrug;
	public ContentCheckToolData[] contentListTool;
	public CheckPageGlobal chkGlobal;
	public string checktype;
	public string seprdno;
	
	// Use this for initialization
	void Start () {
		checkurl = UserCommonData.GetURL ()+"?task=checkInfo&";
		url = UserCommonData.GetURL ()+"?task=checkInfo&";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public void StartDownload(string prdno)
	{
		int offset = chkGlobal.ProductType;
		seprdno = prdno;
		checkformat(prdno);
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
		Debug.Log("Search "+seprdno);
		form.AddField("number",seprdno);
//		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		www = new WWW(url,form);
		Debug.Log ("Start Download JSON Check : " + url);
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
			Debug.Log (www.text);
			
			if (j["type"] != null)
				checktype = j["type"].str;
			else
				checktype = "0";
			
			JSONObject arr = j["result"];
			
			if (arr.list == null){
				contentList = null;
				contentListDangerRegister = null;
				contentListDangerLicense = null;
				contentListFood = null;
				contentListDrug = null;
				contentListTool = null;
			}
			else{
				//			Debug.Log ("check type : "+checktype+" "+arr.list.Count.ToString());
				if(checktype=="1"){ //Cosmetic
					Debug.Log ("Search Cosmetic : "+arr.list);
					if (arr.list != null){
						contentList = new ContentCheckData[arr.list.Count];
						//loop for Result
						int i = 0;
						foreach(JSONObject content in arr.list){
							ContentCheckData data = new ContentCheckData();
							data.cmttype = content["cmttype"].str;
							data.prdno = content["prdno"].str.Replace(@"\/",@"/");
							data.cattype = content["cattype"].str;
							data.branth = content["branth"].str;
							data.branen = content["branen"].str;
							data.prdth = content["prdth"].str;
							data.prden = content["prden"].str;
							data.addr = content["addr"].str.Replace(@"\/",@"/");
							data.frg = content["frg"].str;
							data.ctm = content["ctm"].str;
							data.cnt = content["cnt"].str;
							contentList[i] = data;
							i++;
						}
					}
				}
				else if(checktype=="2.1"){ // Danger Register
					Debug.Log ("Danger Register : "+arr.list);
					if (arr.list != null){
						contentListDangerRegister = new ContentCheckDangerRegisterData[arr.list.Count];
						//loop for Result
						int i = 0;
						foreach(JSONObject content in arr.list){
							ContentCheckDangerRegisterData data = new ContentCheckDangerRegisterData();
							data.txcrgttpnm = content["txcrgttpnm"].str;
							data.txcrgtno = content["txcrgtno"].str.Replace(@"\/",@"/");
							data.txclctnm = content["txclctnm"].str;
							//data.txclctcode = content["txclctcode"].str;
							data.txcaddr = content["txcaddr"].str.Replace(@"\/",@"/");
							data.txcusednm = content["txcusednm"].str;
							data.txcfrmtnm = content["txcfrmtnm"].str;
							data.txcappvdate = content["txcappvdate"].str.Replace(@"\/",@"/");
							data.txcexpdate = content["txcexpdate"].str.Replace(@"\/",@"/");
							data.ttxcnm = content["ttxcnm"].str;
							data.etxcn = content["etxcn"].str;
							data.txclcnsnm = content["txclcnsnm"].str;
							data.txcpdlctnm = content["txcpdlctnm"].str;
							data.txcpdaddr = content["txcpdaddr"].str.Replace(@"\/",@"/");
							JSONObject arrDetail = content["txcpsnnm"];
							data.txcpsnnm = new string[arrDetail.list.Count];
							int k = 0;
							foreach(JSONObject detail in arrDetail.list)
							{
								string detailText;
								if(detail.str!=null){
									detailText = detail.str.Replace(@"< br \/ >",@"\n").Replace(@"\r",@"");
								}
								else{
									detailText = detail.str;
								}
								
								data.txcpsnnm[k] = detailText;
								k++;
							}
							contentListDangerRegister[i] = data;
							i++;
						}
					}
				}
				else if(checktype=="2.2"){ // Danger License
					Debug.Log ("Danger License : "+arr.list);
					if (arr.list != null){
						contentListDangerLicense = new ContentCheckDangerLicenseData[arr.list.Count];
						//loop for Result
						int i = 0;
						foreach(JSONObject content in arr.list){
							ContentCheckDangerLicenseData data = new ContentCheckDangerLicenseData();
							data.txclcntpnm = content["txclcntpnm"].str;
							data.txcrgno = content["txcrgno"].str.Replace(@"\/",@"/");
							data.txclctnm = content["txclctnm"].str;
							data.txclctcode = content["txclctcode"].str;
							data.txcaddr = content["txcaddr"].str.Replace(@"\/",@"/");
							data.txcttrdnm = content["txcttrdnm"].str;
							data.txcetrdnm = content["txcetrdnm"].str;
							data.txcrgno = content["txcrgno"].str.Replace(@"\/",@"/");
							data.txckplctnm = content["txckplctnm"].str;
							data.txckpaddr = content["txckpaddr"].str.Replace(@"\/",@"/");
							data.txcusednm = content["txcusednm"].str;
							data.txcfrmtnm = content["txcfrmtnm"].str;
							data.txclcnsnm = content["txclcnsnm"].str;
							JSONObject arrDetail = content["txcpsnnm"];
							data.txcpsnnm = new string[arrDetail.list.Count];
							int k = 0;
							foreach(JSONObject detail in arrDetail.list)
							{
								string detailText;
								if(detail.str!=null){
									detailText = detail.str.Replace(@"< br \/ >",@"\n").Replace(@"\r",@"");
								}
								else{
									detailText = detail.str;
								}
								
								data.txcpsnnm[k] = detailText;
								k++;
							}
							contentListDangerLicense[i] = data;
							i++;
						}
					}
				}
				else if(checktype=="3"){ //Drug
					if (arr.list != null){
					Debug.Log ("Search Drug : "+arr.list);
						contentListDrug = new ContentCheckDrugData[arr.list.Count];
						//loop for Result
						int i = 0;
						foreach(JSONObject content in arr.list){
							ContentCheckDrugData data = new ContentCheckDrugData();
							data.pvncd = content["pvncd"].str;
							data.lcnno = content["lcnno"].str.Replace(@"\/",@"/");
							data.lcnsid = content["lcnsid"].str;
							data.rgtno = content["rgtno"].str;
							data.lcntpcd = content["lcntpcd"].str;
							data.rgttpcd = content["rgttpcd"].str;
							data.drgtpcd = content["drgtpcd"].str;
							data.appdate = content["appdate"].str.Replace(@"\/",@"/");
							data.thadrgnm = content["thadrgnm"].str;
							data.engdrgnm = content["engdrgnm"].str;
							data.engfrgnnm = content["engfrgnnm"].str;
							data.cntcd = content["cntcd"].str;
							data.engdrgtpnm = content["engdrgtpnm"].str;
							data.itemno = content["itemno"].str;
							data.frn_no = content["frn_no"].str;
							data.expdate = content["expdate"].str;
							data.Ranking = content["Ranking"].str;
							data.GROUPNAME = content["GROUPNAME"].str;
							data.Newcode = content["Newcode"].str;
							data.fulladdr = content["fulladdr"].str.Replace(@"\/",@"/");
							
							contentListDrug[i] = data;
							i++;
						}
					}
				}
				else if(checktype=="4"){ // Food
					Debug.Log ("Food : "+arr.list);
					if (arr.list != null){
						contentListFood = new ContentCheckFoodData[arr.list.Count];
						//loop for Result
						int i = 0;
						foreach(JSONObject content in arr.list){
							ContentCheckFoodData data = new ContentCheckFoodData();
							data.fnregntftpcd = content["fnregntftpcd"].str;
							data.fnregntftpnm = content["fnregntftpnm"].str;
							data.fnfdtypenm = content["fnfdtypenm"].str;
							data.fnfdlcnno = content["fnfdlcnno"].str.Replace(@"\/",@"/");
							data.fnfdlcnsnm = content["fnfdlcnsnm"].str;
							data.fnfdlctnm = content["fnfdlctnm"].str;
							data.fnlctaddr = content["fnlctaddr"].str.Replace(@"\/",@"/");
							JSONObject dataDetail = content["row"];
							//					Debug.Log (dataDetail.list.Count);
							foreach(JSONObject dataList in dataDetail.list){
								data.fnprdnmt = dataList["fnprdnmt"].str;
								data.fnprdnme = dataList["fnprdnme"].str;
								data.fnfdpdtnotxt = dataList["fnfdpdtnotxt"].str;
							}
							contentListFood[i] = data;
							i++;
						}
					}
				}
				else if(checktype=="5"){ // Tool
					Debug.Log ("Tool License : "+arr.list);
					if (arr.list != null){
						contentListTool = new ContentCheckToolData[arr.list.Count];
						//loop for Result
						int i = 0;
						foreach(JSONObject content in arr.list){
							ContentCheckToolData data = new ContentCheckToolData();
							data.pvncd = content["pvncd"].str;
							data.lcnno = content["lcnno"].str;
							data.lcnsid = content["lcnsid"].str;
							data.rgttpcd = content["rgttpcd"].str;
							data.thamdnm = content["thamdnm"].str;
							data.engmdnm = content["engmdnm"].str;
							data.engfrgnnm = content["engfrgnnm"].str;
							data.cntcd = content["cntcd"].str;
							data.appvdate = content["appvdate"].str.Replace(@"\/",@"/");
							data.expdate = content["expdate"].str.Replace(@"\/",@"/");
							data.appno = content["appno"].str;
							data.productnm = content["productnm"].str;
							data.itemno = content["itemno"].str;
							data.desc = content["desc"].str;
							data.GroupName = content["GroupName"].str;
							data.Ranking = content["Ranking"].str;
							data.newcode = content["newcode"].str;
							data.fulladdr = content["fulladdr"].str;
							contentListTool[i] = data;
							i++;
						}
					}
				}
			}
			isFinish = true;
			if (postDownloaded != null){
				Debug.Log ("postDownloaded");
				postDownloaded();
			}
		}
	}
	
	void checkformat(string prdno){
		
		url = checkurl+"&type="+CheckPageGlobal.pGlobal.ProductType;
		
	}
	public void clearxmldata(){
		if(contentList!=null)
			contentList=null;
		if(contentListDangerRegister!=null)
			contentListDangerRegister=null;
		if(contentListDangerLicense!=null)
			contentListDangerLicense=null;
		if(contentListFood!=null)
			contentListFood=null;
		if(contentListDrug!=null)
			contentListDrug=null;
		if(contentListTool!=null)
			contentListTool=null;
	}
}
