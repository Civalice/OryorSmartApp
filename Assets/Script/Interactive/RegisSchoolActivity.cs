using UnityEngine;
using System.Collections;
using TMPro;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class SchoolrActivityData
{
	[XmlAttribute]
	public string result;
}

public class RegisSchoolActivity : MonoBehaviour {
	/********* Use only RegisOryorActivity **************/
	public string checkurl = "http://www.oryor.com/oryor_smart_app_year2/activity_school.php?";
	public string url = "http://www.oryor.com/oryor_smart_app_year2/activity_school.php?";
	private WWW www = null;
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	public OryorActivityData contentList;
	public SettingControl settingControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
