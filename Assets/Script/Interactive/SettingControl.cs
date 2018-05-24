using UnityEngine;
using System.Collections;
using TMPro;

public class SettingControl : MonoBehaviour {
	public Collider2D BTSound;
	public Collider2D BTTel;
	public Collider2D BTLat;

	private int BTON=0;
	
	public GameObject Sound;
	public GameObject Tel;
	public GameObject Lat;
	
	private MorphObject SnappingMorph = new MorphObject ();
	private bool IsTouch = false;
	private Vector2 lastTouch;
	public float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;

	private bool SoundOn = true;
	private bool TelOn = false;
	private bool LatOn = false;
	private string OryorOn = "";
	private string ActivityOn = "";

	public ButtonObject newsButton;
	public ButtonObject closeButton;
	public GameObject NewsBoardLayer;
	public GameObject SettingLayer;
	
	public ButtonObject oryorButton;
	public ButtonObject schoolButton;

	public Collider2D oryorBox;
	public Collider2D schoolBox;

	public GameObject oryorTextBox;
	public TextMeshPro oryorTextInput;
	public GameObject oboryorButton;
	public GameObject schoolTextBox;
	public TextMeshPro schoolTextInput;
	public GameObject obschoolButton;
	
	public RegisOryorActivity RegisGlobal;

	void Awake ()
	{
		closeButton.OnReleased += PressCloseButton;
		newsButton.OnReleased += NewsButtonPress;
		oryorButton.OnReleased += OryorButtonPress;
		schoolButton.OnReleased += SchoolButtonPress;
		SoundOn = UserCommonData.IsSoundOn();
		TelOn = UserCommonData.IsPhoneOn();
		LatOn = UserCommonData.IsGPSOn();
		ActivityOn = UserCommonData.IsActivityOn();
		OryorOn = UserCommonData.IsOryorOn();
		RegisGlobal.postDownloaded += RegisResult;
		Debug.Log (ActivityOn+" "+OryorOn);
		
		Debug.Log ("UserCommonData : "+UserCommonData.IsActivityOn()+", "+UserCommonData.IsOryorOn()+" "+PlayerPrefs.GetString("OryorActivityText"));
		Debug.Log ("Test Activity : "+ActivityOn+", "+OryorOn);
		if (SoundOn)
		{
			Sound.transform.localPosition = new Vector3(0.35f,0,0);
			AudioListener.volume = 1.0f;
		}
		else
		{
			Sound.transform.localPosition = new Vector3(-0.344f,0,0);
			AudioListener.volume = 0.0f;
		}

		if (TelOn)
		{
			Tel.transform.localPosition = new Vector3(0.35f,0,0);
		}
		else
		{
			Tel.transform.localPosition = new Vector3(-0.344f,0,0);
		}

		if (LatOn)
		{
			Lat.transform.localPosition = new Vector3(0.35f,0,0);
		}
		else
		{
			Lat.transform.localPosition = new Vector3(-0.344f,0,0);
		}
		
		if (ActivityOn!="")
		{
			SchoolConfirm();
		}
		else
		{

		}
		
		if (OryorOn!="")
		{
			OryorConfirm();
		}
		else
		{

		}
	}
	// Use this for initialization
	void Start () {
		
		closeButton.OnReleased += PressCloseButton;
		newsButton.OnReleased += NewsButtonPress;
		oryorButton.OnReleased += OryorButtonPress;
		schoolButton.OnReleased += SchoolButtonPress;
		SoundOn = UserCommonData.IsSoundOn();
		TelOn = UserCommonData.IsPhoneOn();
		LatOn = UserCommonData.IsGPSOn();
		ActivityOn = UserCommonData.IsActivityOn();
		OryorOn = UserCommonData.IsOryorOn();
		RegisGlobal.postDownloaded += RegisResult;
		Debug.Log (ActivityOn+" "+OryorOn);
		
		Debug.Log ("UserCommonData : "+UserCommonData.IsActivityOn()+", "+UserCommonData.IsOryorOn()+" "+PlayerPrefs.GetString("OryorActivityText"));
		Debug.Log ("Test Activity : "+ActivityOn+", "+OryorOn);
		if (SoundOn)
		{
			Sound.transform.localPosition = new Vector3(0.35f,0,0);
			AudioListener.volume = 1.0f;
		}
		else
		{
			Sound.transform.localPosition = new Vector3(-0.344f,0,0);
			AudioListener.volume = 0.0f;
		}
		
		if (TelOn)
		{
			Tel.transform.localPosition = new Vector3(0.35f,0,0);
		}
		else
		{
			Tel.transform.localPosition = new Vector3(-0.344f,0,0);
		}
		
		if (LatOn)
		{
			Lat.transform.localPosition = new Vector3(0.35f,0,0);
		}
		else
		{
			Lat.transform.localPosition = new Vector3(-0.344f,0,0);
		}
		
		if (ActivityOn!="")
		{
			SchoolConfirm();
		}
		else
		{
			
		}
		
		if (OryorOn!="")
		{
			OryorConfirm();
		}
		else
		{
			
		}
	}

	void NewsButtonPress()
	{
		NewsBoardLayer.SetActive(true);
		SettingLayer.SetActive(false);
		MainMenuGlobal.SetState(MainMenuState.MS_NEWSBOARD);
	}

	void PressCloseButton()
	{
		MainMenuGlobal.SetSettingObject(false);
	}

	void RegisResult()
	{
		if (RegisGlobal.contentList.result == "Not Found Activity") {
			PopupObject.ShowAlertPopup("ไม่สามารถลงทะเบียนได้",
			                           "ไม่พบรหัสลงทะเบียนที่กรอก กรุณาตรวจสอบรหัสลงทะเบียน และลองใหม่อีกครั้ง",
			                           "ตกลง",null);
		} else if(RegisGlobal.contentList.result == "User Already Regis Activity") {
			PopupObject.ShowAlertPopup("ไม่สามารถลงทะเบียนได้",
			                           "ท่านได้ร่วมสนุกกับทางสำนักงานคณะกรรมการอาหารและยาเป็นที่เรียบร้อย จึงไม่สามารถร่วมสนุกอีกครั้งได้",
			                           "ตกลง",CheckDisable);
		}else if(RegisGlobal.contentList.result == "Not Found User") {
			PopupObject.ShowAlertPopup("ไม่สามารถลงทะเบียนได้",
			                           "",
			                           "ตกลง",null);
		}else if(RegisGlobal.contentList.result == "success") {
			if(RegisGlobal.rtype=="school"){
				UserCommonData.SetActivity(true,schoolTextInput.text);
				PopupObject.ShowAlertPopup("ลงทะเบียนเรียบร้อย","สำนักงานคณะกรรมการอาหารและยาขอขอบคุณ ที่ได้มาลงทะเบียนร่วมสนุกกับเรา","ตกลง",SchoolConfirm);
			}
			else{
				UserCommonData.SetOryor(true,oryorTextInput.text);
				PopupObject.ShowAlertPopup("ลงทะเบียนเรียบร้อย","สำนักงานคณะกรรมการอาหารและยาขอขอบคุณ ที่ได้มาลงทะเบียนร่วมสนุกกับเรา","ตกลง",OryorConfirm);
			}
		}
		else{
			PopupObject.ShowAlertPopup("เกิดความผิดพลาด","กรุณาลองใหม่อีกครั้ง","ตกลง",OryorConfirm);
		}
	}
	void CheckDisable(){
		if(RegisGlobal.rtype=="school"){SchoolConfirm();}
		else{OryorConfirm();}
	}
	void OryorButtonPress()
	{
//		Debug.Log ("OryorActivity Input : "+oryorTextInput.text);
		MainMenuGlobal.SetState(MainMenuState.MS_SETTING);
		PopupObject.ShowAlertPopup("ท่านต้องการลงทะเบียนเพื่อเข้าร่วมกิจกรรมหรือไม่","ถ้าหากท่านได้ลงทะเบียนเข้าร่วมกิจกรรมแล้วท่านจะไม่สามารถลงทะเบียนซ้ำได้","ตกลง",OryorRegisAcitivity,"ยกเลิก",null);
	}
	void OryorRegisAcitivity(){
		RegisGlobal.StartDownload ("oryor", oryorTextInput.text);
	}
	void OryorConfirm()
	{
		oryorTextBox.SetActive (false);
//		oryorBox.gameObject.SetActive (false);
		oboryorButton.SetActive (false);
		oryorTextInput.text = OryorOn;
	}
	
	void SchoolButtonPress()
	{
//		Debug.Log ("SchoolActivity Input : "+schoolTextInput.text);
		MainMenuGlobal.SetState(MainMenuState.MS_SETTING);
		PopupObject.ShowAlertPopup("ท่านต้องการลงทะเบียนเพื่อเข้าร่วมกิจกรรมหรือไม่","ถ้าหากท่านได้ลงทะเบียนเข้าร่วมกิจกรรมแล้วท่านจะไม่สามารถลงทะเบียนซ้ำได้","ตกลง",SchoolRegisAcitivity,"ยกเลิก",null);
	}
	void SchoolRegisAcitivity(){
		RegisGlobal.StartDownload ("school", schoolTextInput.text);
	}
	void SchoolConfirm()
	{
		schoolTextBox.SetActive (false);
//		schoolBox.gameObject.SetActive (false);
		obschoolButton.SetActive (false);
		schoolTextInput.text = ActivityOn;
	}

	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown && MainMenuState.MS_SETTING == MainMenuGlobal.getCurrentState()){
			if(BTSound.OverlapPoint(touchPos)){
				//Debug.Log("Sound");
				BTON=1;
				IsTouch = true;
				lastTouch = touchPos;
			}
			else if(BTTel.OverlapPoint(touchPos)){
				//Debug.Log("Tel");
				BTON=2;
				IsTouch = true;
				lastTouch = touchPos;
			}
			else if(BTLat.OverlapPoint(touchPos)){
				//Debug.Log("Lat Lon");
				BTON=3;
				IsTouch = true;
				lastTouch = touchPos;
			}
//			else if(newsCollider.OverlapPoint(touchPos)){
//				//Debug.Log("Lat Lon");
//				BTON=4;
//				IsTouch = true;
//				lastTouch = touchPos;
//			}
		}
		if (touchedUp && MainMenuState.MS_SETTING == MainMenuGlobal.getCurrentState()){
			if(IsTouch){
				IsTouch = false;

//				if(newsCollider.OverlapPoint(touchPos)){
//
//				}
//				else{
					BTON=0;
//				}
			}
		}
		setUpdateSlide();
	}

	private void setUpdateSlide(){

		if(IsTouch){
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			ItemMoveMent += (touchPos.x - lastTouch.x) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.x - lastTouch.x)* (Sensitive/10.0f) / (Time.deltaTime);
			currentMoveSpeed = MoveSpeed;
			if (ItemMoveMent < -0.344f)
				ItemMoveMent = -0.344f;
			if (ItemMoveMent > 0.35f)
				ItemMoveMent = 0.35f;
			lastTouch = touchPos;
			SnappingMorph.morphEasein (ItemMoveMent, ItemMoveMent+currentMoveSpeed, Mathf.Abs(MoveSpeed)/10.0f + Smooth*30.0f);
			//if(currentMoveSpeed!=0){
			//	IsDrag = true;
			//}
			//move all item

			if(BTON==1){
				if(ItemMoveMent<=0.0f){
					Sound.transform.localPosition = new Vector3(-0.344f,0,0);
				}
				else{
					Sound.transform.localPosition = new Vector3(0.35f,0,0);
				}
			}
			else if(BTON==2){
				if(ItemMoveMent<=0.0f){
					Tel.transform.localPosition = new Vector3(-0.344f,0,0);
					TelOn = false;
				}
				else{
					Tel.transform.localPosition = new Vector3(0.35f,0,0);
					TelOn = true;
				}
			}
			else if(BTON==3){
				if(ItemMoveMent<=0.0f){
					Lat.transform.localPosition = new Vector3(-0.344f,0,0);
					LatOn = false;
				}
				else{
					Lat.transform.localPosition = new Vector3(0.35f,0,0);
					LatOn = true;
				}
			}
		}
		else{
			if(Sound.transform.localPosition.x<=0.0f){
				Sound.transform.localPosition = new Vector3(-0.344f,0,0);
				AudioListener.volume = 0.0f;
				UserCommonData.SetSound(false);
				SoundOn = false;
			}
			else{
				Sound.transform.localPosition = new Vector3(0.35f,0,0);
				AudioListener.volume = 1.0f;
				UserCommonData.SetSound(true);
				SoundOn = true;
			}

			if(Tel.transform.localPosition.x<=0.0f){
				Tel.transform.localPosition = new Vector3(-0.344f,0,0);
				UserCommonData.SetPhone(false);
				TelOn = false;
			}
			else{
				Tel.transform.localPosition = new Vector3(0.35f,0,0);
				UserCommonData.SetPhone(true);
				TelOn = true;
			}

			if(Lat.transform.localPosition.x<=0.0f){
				Lat.transform.localPosition = new Vector3(-0.344f,0,0);
				UserCommonData.SetGPS(false);
				LatOn = false;
			}
			else{
				Lat.transform.localPosition = new Vector3(0.35f,0,0);
				UserCommonData.SetGPS(true);
				LatOn = true;
			}
		}
	}
}
