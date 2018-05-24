using UnityEngine;
using System.Collections;
using TMPro;

public class NewboardDetailLayer : MonoBehaviour {
	private AudioSource mSound;
	public NewsboardDetail nDetail;
	public NewsboardInfo contentInfo;
	
	public GameObject SwapInPts;
	public GameObject SwapOutPts;
	public GameObject NewsboardMain;

	public ButtonObject CloseButton;
	private string link;
	#if !UNITY_WEBGL && !DISABLE_WEBVIEW	
	public UniWebView _webView;
	#endif
	public float smooth = 1.0f;
	public GameObject loadObject;
	public void HideContent()
	{
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		_webView.Hide();
		#endif
		loadObject.SetActive (true);
		StopCoroutine ("ContentShow");
		StopCoroutine ("ContentHide");
		//start coroutine
		StartCoroutine ("ContentHide");
	}
	
	public void PopWebPage(string data)
	{
		link = data;
		Debug.Log("Open Web : "+link);
		this.gameObject.SetActive(true);
		setupData ();
		StopCoroutine ("ContentShow");
		StopCoroutine ("ContentHide");
		//start coroutine
		StartCoroutine ("ContentShow");
	}
	
	public void OpenDetail(NewsboardDetail data)
	{
		link = null;
		Debug.Log ("OpenDetail");
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		_webView.Hide();
		#endif
		nDetail = data;
		this.gameObject.SetActive(true);
		setupDetailData ();
		StopCoroutine ("ContentShow");
		StopCoroutine ("ContentHide");
		//start coroutine
		StartCoroutine ("ContentShow");
	}
	
	IEnumerator ContentShow()
	{ 
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
		NewsboardMain.SetActive(false);
//		if(nDetail.link_web!=""){
		if(link!=null){
			#if !UNITY_WEBGL && !DISABLE_WEBVIEW
			_webView.zoomEnable = true;
			_webView.Load();
			#endif
		}
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
		Debug.Log("Open Web : "+link.Replace(@"\/",@"/"));
//		_webView = GetComponent<UniWebView>();
//		if (_webView != null) {
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
			_webView = gameObject.AddComponent<UniWebView>();
			_webView.OnLoadComplete += OnLoadComplete;
			_webView.OnWebViewShouldClose += OnWebViewShouldClose;
		#endif
			CloseButton.OnReleased += CloseButtonPress;
//		}
		int offset_top = (int)Mathf.Floor (50.0f + ((4.8f - ScreenRatio.CameraSize)/3*5));
		Debug.Log (" "+offset_top.ToString()+" "+Screen.height.ToString()+" "+Screen.width.ToString());
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		_webView.insets = new UniWebViewEdgeInsets(offset_top,0,0,0);
		#endif
#if UNITY_ANDROID
//		_webView.url = "http://docs.google.com/gview?embedded=true&url=http://www.oryor.com/media/k2/pdfs/7695.pdf";
		_webView.url = link.Replace(@"\/",@"/");
//#elif UNITY_WINRT
//		UnityPluginForWindowsPhone.BridgeWP.webBrowserTask(mUrlForWp8);
#else
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		_webView.url = link.Replace(@"\/",@"/");
		#endif
#endif
	}
	void setupDetailData()
	{
		CloseButton.OnReleased += CloseButtonPress;
		loadObject.SetActive (false);
		contentInfo.SetContent (nDetail);
	}
	
	void Awake()
	{
//		loadControl.Play (0);
	}
	void CloseButtonPress()
	{
		NewsboardMain.SetActive(true);
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		_webView.OnLoadComplete -= OnLoadComplete;
		_webView.OnWebViewShouldClose -= OnWebViewShouldClose;
		#endif
		contentInfo.ClearContent ();
		mSound.Play();
		HideContent();
	}
#if !UNITY_WEBGL && !DISABLE_WEBVIEW
	void OnLoadComplete(UniWebView webView, bool success, string errorMessage) {
		loadObject.SetActive (false);
#if UNITY_ANDROID
		if(link.Contains("pdf") || link.Contains("PDF")){
			LoadingScript.HideLoading();
			CloseButtonPress();
		}
		else{
			if (success) {
				Debug.Log("Show Web");
				webView.Show();
			} else {
				Debug.Log("Something wrong in webview loading: " + errorMessage);
//				_errorMessage = errorMessage;
			}
		}
#else
		if (success) {
			Debug.Log("Show Web");
			webView.Show();
		} else {
			Debug.Log("Something wrong in webview loading: " + errorMessage);
//			_errorMessage = errorMessage;
		}
		#endif
	}
	
	
	//10. If the user close the webview by tap back button (Android) or toolbar Done button (iOS), 
	//    we should set your reference to null to release it. 
	//    Then we can return true here to tell the webview to dismiss.
	bool OnWebViewShouldClose(UniWebView webView) {
		if (webView == _webView) {
			_webView = null;
			return true;
		}
		return false;
	}
#endif

	// Use this for initialization
	void Start () {
		mSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
