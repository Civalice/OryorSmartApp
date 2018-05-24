using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


#if UNITY_IOS
public class ReportCamera : MonoBehaviour {
	//	public Collider2D buttonCollider;
	public ButtonObject button;
	public GameObject PictureGal;
	public GameObject PictureSrcPrefabs;
	public GameObject WarningText;
	public ButtonObject BackButton;
	
	public ReportMainSound mSound;
	
	public static List<ReportPicSrc> PictureList = new List<ReportPicSrc>();
	
	Texture2D mImg;
	void Awake()
	{
		BackButton.OnReleased += exit;
		button.OnReleased += TakePicture;
		//		IOSCamera.instance.OnImagePicked += OnImage;
		IOSCamera.OnImagePicked += OnImage;
	}
	
	void exit()
	{
		//		IOSCamera.instance.OnImagePicked -= OnImage;
		IOSCamera.OnImagePicked -= OnImage;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void CreateReport(Texture2D _img,byte[] _data)
	{
		GameObject tmp = GameObject.Instantiate(PictureSrcPrefabs) as GameObject;
		ReportPicSrc comp = tmp.GetComponent<ReportPicSrc>();
		comp.SetImg(_img);
		comp.rawData = _data;
		comp.idx = PictureList.Count+1;
		tmp.transform.parent = PictureGal.transform;
		tmp.transform.localPosition = new Vector3(PictureList.Count * 1.5f,0,0);
		WarningText.SetActive(false);
		PictureList.Add(comp);
	}
	void TakePicture()
	{
		Debug.Log ("Open the Camera");
		mSound.playReportSound("click");
		if (PictureList.Count >= 3) return;
		IOSCamera.instance.GetImageFromCamera();
	}
	private void OnImage (IOSImagePickResult result) {
		if(result.IsSucceeded) {
			mImg = result.image;
			byte[] bArr = result.image.EncodeToJPG();
			CreateReport(mImg,bArr);
		} else {
			IOSMessage.Create("ERROR", "Image Load Failed");
		}
		
		//		IOSCamera.instance.OnImagePicked -= OnImage;
		IOSCamera.OnImagePicked -= OnImage;
	}
}
#elif UNITY_ANDROID
public class ReportCamera : MonoBehaviour {
	public ButtonObject button;
	public GameObject PictureGal;
	public GameObject PictureSrcPrefabs;
	public GameObject WarningText;
	public ButtonObject BackButton;
	
	public ReportMainSound mSound;
	
	public static List<ReportPicSrc> PictureList = new List<ReportPicSrc>();
	
	Texture2D mImg;
	void Awake()
	{
		//		if(AndroidCamera.instance.OnImagePicked==null){
		PictureList.Clear();
		button.OnReleased += TakePicture;
		BackButton.OnReleased += exit;
	}
	
	void exit()
	{
		AndroidCamera.instance.OnImagePicked -= OnImagePicked;
	}
	// Use this for initialization
	void Start () {
		AndroidCamera.instance.OnImagePicked += OnImagePicked;
	}
	
	void TakePicture()
	{
		Debug.Log ("Take Picture");
		mSound.playReportSound("click");
		if (PictureList.Count >= 3) return;
		AndroidCamera.instance.GetImageFromCamera();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void CreateReport(Texture2D _img,byte[] _data)
	{
		GameObject tmp = GameObject.Instantiate(PictureSrcPrefabs) as GameObject;
		ReportPicSrc comp = tmp.GetComponent<ReportPicSrc>();
		comp.SetImg(_img);
		comp.rawData = _data;
		comp.idx = PictureList.Count+1;
		tmp.transform.parent = PictureGal.transform;
		tmp.transform.localPosition = new Vector3(PictureList.Count * 1.5f,0,0);
		WarningText.SetActive(false);
		PictureList.Add(comp);
	}
	
	private void OnImagePicked(AndroidImagePickResult result) {
		Debug.Log("OnImagePicked");
		if(result.IsSucceeded) {
			mImg = result.image;
			byte[] bArr = result.image.EncodeToPNG();
			CreateReport(mImg,bArr);
		}
	}
}
#elif UNITY_WINRT
public class ReportCamera : MonoBehaviour {
	public ButtonObject button;
	public GameObject PictureGal;
	public GameObject PictureSrcPrefabs;
	public GameObject WarningText;
	public ButtonObject BackButton;
	
	public ReportMainSound mSound;
	private bool hasCache;
	private byte [] cacheImage;
	private byte[] cacheThumbnail;
	public static List<ReportPicSrc> PictureList = new List<ReportPicSrc>();
	
	Texture2D mImg;
	void Awake()
	{
		
		
		//	if(AndroidCamera.instance.OnImagePicked==null){
		//PictureList.Clear();
		button.OnReleased += TakePicture;
		BackButton.OnReleased += exit;
	}
	
	void exit()
	{
		
	}
	// Use this for initialization
	void Start () {
		
	}
	
	void TakePicture()
	{
		Debug.Log ("Take Picture");
		mSound.playReportSound("click");
		if (PictureList.Count >= 3) return;
		//		AndroidCamera.instance.GetImageFromCamera();
		UnityPluginForWindowsPhone.BridgeWP.photoChooser ();
	}
	
	// Update is called once per frame
	void Update () {
		if(hasCache){
			
			Texture2D texture = new Texture2D (2,2);
			texture.LoadImage(cacheThumbnail);
			CreateReport(texture,cacheImage);
			hasCache = false;
			//		cacheImage = null;
		}
	}
	
	public void CreateReport(Texture2D _img,byte[] _data)
	{
		GameObject tmp = GameObject.Instantiate(PictureSrcPrefabs) as GameObject;
		ReportPicSrc comp = tmp.GetComponent<ReportPicSrc>();
		comp.SetImg(_img);
		comp.rawData = _data;
		comp.idx = PictureList.Count+1;
		tmp.transform.parent = PictureGal.transform;
		tmp.transform.localPosition = new Vector3(PictureList.Count * 1.5f,0,0);
		WarningText.SetActive(false);
		PictureList.Add(comp);
	}
	public void callbackPickedPhoto(bool isSuccess,  byte [] data  , byte [] thumbnailData){
		if (isSuccess) {
			cacheImage= data;
			cacheThumbnail = thumbnailData;
			hasCache = true;
		}
	}
}
#else
//editor version
public class ReportCamera : MonoBehaviour {
	//	public Collider2D buttonCollider;
	public ButtonObject button;
	public GameObject PictureGal;
	public GameObject PictureSrcPrefabs;
	public GameObject WarningText;
	public ButtonObject BackButton;
	
	public ReportMainSound mSound;
	
	public static List<ReportPicSrc> PictureList = new List<ReportPicSrc>();
	public Texture2D mImg;
	void Awake()
	{
		BackButton.OnReleased += exit;
		PictureList.Clear();
		button.OnReleased += TakePicture;
	}
	
	void exit()
	{
	}
	// Use this for initialization
	void Start () {
	}
	
	void TakePicture()
	{
		mSound.playReportSound("click");
		if (PictureList.Count >= 3) return;
		//set img to item
		CreateReport(mImg,mImg.EncodeToPNG());		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void CreateReport(Texture2D _img,byte[] _data)
	{
		GameObject tmp = GameObject.Instantiate(PictureSrcPrefabs) as GameObject;
		ReportPicSrc comp = tmp.GetComponent<ReportPicSrc>();
		comp.SetImg(_img);
		comp.rawData = _data;
		comp.idx = PictureList.Count+1;
		tmp.transform.parent = PictureGal.transform;
		tmp.transform.localPosition = new Vector3(PictureList.Count * 1.5f,0,0);
		WarningText.SetActive(false);
		PictureList.Add(comp);
	}	
}
#endif

