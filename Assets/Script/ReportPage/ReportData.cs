using UnityEngine;
using System.Collections;
using TMPro;
using System.Timers;

public class ReportData : MonoBehaviour {
	public ButtonObject submitButton;
	public PopupObject popupobject;
	public ReportTypeBar type;
	public TextMeshPro name;
	public TextMeshPro email;
	public TextMeshPro Tel;
	public TextMeshPro Detail;

	private string lat="";
	private string lon="";
	private string phonenumber="";

	private int flagPhone=0;
	private int flagGPS=0;

	bool gpsInit = false;
	LocationInfo currentGPSPosition;
	// Use this for initialization
	void Awake()
	{
		submitButton.OnReleased += submitData;
		flagGPS = PlayerPrefs.GetInt("getGPS");
		flagPhone = PlayerPrefs.GetInt("getPhone");
#if UNITY_EDITOR
		PlayerPrefs.SetInt("getGPS",0);
		PlayerPrefs.Save();
		PlayerPrefs.SetInt("getPhone",0);
		PlayerPrefs.Save();
#endif
	}
	void Start () {
	}

	void submitData()
	{
		//check all attribute first
		//PopupObject.Button1.OnReleased += getGPS;
		//PopupObject.Button2.OnReleased += nogetGPS;
		if ((name.text != "")&&(email.text != "")&&(Tel.text != "")&&(Detail.text != "")){
			if(flagGPS!=1){
				PopupObject.ShowAlertPopup("แจ้งการขอข้อมูลพิกัดที่อยู่ปัจจุบัน","ท่านยินยอมให้ระบบส่งพิกัดปัจจุบันของท่านหรือไม่ \n\nท่านสามารถเปลี่ยนแปลงการบันทึกค่าการส่ง\nพิกัดได้ ที่ปุ่ม Settings ที่หน้าเมนูหลัก","ยินยอม",getGPS,"ไม่ยินยอม",nogetGPS);
			}
			else if(flagPhone!=1){
				PopupObject.ShowAlertPopup("แจ้งการขอข้อมูลเบอร์โทรศัพท์จากเครื่อง","ท่านยินยอมให้ระบบส่งข้อมูลเบอร์โทรศัพท์\nจากเครื่องของท่านหรือไม่\n\nท่านสามารถเปลี่ยนแปลงการบันทึกค่าการส่ง\nข้อมูลเบอร์โทรศัพท์ได้ ที่ปุ่ม Settings ที่หน้าเมนูหลัก","ยินยอม",getCellNumber,"ไม่ยินยอม",nogetCellNumber);
			}
			else{
				beginSendData();
			}
		}
		else
			PopupObject.ShowAlertPopup("Error","กรุณาใส่ข้อมูลให้ครบทุกช่อง","ตกลง");
	}
	void getGPS(){
		//lat = "";
		//lon = "";
		flagGPS = 1;
		PlayerPrefs.SetInt("getGPS",1);
		PlayerPrefs.Save();


		if (flagPhone != 1) {
			PopupObject.ShowAlertPopup("แจ้งการขอข้อมูลเบอร์โทรศัพท์จากเครื่อง","ท่านยินยอมให้ระบบส่งข้อมูลเบอร์โทรศัพท์\nจากเครื่องของท่านหรือไม่\n\nท่านสามารถเปลี่ยนแปลงการบันทึกค่าการส่ง\nข้อมูลเบอร์โทรศัพท์ได้ ที่ปุ่ม Settings ที่หน้าเมนูหลัก","ยินยอม",getCellNumber,"ไม่ยินยอม",nogetCellNumber);
		} else {
			beginSendData();
		}
				
		//beginSendData ();
	}
	void nogetGPS(){
		lat = "";
		lon = "";

		flagGPS = 0;
		if (flagPhone != 1) {
			PopupObject.ShowAlertPopup("แจ้งการขอข้อมูลเบอร์โทรศัพท์จากเครื่อง","ท่านยินยอมให้ระบบส่งข้อมูลเบอร์โทรศัพท์\nจากเครื่องของท่านหรือไม่\n\nท่านสามารถเปลี่ยนแปลงการบันทึกค่าการส่ง\nข้อมูลเบอร์โทรศัพท์ได้ ที่ปุ่ม Settings ที่หน้าเมนูหลัก","ยินยอม",getCellNumber,"ไม่ยินยอม",nogetCellNumber);
		} else {
			beginSendData();
		}
			
	}
	void getCellNumber(){
		phonenumber = "";
		flagPhone = 1;
		PlayerPrefs.SetInt("getPhone",1);
		PlayerPrefs.Save();
		
		beginSendData ();
	}
	void nogetCellNumber(){
		phonenumber = "";
		flagPhone = 0;
		
		beginSendData ();
	}
	void beginSendData(){
		StartCoroutine("SendData");
	}

	IEnumerator SendData()
	{
		LoadingScript.ShowLoading();
		WWWForm form = new WWWForm();
		string wwwText = UserCommonData.GetURL();

		//http://www.oryor.com/oryor_smart_app_year2/ws_client_year2v1.php?task=inform&
		//email=xxx&number=xxx&name=xxx&tel=xxx&detail=xxx&lat=xxx&lon=xxx&file1=&file2=&file3=
		form.AddField("task","inform");
		form.AddField("user_id",UserCommonData.pGlobal.user.user_id);
		form.AddField("type",type.currentIdx);
		form.AddField("name",name.text);
		form.AddField("email",email.text);
		form.AddField("tel",Tel.text);
		form.AddField("detail",Detail.text);
		if(flagGPS==1){
			form.AddField("lat",lat);
			form.AddField("lon",lon);
		}
		else{
			form.AddField("lat","");
			form.AddField("lon","");
		}
		if(flagPhone==1){form.AddField("phone",phonenumber);}
		else{form.AddField("phone","");}

		int i = 1;
		foreach(ReportPicSrc src in ReportCamera.PictureList)
		{
			string attr = "file"+i;
			Debug.Log(attr);
			Debug.Log(src.rawData);
			form.AddBinaryData(attr,src.rawData,attr+".jpeg","image/jpeg");
			i++;
		}

		WWW upload = new WWW(wwwText,form);
		yield return upload;
		LoadingScript.HideLoading ();
		if (!string.IsNullOrEmpty (upload.error)) {
				PopupObject.ShowAlertPopup ("พบปัญหาในการเชื่อมต่อ",
	                           "ไม่สามารถส่งข้อมูลของท่านไปยังเจ้าหน้าที่ได้ กรุณาตรวจสอบอินเทอร์เน็ตของท่าน และลองใหม่อีกครั้ง",
	                           "ตกลง", null);
			Debug.LogWarning ("LOCAL FILE ERROR: " + upload.error);
		} else {
				Debug.Log (upload.text);
				Debug.Log ("Uploaded Finish");
				PopupObject.ShowAlertPopup ("ระบบได้รับเรื่องร้องเรียนของท่านเรียบร้อยแล้ว", "ขอขอบคุณสำหรับข้อมูลที่มีประโยชน์ของท่าน ทางสำนักงานคณะกรรมการอาหารและยา จะดำเนินการตรวจสอบและแก้ไขโดยเร็วที่สุด", "ตกลง");
		}
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

	// Update is called once per frame
	void Update () {
//		bool touchedDown = TouchInterface.GetTouchDown ();
//		bool touchedUp = TouchInterface.GetTouchUp ();
//		Vector2 touchPos = TouchInterface.GetTouchPosition ();
//		if (touchedDown)
//		{
//			if (buttonCollider.OverlapPoint(touchPos))
//			{
//				StartCoroutine("SendData");
//			}
//		}
	}
}
