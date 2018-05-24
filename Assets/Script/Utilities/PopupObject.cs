using UnityEngine;
using System.Collections;
using TMPro;

public class PopupObject : MonoBehaviour {
	// three button maximum
	public delegate float ProgressCB(out string text);
	public delegate void FinishCB();

	public ProgressCB TextProgressCB;
	public FinishCB progressDoneCB;

	public SpriteRenderer Black;
	public GameObject PopupMenu;
	public GameObject PopupBG;
	public PopupButton Button1;
	public PopupButton Button2;
	public PopupButton Button3;

	public TextMeshPro headerText;
	public TextMeshPro detailText;

	public GameObject SwapInObj;
	public GameObject SwapOutObj;

	public AudioClip popupSound;

	private Vector3 SwapInPts;
	private Vector3 SwapOutPts;

	public static PopupObject instance;
	public static bool IsPopup = false;
	private Color blackColor;
	private float Button1origin;
	private float Button2origin;
	private float Button3origin;
	private float headerOrigin;
	private float detailOrigin;

	const float defaultSize = 1.47f;

	public static void ShowWaitingPopup(string Header,
	                                    string bt1Text,
	                                    FinishCB finishCB = null,
	                                    ProgressCB cb = null,
	                                    PopupButton.ButtonAction cancelCB = null)
	{
		instance.SetupText(Header,"");
		instance.SetupButton(bt1Text,"","");
		instance.TextProgressCB = cb;
		instance.Button1.OnReleased = cancelCB;
		instance.progressDoneCB = finishCB;
		instance.mShowProgress();
	}

	public static void ShowAlertPopup(string Header,
	                                  string Detail,
	                                  string bt1Text,
	                                  PopupButton.ButtonAction bt1Action = null,
	                                  string bt2Text = "",
	                                  PopupButton.ButtonAction bt2Action = null,
	                                  string bt3Text = "",
	                                  PopupButton.ButtonAction bt3Action = null
	                                  )
	{
		Debug.Log("Show Alert Box : "+Header);
		instance.SetupText(Header,Detail);
		instance.SetupButton(bt1Text,bt2Text,bt3Text);
		instance.Button1.OnReleased = bt1Action;
		instance.Button2.OnReleased = bt2Action;
		instance.Button3.OnReleased = bt3Action;
		instance.mShow();
	}

	public static void Hide()
	{
		instance.mHide();
	}

	public void SetupText(string header,string detail)
	{
		headerText.text = "<b>"+header.Replace(@" ",@"  ")+"</b>";
		detailText.text = detail.Replace(@" ",@"  ");
		detailText.ForceMeshUpdate();
		//GetBoundery
		{
			//3.2 = current size with 1 scale ratio
			float diff = detailText.bounds.size.y - defaultSize;
			if (diff < 0) diff = 0;
			float scale = 1 + diff/3.2f;
			PopupBG.transform.localScale = new Vector3(PopupBG.transform.localScale.x,scale,1);
			Button1.transform.localPosition = new Vector3(Button1.transform.localPosition.x,
			                                              Button1origin - diff/2,
			                                              0);
			Button2.transform.localPosition = new Vector3(Button2.transform.localPosition.x,
			                                              Button2origin - diff/2,
			                                              0);
			Button3.transform.localPosition = new Vector3(Button3.transform.localPosition.x,
			                                              Button3origin - diff/2,
			                                              0);
			headerText.transform.localPosition = new Vector3(headerText.transform.localPosition.x,
			                                                 headerOrigin + diff/2,
			                                              0);
			detailText.transform.localPosition = new Vector3(detailText.transform.localPosition.x,
			                                                 detailOrigin + diff/2,
			                                                 0);

		}
	}

	public void SetupButton(string bt1Text,
	                        string bt2Text,
	                        string bt3Text)
	{
		Button1.SetText(bt1Text);
		if (bt2Text != "")
		{
			Button2.SetText(bt2Text);
			Button2.gameObject.SetActive(true);
		}
		else
		{
			Button2.gameObject.SetActive(false);
		}
		if (bt3Text != "")
		{
			Button3.SetText(bt3Text);
			Button3.gameObject.SetActive(true);
		}
		else
		{
			Button3.gameObject.SetActive(false);
		}

	}

	public void mShow()
	{
		IsPopup = true;
		GetComponent<AudioSource>().PlayOneShot(popupSound);
		Button1.Enable();
		Button2.Enable();
		Button3.Enable();
		StopCoroutine("PopIn");
		StopCoroutine("PopOut");
		StopCoroutine("CheckProgress");
		StartCoroutine("PopIn");
	}

	public void mShowProgress()
	{
		IsPopup = true;
		Button1.Enable();
		StopCoroutine("PopIn");
		StopCoroutine("PopOut");
		StartCoroutine("PopIn");
		StartCoroutine("CheckProgress");
	}
	
	IEnumerator CheckProgress()
	{
		if (TextProgressCB != null)
		{
			string detailTxt = "";
			while (TextProgressCB(out detailTxt)<1.0f)
			{
				detailText.text = detailTxt.Replace(@" ",@"  ");
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
			if (progressDoneCB != null)
				progressDoneCB();
		}
		mHide();
	}

	IEnumerator PopIn()
	{
		while(Mathf.Abs(Black.color.a - blackColor.a) > 0.01f)
		{
			Black.color = Color.Lerp(Black.color,blackColor,Time.deltaTime*10);
			yield return null;
		}
		Black.color = blackColor;

		Vector3 targetPosition = SwapInPts;
		while (Vector3.Distance(PopupMenu.transform.localPosition,targetPosition) > 0.05f)
		{
			PopupMenu.transform.localPosition = Vector3.Lerp(PopupMenu.transform.localPosition,targetPosition,Time.deltaTime*10);
			yield return null;
		}
		PopupMenu.transform.localPosition = targetPosition;
		yield return null;
	}

	public void mHide()
	{
		Button1.Disable();
		Button2.Disable();
		Button3.Disable();
		StopCoroutine("PopIn");
		StopCoroutine("PopOut");
		StopCoroutine("CheckProgress");
		StartCoroutine("PopOut");
	}

	IEnumerator PopOut()
	{
		Vector3 targetPosition = SwapOutPts;
		while (Vector3.Distance(PopupMenu.transform.localPosition,targetPosition) > 0.05f)
		{
			PopupMenu.transform.localPosition = Vector3.Lerp(PopupMenu.transform.localPosition,targetPosition,Time.deltaTime*10);
			yield return null;
		}
		PopupMenu.transform.localPosition = targetPosition;
		Color BlackTargetColor = new Color(Black.color.r,Black.color.g,Black.color.b,0);

		while (Mathf.Abs(Black.color.a - BlackTargetColor.a) > 0.01f)
		{
			Black.color = Color.Lerp(Black.color,BlackTargetColor,Time.deltaTime*10);
			yield return null;
		}
		Black.color = BlackTargetColor;
		IsPopup = false;
		yield return null;
	}

	void Awake ()
	{
		instance = this;
		DontDestroyOnLoad(this);
		blackColor = Black.color;
		SwapInPts = SwapInObj.transform.localPosition;
		SwapOutPts = SwapOutObj.transform.localPosition;
		Button1.HideEvent = mHide;
		Button2.HideEvent = mHide;
		Button3.HideEvent = mHide;
		Button1.Disable();
		Button2.Disable();
		Button3.Disable();
		PopupMenu.transform.localPosition = SwapOutPts;
		Black.color = new Color(Black.color.r,Black.color.g,Black.color.b,0);
		Button1origin = Button1.transform.localPosition.y;
		Button2origin = Button1.transform.localPosition.y;
		Button3origin = Button1.transform.localPosition.y;
		headerOrigin = headerText.transform.localPosition.y;
		detailOrigin = detailText.transform.localPosition.y;
		gameObject.SetActive(true);
	}

	// Use this for initialization
	void Start () {
		Debug.Log(detailText.bounds.size.y);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
