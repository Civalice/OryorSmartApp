using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour {
	public string nextScene = "MainMenu";
	public SpriteRenderer BG;
	public SpriteRenderer Black;
	public SpriteRenderer Text;
	public SpriteRenderer Logo;
	public GameObject LoadingIcon;
	public static bool IsLoading = false;
	public static LoadingScript pGlobal;
	public static string sNextScene = "MainMenu";
	public static Vector3 loadingIconPos;

	private Color blackColor;
	private AsyncOperation scnLoadOp;

 	public static void ChangeScene(string scene)
	{
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		sNextScene = scene;
		#else
		sNextScene = scene;
		#endif
		pGlobal.RunChangeScene();
	}

	public static void ShowLoading()
	{
		if (pGlobal == null) return;
		Debug.Log("ShowLoading()");
		pGlobal.mShowLoading();
	}
	
	public static void HideLoading()
	{
		if (pGlobal == null) return;
		Debug.Log("HideLoading()");
		pGlobal.mHideLoading();
	}

	void mShowLoading()
	{
		IsLoading = true;
		StopCoroutine("LoadingIn");
		StopCoroutine("LoadingOut");
		StopCoroutine("TransitionIn");
		StopCoroutine("TransitionOut");
		StartCoroutine("LoadingIn");
	}

	void mHideLoading()
	{
		StopCoroutine("LoadingIn");
		StopCoroutine("LoadingOut");
		StopCoroutine("TransitionIn");
		StopCoroutine("TransitionOut");
		StartCoroutine("LoadingOut");
	}

	public IEnumerator LoadingIn()
	{
		while(Mathf.Abs(Black.color.a - blackColor.a) > 0.01f)
		{
			Black.color = Color.Lerp(Black.color,blackColor,Time.deltaTime*10);
			yield return null;
		}
		Black.color = blackColor;
		Vector3 targetPosition = loadingIconPos;
		while (Vector3.Distance(targetPosition,LoadingIcon.transform.localPosition) > 0.05f)
		{
			LoadingIcon.transform.localPosition = Vector3.Lerp(LoadingIcon.transform.localPosition,
			                                                   targetPosition,
			                                                   Time.deltaTime*10);
			yield return null;
		}
		LoadingIcon.transform.localPosition = targetPosition;
	}
	
	public IEnumerator LoadingOut()
	{
		Vector3 targetPosition = new Vector3(0,8,0);
		Color BlackTargetColor = new Color(Black.color.r,Black.color.g,Black.color.b,0);
		while (Vector3.Distance(targetPosition,LoadingIcon.transform.localPosition) > 0.05f)
		{
			LoadingIcon.transform.localPosition = Vector3.Lerp(LoadingIcon.transform.localPosition,
			                                                   targetPosition,
			                                                   Time.deltaTime*10);
			yield return null;
		}
		LoadingIcon.transform.localPosition = targetPosition;
		while (Mathf.Abs(Black.color.a - BlackTargetColor.a) > 0.01f)
		{
			Black.color = Color.Lerp(Black.color,BlackTargetColor,Time.deltaTime*10);
			yield return null;
		}
		Black.color = BlackTargetColor;
		IsLoading = false;
	}

	public void RunChangeScene()
	{
		IsLoading = true;
		StopCoroutine("TransitionIn");
		StopCoroutine("TransitionOut");
		StopCoroutine("LoadingIn");
		StopCoroutine("LoadingOut");

		StartCoroutine("TransitionIn");
	}

	public IEnumerator TransitionIn()
	{
		Color TargetColor = new Color(BG.color.r,BG.color.g,BG.color.b,1.0f);
		Color TextTargetColor = new Color(Text.color.r,Text.color.g,Text.color.b,1.0f);
		while ((Mathf.Abs(BG.color.a - TargetColor.a) > 0.01f)&&
		       (Mathf.Abs(Black.color.a - blackColor.a) > 0.01f)&&
		       (Mathf.Abs(Text.color.a - TextTargetColor.a) > 0.01f)
		       )
		{
			BG.color = Color.Lerp(BG.color,TargetColor,Time.deltaTime*10);
			Text.color = Color.Lerp(Text.color,TextTargetColor,Time.deltaTime*10);
			Logo.color = Text.color;
			Black.color = Color.Lerp(Black.color,blackColor,Time.deltaTime*10);
			yield return null;
		}
		BG.color = TargetColor;
		Text.color = TextTargetColor;
		Logo.color = Text.color;
		Black.color = blackColor;
		Vector3 targetPosition = loadingIconPos;
		while (Vector3.Distance(targetPosition,LoadingIcon.transform.localPosition) > 0.05f)
		{
			LoadingIcon.transform.localPosition = Vector3.Lerp(LoadingIcon.transform.localPosition,
			                                                   targetPosition,
			                                                   Time.deltaTime*10);
			yield return null;
		}
		LoadingIcon.transform.localPosition = targetPosition;
		scnLoadOp = Application.LoadLevelAsync("BlankScene");
		while (!scnLoadOp.isDone)
		{
			yield return null;
		}
		nextScene = sNextScene;
		StartCoroutine("LoadingNewScene");
	}

	public IEnumerator LoadingNewScene()
	{
		scnLoadOp = Application.LoadLevelAsync(nextScene);
		while (!scnLoadOp.isDone)
		{
			yield return null;
		}
		StopCoroutine("TransitionIn");
		StopCoroutine("TransitionOut");
		StopCoroutine("LoadingIn");
		StopCoroutine("LoadingOut");

		StartCoroutine("TransitionOut");
	}

	public IEnumerator TransitionOut()
	{
		Vector3 targetPosition = new Vector3(0,8,0);
		Color TargetColor = new Color(BG.color.r,BG.color.g,BG.color.b,0);
		Color BlackTargetColor = new Color(Black.color.r,Black.color.g,Black.color.b,0);
		Color TextTargetColor = new Color(Text.color.r,Text.color.g,Text.color.b,0);
		while (Vector3.Distance(targetPosition,LoadingIcon.transform.localPosition) > 0.05f)
		{
			LoadingIcon.transform.localPosition = Vector3.Lerp(LoadingIcon.transform.localPosition,
			                                                   targetPosition,
			                                                   Time.deltaTime*10);
			yield return null;
		}
		LoadingIcon.transform.localPosition = targetPosition;
		while ((Mathf.Abs(BG.color.a - TargetColor.a) > 0.01f)&&
		       (Mathf.Abs(Black.color.a - BlackTargetColor.a) > 0.01f)&&
		       (Mathf.Abs(Text.color.a - TextTargetColor.a) > 0.01f)
		       )
		{
			Black.color = Color.Lerp(Black.color,BlackTargetColor,Time.deltaTime*10);
			Text.color = Color.Lerp(Text.color,TextTargetColor,Time.deltaTime*10);
			Logo.color = Text.color;
			BG.color = Color.Lerp(BG.color,TargetColor,Time.deltaTime*10);
			yield return null;
		}
		Black.color = BlackTargetColor;
		Text.color = TextTargetColor;
		Logo.color = Text.color;
		BG.color = TargetColor;
		IsLoading = false;
	}
	// Use this for initialization
	void Awake()
	{
		pGlobal = this;
		Application.targetFrameRate = 60;
		blackColor = Black.color;
	}
	
	void Start () {
		IsLoading = true;
		loadingIconPos = LoadingIcon.transform.localPosition;
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
