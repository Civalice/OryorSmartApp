using UnityEngine;
using System.Collections;
using TMPro;

public class FullListItem : MonoBehaviour {
	public ButtonObject LikeButton;
	public ButtonObject FavButton;
	public ButtonObject ShareButton;
	public ButtonObject ReadMoreButton;
	public WebViewButton WebButton;

	public SpriteRenderer LikeRenderer;
	public Sprite LikeOn;
	public Sprite LikeOff;
	public SpriteRenderer FavRenderer;
	public Sprite FavOn;
	public Sprite FavOff;

	public TextMeshPro TitleText;
	public TextMeshPro DateText;
	private ContentData cData;

	void SetText(string title,string detail)
	{
		TitleText.text = "<b>" + StringUtil.ParseUnicodeEscapes(title) + "</b>\n" + StringUtil.ParseUnicodeEscapes(detail);
	}
	void SetDate (string date)
	{
		DateText.text = date;
	}

	public void SetContentData(ContentData data)
	{
		cData = data;
		SetText (data.title, data.detail [0]);
		SetDate (data.publishdate.Replace (@" ", @"  "));
		WebButton.SetContentData(data,false);
		WebButton.SetButtonEnable(true);
	}

	// Use this for initialization
	void Start () {
		TitleText.maxVisibleLines = 4;
		LikeButton.OnReleased += PressLike;
		FavButton.OnReleased += PressFav;
		ShareButton.OnReleased += PressShare;
		ReadMoreButton.OnReleased += PressReadMore;
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

	void PressLike()
	{
		//check flag IsLike
		if (cData.IsLike)
		{
			if (PageDetailGlobal.RemoveLike(cData.dataid))
			{
				cData.IsLike = false;
				cData.like--;
			}
			else
			{
				Debug.Log("Error Occured");
				if (PageDetailGlobal.AddLike(cData.dataid))
				{
					cData.IsLike = true;
					cData.like++;
				}
			}
		}
		else
		{
			if (PageDetailGlobal.AddLike(cData.dataid))
			{
				cData.IsLike = true;
				cData.like++;
			}
			else
			{
				Debug.Log("Error Occured");
				if (PageDetailGlobal.RemoveLike(cData.dataid))
				{
					cData.IsLike = false;
					cData.like--;
				}
			}
		}
	}

	void PressFav()
	{
		if (cData != null)
		{
			if (!cData.IsFav)
				PageDetailGlobal.AddFavourite (cData);
			else
				PopupObject.ShowAlertPopup("Error","ข้อมูลนี้ได้ถูกบันทึกไปแล้ว","ปิด");

		}
	}

	void PressShare()
	{
		if (cData != null)
		{
			string title = StringUtil.ParseUnicodeEscapes(cData.title).Replace(@"  ",@"|||").Replace(@" ",@"").Replace(@"|||",@" ");
			string detail = StringUtil.ParseUnicodeEscapes(cData.detail[0]).Replace(@"  ",@"|||").Replace(@" ",@"").Replace(@"|||",@" ");
			FacebookLogin.pGlobal.PostFacebook(null,
			                                   title,
			                                   title,
			                                   detail,
			                                   cData.imgthumbnail,
			                                   "http://www.oryor.com/"+cData.weburl);		}
	}

	void PressReadMore()
	{
		PageDetailGlobal.PopContentPage(cData);
	}
}
