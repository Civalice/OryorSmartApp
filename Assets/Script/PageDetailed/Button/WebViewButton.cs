using UnityEngine;
using System.Collections;
public enum LinkType
{
	LINK_NULL = 0,
	LINK_PDF,
	LINK_MOVIE
};
public class WebViewButton : MonoBehaviour {
	public Collider2D ButtonCollider;
	public Collider2D box = null;
	public ContentMainSound mSound;
	public ContentData cData;
	public Sprite PlayVideo;
	public Sprite OpenPDF;
	private bool IsEnable = false;
	private bool IsFavourite = false;

	private LinkType link = LinkType.LINK_NULL;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		if (!IsEnable)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (ButtonCollider.OverlapPoint(touchPos) && ((box == null)||((box != null)&&(box.OverlapPoint(touchPos)))))
			{
				Debug.Log ("WebViewButton Click");
				if (mSound != null)
					mSound.playContentSound("click");
				switch(link)
				{
					case LinkType.LINK_MOVIE:
					{
					string url = cData.vdourl.Replace(@"\/",@"/");
					#if UNITY_WINRT
						UnityPluginForWindowsPhone.BridgeWP.mediaPlayerLauncher(url);
					#else
						#if !UNITY_WEBGL && !DISABLE_WEBVIEW
							Handheld.PlayFullScreenMovie(url);
						#endif
					#endif
					}break;
					case LinkType.LINK_PDF:
					{
						Debug.Log ("WebViewButton Link PDF");
						if (IsFavourite) {
							Debug.Log ("WebViewButton Favorite");
							PageDetailGlobal.PopFavouritePDFPage (cData);
						} else {
							Debug.Log ("WebViewButton Normal");
							PageDetailGlobal.PopPDFPage (cData);
						}
					}break;
					case LinkType.LINK_NULL:
					{
					}break;
				}
			}
		}
	}
	public void SetContentData(ContentData data,bool FavFlag)
	{
		IsFavourite = FavFlag;
		cData = data;
		SpriteRenderer spr = GetComponent<SpriteRenderer>();
		//check Video
		if (cData.vdourl.Length > 0)
		{
			link = LinkType.LINK_MOVIE;
			gameObject.SetActive(true);
			spr.sprite = PlayVideo;
		}
		else if (cData.pdfurl.Length > 0)
		{
			link = LinkType.LINK_PDF;
			gameObject.SetActive(true);
			spr.sprite = OpenPDF;
		}
		else
		{
			link = LinkType.LINK_NULL;
			gameObject.SetActive(false);
		}
	}
	
	public void SetButtonEnable(bool flag)
	{
		IsEnable = flag;
	}
}
