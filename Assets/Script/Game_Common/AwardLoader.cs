using UnityEngine;
using System.Collections;
[System.Serializable]
public class AwardLoadData
{
	public AwardItemData[] allGameData;
	public AwardItemData[] Game1Data;
	public AwardItemData[] Game2Data;
	public AwardItemData[] Game3Data;
	public AwardItemData[] Game4Data;
	public AwardItemData[] Game5Data;
	public AwardItemData[] Game6Data;
}

public class AwardLoader : MonoBehaviour {
	public AwardLoadData awardList = new AwardLoadData();
	
	public string url;
	WWW loader;
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	
	public void LoadAward()
	{
		LoadingScript.ShowLoading();
		StartCoroutine("Loading");
	}
	
	IEnumerator Loading()
	{
		url = UserCommonData.GetURL () + url;
		WWWForm form = new WWWForm();
		//check ID = 7 for test
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
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
			JSONObject awardAll = json["award_all"];
			int i = 0;
			if (awardAll.list != null)
			{
				awardList.allGameData = new AwardItemData[awardAll.list.Count];
				foreach(JSONObject award in awardAll.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.allGameData[i] = data;
					i++;
				}
			}
			else{
				awardList.allGameData[i] = null;
			}
			JSONObject awardMole = json["award_mole_lesson"];
			i = 0;
			if (awardMole.list != null)
			{
				awardList.Game1Data = new AwardItemData[awardMole.list.Count];
				foreach(JSONObject award in awardMole.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.Game1Data[i] = data;
					i++;
				}
			}
			JSONObject awardGDA = json["award_gda_challenge"];
			i = 0;
			if (awardGDA.list != null)
			{
				awardList.Game2Data = new AwardItemData[awardGDA.list.Count];
				foreach(JSONObject award in awardGDA.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.Game2Data[i] = data;
					i++;
				}
			}
			JSONObject awardDragDrop = json["award_drop_drag"];
			i = 0;
			if (awardDragDrop.list != null)
			{
				awardList.Game3Data = new AwardItemData[awardDragDrop.list.Count];
				foreach(JSONObject award in awardDragDrop.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.Game3Data[i] = data;
					i++;
				}
			}
			JSONObject awardCollector = json["award_oryor_collector"];
			i = 0;
			if (awardCollector.list != null)
			{
				awardList.Game4Data = new AwardItemData[awardCollector.list.Count];
				foreach(JSONObject award in awardCollector.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.Game4Data[i] = data;
					i++;
				}
			}
			JSONObject awardSchool = json["oryor_school"];
			i = 0;
			if (awardSchool.list != null)
			{
				awardList.Game5Data = new AwardItemData[awardSchool.list.Count];
				foreach(JSONObject award in awardSchool.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.Game5Data[i] = data;
					i++;
				}
			}
			JSONObject awardSBokBokBang = json["bok_bok_bang"];
			i = 0;
			if (awardSBokBokBang.list != null)
			{
				awardList.Game6Data = new AwardItemData[awardSBokBokBang.list.Count];
				foreach(JSONObject award in awardSBokBokBang.list)
				{
					AwardItemData data = new AwardItemData();
					data.title = award["award_name"].str;
					data.AwardDetail = award["award_detail"].str;
					data.max = int.Parse(award["award_condition"].str);
					data.progress = int.Parse(award["award_score"].str);
					awardList.Game6Data[i] = data;
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
