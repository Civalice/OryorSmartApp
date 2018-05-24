using UnityEngine;
using System.Collections;
using TMPro;
using System.IO;
public class FavItem : MonoBehaviour {
	public TextMeshPro TitleText;
	public TextMeshPro DateText;
	public DownloadSprite texture;
	public WebViewButton WebButton;
	public ButtonObject ReadMoreButton;
	public DeleteFavButton DeleteButton;
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
		DeleteButton.SetContentData (cData);
		WebButton.SetContentData(cData,true);
		WebButton.SetButtonEnable(true);
		if (data.imgthumbnail.Length > 0)
		{
			DownloadThumbnail ("file://"+data.imgthumbnail);
		}
	}

	void DownloadThumbnail(string path)
	{
		if (path.Length <= 0) return;
		DownloadedTexture dTexture = ContentPageFile.CreateLoadTexture();
		dTexture.name = ContentPageFile.GetFileName (path);
		texture.AddDownloadTexture (dTexture,"FavThumb");
		dTexture.StartDownload (path);
	}
	void Awake()
	{
		ReadMoreButton.OnReleased += ReadMoreButtonPress;
	}
	// Use this for initialization
	void Start () {
		TitleText.maxVisibleLines = 4;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ReadMoreButtonPress()
	{
		PageDetailGlobal.PopFavouriteContentPage(cData);
	}
}
