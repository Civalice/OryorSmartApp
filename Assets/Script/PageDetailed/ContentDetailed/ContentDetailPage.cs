using UnityEngine;
using System.Collections;
using TMPro;

public class ContentDetailPage : MonoBehaviour {
	public ContentInfo contentInfo;
	public ContentMainSound mSound;

	public GameObject SwapInPts;
	public GameObject SwapOutPts;

	public ButtonObject LikeButton;
	public ButtonObject FavButton;
	public ButtonObject ShareButton;
	public ButtonObject CloseButton;

	public SpriteRenderer likeRenderer;
	public Sprite LikeOn;
	public Sprite LikeOff;
	public SpriteRenderer favRenderer;
	public Sprite FavOn;
	public Sprite FavOff;

	public TextMeshPro likeNumber;
	#if !UNITY_WEBGL && !DISABLE_WEBVIEW
	public UniWebView webView;
	#endif
	public GameObject Indicator;
	public float smooth = 1.0f;
	private ContentData currentData;
	private bool IsFavourite = false;
	private bool IsWeb = false;
	
	private string mUrlForWp8;
	public void HideContent()
	{
		#if UNITY_WINRT
		if (IsWeb)
		{
			webView.Hide();
		}
		contentInfo.ClearContent ();
		StartCoroutine ("ContentHide");
		PageDetailGlobal.ShowList ();
		
		#else
		if (IsWeb)
		{
			#if !UNITY_WEBGL && !DISABLE_WEBVIEW
				webView.Hide();
			#endif
		}
		contentInfo.ClearContent ();
		StopCoroutine ("ContentShow");
		StopCoroutine ("ContentHide");
		//start coroutine
		StartCoroutine ("ContentHide");
		PageDetailGlobal.ShowList ();
		#endif
	}

	public void PopPDFPage(ContentData data,bool favFlag)
	{
		IsFavourite = favFlag;
		this.gameObject.SetActive(true);
		contentInfo.gameObject.SetActive(false);
		currentData = data;
		//set Web Data
		IsWeb = true;
		Indicator.SetActive(true);
		setupData ();
		#if UNITY_WINRT
		UnityPluginForWindowsPhone.BridgeWP.webBrowserTask(mUrlForWp8);
		#else
			#if !UNITY_IOS
				StopCoroutine ("ContentShow");
				StopCoroutine ("ContentHide");
				//start coroutine
				StartCoroutine ("ContentShow");
			#else
				if(IsFavourite){
					StopCoroutine ("ContentShow");
					StopCoroutine ("ContentHide");
					//start coroutine
					StartCoroutine ("ContentShow");
				}
			#endif
		#endif
	}

	public void PopContent(ContentData data,bool favFlag)
	{
		IsFavourite = favFlag;
		this.gameObject.SetActive (true);
		contentInfo.gameObject.SetActive(true);
		currentData = data;
		IsWeb = false;
		Indicator.SetActive(false);
		setupData ();
		StopCoroutine ("ContentShow");
		StopCoroutine ("ContentHide");
		//start coroutine
		StartCoroutine ("ContentShow");
	}

	IEnumerator ContentShow()
	{ 
//#if UNITY_ANDROID
//		LoadingScript.ShowLoading();
//		webView.zoomEnable = true;
//		webView.Load();
//		yield return null;
//#else
		

		float time = 0;
		Vector3 nPosition = transform.position;
		Vector3 destPos = SwapInPts.transform.position;
		while (Vector3.Distance(nPosition,destPos) > 0.05f)
		{
			time+=Time.deltaTime*smooth;
			nPosition = Vector3.Lerp(transform.position,destPos,time);
			transform.position = nPosition;
			yield return null;
		}
		transform.position = destPos;
		if (IsWeb)
		{
			Indicator.SetActive(true);
			#if !UNITY_WEBGL && !DISABLE_WEBVIEW
			webView.zoomEnable = true;
			#endif
			
			
			int offset_top = (int)Mathf.Floor (50.0f + ((4.8f - ScreenRatio.CameraSize)/3*5));
			Debug.Log (" "+offset_top.ToString()+" "+Screen.height.ToString()+" "+Screen.width.ToString());
			#if !UNITY_WEBGL && !DISABLE_WEBVIEW
			webView.insets = new UniWebViewEdgeInsets(offset_top,0,0,0);
			webView.Load();
			#endif
		}
		PageDetailGlobal.HideList ();
//#endif
	}

	IEnumerator ContentHide()
	{ 
		float time = 0;
		Vector3 nPosition = transform.position;
		Vector3 destPos = SwapOutPts.transform.position;

		while (Vector3.Distance(nPosition,destPos) > 0.05f)
		{
			time+=Time.deltaTime*smooth;
			nPosition = Vector3.Lerp(transform.position,destPos,time);
			transform.position = nPosition;
			yield return null;
		}
		transform.position = destPos;
		this.gameObject.SetActive (false);
	}
	
	void setupData()
	{
		if (IsWeb)
		{
			#if !UNITY_WEBGL && !DISABLE_WEBVIEW
//			webView.url = currentData.pdfurl.Replace(@"\/",@"/");
			#if UNITY_IOS
			Debug.Log("IOS : "+currentData.pdfurl.Replace(@"\/",@"/"));
			if(!IsFavourite){
				Application.OpenURL(currentData.pdfurl.Replace(@"\/",@"/"));
			}
			else{
				webView.url = currentData.pdfurl.Replace(@"\/",@"/");
			}
			#elif UNITY_ANDROID
			webView.url = currentData.pdfurl.Replace(@"\/",@"/");
			#elif UNITY_WP8
			webView.url = mUrlForWp8;
			#endif
//			webView.url = "http://www.google.com";
			Debug.Log("Open Web : "+webView.url);
			#endif
		}
		else
		{
			if (IsFavourite) {
				contentInfo.SetContentFavourite (currentData);
			} else {
				contentInfo.SetContent (currentData);
				likeNumber.text = currentData.like.ToString ();;
			}
		}
	}

	void Awake()
	{
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		webView.OnLoadComplete += OnLoadComplete;
		#endif
		CloseButton.OnReleased += CloseButtonPress;
		LikeButton.OnReleased += LikeButtonPress;
		FavButton.OnReleased += FavButtonPress;
		ShareButton.OnReleased += ShareButtonPress;

	}
	
	// Use this for initialization
	void Start () {
		Indicator.SetActive (false);
	}

	void LikeButtonPress()
	{
		if (currentData == null) return;
		mSound.playContentSound("click");
		//check flag IsLike
		if (currentData.IsLike)
		{
			if (PageDetailGlobal.RemoveLike(currentData.dataid))
			{
				currentData.IsLike = false;
				currentData.like--;
			}
			else
			{
				Debug.Log("Error Occured");
				if (PageDetailGlobal.AddLike(currentData.dataid))
				{
					currentData.IsLike = true;
					currentData.like++;
				}
			}
		}
		else
		{
			if (PageDetailGlobal.AddLike(currentData.dataid))
			{
				currentData.IsLike = true;
				currentData.like++;
			}
			else
			{
				Debug.Log("Error Occured");
				if (PageDetailGlobal.RemoveLike(currentData.dataid))
				{
					currentData.IsLike = false;
					currentData.like--;
				}
			}
		}

	}

	void FavButtonPress()
	{
		mSound.playContentSound("click");
		if (currentData != null)
		{
			#if !UNITY_WEBGL && !DISABLE_WEBVIEW
			webviewhide();
			if (!currentData.IsFav){
				PageDetailGlobal.AddFavourite (currentData,IsWeb);
			}
			else{
				PopupObject.ShowAlertPopup("Error","ข้อมูลนี้ได้ถูกบันทึกไปแล้ว","ปิด",webviewhide);
			}
			#endif
		}
	}
	#if !UNITY_WEBGL && !DISABLE_WEBVIEW
	public void webviewshow(){
		webView.Show ();
	}
	public void webviewhide(){
		webView.Hide ();
	}
	#endif

	void ShareButtonPress()
	{
		mSound.playContentSound("click");
		if (currentData != null)
		{
			string title = StringUtil.ParseUnicodeEscapes(currentData.title).Replace(@"  ",@"|||").Replace(@" ",@"").Replace(@"|||",@" ");
			string detail = StringUtil.ParseUnicodeEscapes(currentData.detail[0]).Replace(@"  ",@"|||").Replace(@" ",@"").Replace(@"|||",@" ");
			FacebookLogin.pGlobal.PostFacebook(null,
			                                   title,
			                                   title,
			                                   detail,
			                                   currentData.imgthumbnail,
			                                   "http://www.oryor.com/"+currentData.weburl);		
		}
	}

	void CloseButtonPress()
	{
		mSound.playContentSound("click");
		PageDetailGlobal.HideContentPage();
	}

	// Update is called once per frame
	void Update () {
		if (currentData!=null)
		{
			if (currentData.IsLike)
			{
				likeRenderer.sprite = LikeOn;
			}
			else
			{
				likeRenderer.sprite = LikeOff;
			}
			if (currentData.IsFav)
			{
				favRenderer.sprite = FavOn;
			}
			else
			{
				favRenderer.sprite = FavOff;
			}

			likeNumber.text = currentData.like.ToString();
		}

	}
	#if !UNITY_WEBGL && !DISABLE_WEBVIEW
	void OnLoadComplete(UniWebView webView, bool success, string errorMessage) {
#if UNITY_ANDROID
		LoadingScript.HideLoading();
		PageDetailGlobal.HideContentPage();
#else
		if (success) {
			Debug.Log("Show Web");
			Indicator.SetActive(false);
			webView.Show();
		} else {
			Debug.Log("Something wrong in webview loading: " + errorMessage);
//			_errorMessage = errorMessage;
		}
#endif
	}
	#endif
}
