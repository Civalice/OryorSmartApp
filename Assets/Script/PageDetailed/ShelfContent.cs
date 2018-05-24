using UnityEngine;
using System.Collections;
using TMPro;

public class ShelfContent : MonoBehaviour {
	public TextMeshPro TitleText;
	public TextMeshPro DetailText;
	public TextMeshPro likeNumber;
//	public GameObject MoreButton;
	public GameObject WebButton;

	public ButtonObject ButtonLike;
	public SpriteRenderer LikeRenderer;
	public Sprite LikeOn;
	public Sprite LikeOff;
	public SpriteRenderer FavRenderer;
	public Sprite FavOn;
	public Sprite FavOff;
	public ButtonObject ButtonFavourite;
	public ButtonObject ButtonShared;
	public ButtonObject MoreButton;

	public ContentMainSound mSound;

	private Color32 TitleTextColor;
	private Color32 DetailTextColor;
	private float mTitleLineLength;
	private float mDetailLineLength;
	private ContentData cData;
	public float smooth = 1.0f;
	// Use this for initialization
	void Start () {
		ButtonFavourite.OnReleased += AddFavourite;
		ButtonLike.OnReleased += PressLike;
		ButtonShared.OnReleased += PressShare;
		MoreButton.OnReleased += PressReadMore;

		TitleTextColor = TitleText.color;
		//mTitleLineLength = TitleText.lineLength;
		DetailTextColor = DetailText.color;
		//mDetailLineLength = DetailText.lineLength;
	}

	public void ShowContent()
	{
		StopCoroutine ("HidingText");
		StopCoroutine ("ShowingText");
//		MoreButton.GetComponent<ReadMoreButton>().SetButtonEnable(true);
		WebButton.GetComponent<WebViewButton>().SetButtonEnable(true);

		if (this.gameObject.activeInHierarchy) {
			StartCoroutine ("ShowingText");
		} 
		else 
		{
//			TitleText.gameObject.SetActive(true);
			TitleText.color = TitleTextColor;

			DetailText.color = DetailTextColor;
			MoreButton.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
			WebButton.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);

		}
		
	}
	public void HideContent()
	{
		StopCoroutine ("HidingText");
		StopCoroutine ("ShowingText");
//		MoreButton.GetComponent<ReadMoreButton>().SetButtonEnable(false);
		WebButton.GetComponent<WebViewButton>().SetButtonEnable(false);

		if (this.gameObject.activeInHierarchy) {
						StartCoroutine ("HidingText");
				} else {
					TitleText.color = new Color32(TitleTextColor.r,TitleTextColor.g,TitleTextColor.b,0);
//			TitleText.gameObject.SetActive(false);
					DetailText.color = new Color32(DetailTextColor.r,DetailTextColor.g,DetailTextColor.b,0);
					MoreButton.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
					WebButton.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
			}
		}

	IEnumerator ShowingText()
	{
		float time = 0;
		Color32 mTColor = TitleText.color;
		Color32 mDColor = DetailText.color;
		Color32 mSColor = MoreButton.GetComponent<SpriteRenderer> ().color;
//		TitleText.gameObject.SetActive(true);
		while ((mTColor.a < 255)||(mDColor.a < 255)||(mSColor.a > 0)) {
			time+=Time.deltaTime*smooth;
			mTColor = Color32.Lerp(mTColor,new Color32(mTColor.r,mTColor.g,mTColor.b,255),time);
			mDColor = Color32.Lerp(mDColor,new Color32(mDColor.r,mDColor.g,mDColor.b,255),time);
			mSColor = Color32.Lerp(mSColor,new Color32(255,255,255,255),time);
			TitleText.color = mTColor;
			DetailText.color = mDColor;
			MoreButton.GetComponent<SpriteRenderer> ().color = mSColor;
			WebButton.GetComponent<SpriteRenderer> ().color = mSColor;
			yield return null;
		}
	}

	public void SetContent(ContentData data)
	{
		cData = data;
		cData.IsLike = PageDetailGlobal.GetLikeAlready(data.dataid);
		cData.IsFav = PageDetailGlobal.IsFavourite(data.dataid);
		if (cData.IsLike)
		{
			LikeRenderer.sprite = LikeOn;
		}
		else
		{
			LikeRenderer.sprite = LikeOff;
		}
		if (cData.IsFav)
		{
			FavRenderer.sprite = FavOn;
		}
		else
		{
			FavRenderer.sprite = FavOff;
		}
		likeNumber.text = cData.like.ToString ();
		setTitleText (cData.title);
		setDetailText (cData.detail [0]);
		WebButton.GetComponent<WebViewButton>().SetContentData(cData,false);
	}

	void setTitleText (string text)
	{
		//TitleText.lineLength = mTitleLineLength;
		TitleText.text = "<b>"+StringUtil.ParseUnicodeEscapes(text)+"</b>";;
	}

	void setDetailText (string text)
	{
		//DetailText.lineLength = mDetailLineLength;
		DetailText.text = StringUtil.ParseUnicodeEscapes(text);
	}

	IEnumerator HidingText()
	{
		float time = 0;
		Color32 mTColor = TitleText.color;
		Color32 mDColor = DetailText.color;
		Color32 mSColor = MoreButton.GetComponent<SpriteRenderer> ().color;
		while ((mTColor.a > 0)||(mDColor.a > 0)||(mSColor.a > 0)) {
			yield return null;
			time+=Time.deltaTime*smooth;
			mTColor = Color32.Lerp(mTColor,new Color32(mTColor.r,mTColor.g,mTColor.b,0),time);
			mDColor = Color32.Lerp(mDColor,new Color32(mDColor.r,mDColor.g,mDColor.b,0),time);
			mSColor = Color32.Lerp(mSColor,new Color32(255,255,255,0),time);
			TitleText.color = mTColor;
			DetailText.color = mDColor;
			MoreButton.GetComponent<SpriteRenderer> ().color = mSColor;
			WebButton.GetComponent<SpriteRenderer> ().color = mSColor;
		}	
//		TitleText.gameObject.SetActive(false);
	}
	void AddFavourite()
	{
		if (cData != null)
		{
			mSound.playContentSound("click");

			if (!cData.IsFav)
				PageDetailGlobal.AddFavourite (cData);
			else
				PopupObject.ShowAlertPopup("Error","ข้อมูลนี้ได้ถูกบันทึกไปแล้ว","ปิด");
		}
	}

	void PressLike()
	{
		if (cData==null) return;
		mSound.playContentSound("click");
		//check flag IsLike
		if (cData.IsLike)
		{
			if (PageDetailGlobal.RemoveLike(cData.dataid))
			{
				cData.IsLike = false;
				//1. decrease like
				cData.like--;
				//2. sent url request
				//3. unlike button
				LikeRenderer.sprite = LikeOff;
				//4. save to ContentPageFile
			}
			else
			{
				Debug.Log("Error Occured");
				if (PageDetailGlobal.AddLike(cData.dataid))
				{
					cData.IsLike = true;
					//1. increase like
					cData.like++;
					//2. sent url request
					//3. like button
					LikeRenderer.sprite = LikeOn;
					//4. save to ContentPageFile
				}
			}
		}
		else
		{
			if (PageDetailGlobal.AddLike(cData.dataid))
			{
				cData.IsLike = true;
				//1. increase like
				cData.like++;
				//2. sent url request
				//3. like button
				LikeRenderer.sprite = LikeOn;
				//4. save to ContentPageFile
			}
			else
			{
				Debug.Log("Error Occured");
				if (PageDetailGlobal.RemoveLike(cData.dataid))
				{
					cData.IsLike = false;
					//1. decrease like
					cData.like--;
					//2. sent url request
					//3. unlike button
					LikeRenderer.sprite = LikeOff;
					//4. save to ContentPageFile
				}
			}
		}
	}

	void PressShare()
	{
		Debug.Log ("Test Click Share");
		if (cData!=null)
		{
			Debug.Log ("Click Share : "+cData.weburl);
			mSound.playContentSound("click");
			string title = StringUtil.ParseUnicodeEscapes(cData.title).Replace(@"  ",@"|||").Replace(@" ",@"").Replace(@"|||",@" ");
			string detail = StringUtil.ParseUnicodeEscapes(cData.detail[0]).Replace(@"  ",@"|||").Replace(@" ",@"").Replace(@"|||",@" ");
			FacebookLogin.pGlobal.PostFacebook(null,
			                                   title,
			                                   title,
			                                   detail,
			                                   cData.imgthumbnail,
			                                   cData.weburl);
		}
	}

	void PressReadMore()
	{
		mSound.playContentSound("click");
		PageDetailGlobal.PopContentPage(cData);
	}
	// Update is called once per frame
	void Update () {
		if (cData!=null)
		{
			if (cData.IsLike)
			{
				LikeRenderer.sprite = LikeOn;
			}
			else
			{
				LikeRenderer.sprite = LikeOff;
			}
			likeNumber.text = cData.like.ToString();
			if (cData.IsFav)
			{
				FavRenderer.sprite = FavOn;
			}
			else
			{
				FavRenderer.sprite = FavOff;
			}
		}
	}
}
