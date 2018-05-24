using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class LinkStructure
{
	public string Header;
	public string Link;
};
[System.Serializable]
public class GroupStructure
{
	public ContentType[] TypeList;
};
public enum DetailMode
{
	DM_SHELF = 0,
	DM_FULLLIST,
	DM_FAVOURITE
};
public enum DetailState
{
	DS_LIST = 0,
	DS_CONTENT,
	DS_SUBMENU,
	DS_FILTER,
	DS_SEARCH,
	DS_WAITING
};

public class PageDetailGlobal : MonoBehaviour {
	public int MaxContentPerPage = 15;

	static public Color pageColor = new Color32(16,112,137,255);
	static public ContentType type = ContentType.Null;

	public GameObject PageDetail;
	public GameObject waitingDialogue;
	public GameObject HeadbarButton;
	public FooterPageBar FooterBar;
	public SubMenuButtonList subMenu;
	public FilterButtonList filter;
	public ShelfInteractive ShelfDetail;
	public ItemListController FullListDetail;
	public FavListController FavListDetail;
	public XMLDownloader xmlData;
	public ContentDetailPage contentPage;
	public TextMeshPro HeaderText;

	public ContentPageFile file;
	public List<DownloadedTexture> mDownloadTexList;

	public LinkStructure[] Link;
	public GroupStructure[] GroupList;
	public string[] FilterList;

	private int CurrentPageSize = 0;
	private bool pchk;

	static public DetailMode mode;
	static public DetailState state = DetailState.DS_LIST;
	static public DetailState prevState;
	static public PageDetailGlobal pGlobal;
	
	public GameObject Indecator = null;
	static public void PrevPage()
	{
		pGlobal.FooterBar.PrevPage();
	}
	static public void NextPage()
	{
		pGlobal.FooterBar.NextPage();
	}
	static public bool IsFinishLoading()
	{
		return pGlobal.xmlData.isFinish;
	}

	static public bool GetLikeAlready(int id)
	{
		foreach (int mId in pGlobal.file.likeList)
		{
			if (mId == id) return true;
		}
		return false;
	}
	static public bool IsFavourite(int id)
	{
		return pGlobal.file.IsFavourite(id);
	}
	static public bool AddLike(int id)
	{
		foreach (int mId in pGlobal.file.likeList)
		{
			if (mId == id) 
				return false;
		}
		//add like list
		SendLike(id,true);
		pGlobal.file.likeList.Add(id);
		ContentPageFile.SaveContentFile();
		return true;
	}
	static public bool RemoveLike(int id)
	{
		if (pGlobal.file.likeList.Remove(id))
		{
			SendLike(id,false);
			ContentPageFile.SaveContentFile();
			return true;
		}
		return false;
	}
	static public void SendLike(int id,bool flag)
	{
		//type = -1 -> Not smartTips
		string name = (flag)?"LikeSender":"UnlikeSender";
		GameObject obj = new GameObject(name,typeof(LikeAPI));
		obj.transform.parent = pGlobal.transform;
		LikeAPI api = obj.GetComponent<LikeAPI>();
		//setup [LIKEURL] here
		switch(type)
		{
		case ContentType.SmartTips_Banner:
		case ContentType.SmartTips_Choose:
		case ContentType.SmartTips_GDA:
		case ContentType.SmartTips_KnowOryor:
		{
//			string baseURL = "http://www.oryor.com/oryor_smart_app_year2/ws_client_year2v1.php?task=updateInfo";
			string baseURL = UserCommonData.GetURL()+"?task=updateInfo";
			api.LikeURL = baseURL + "&type="+(int)type+"&id="+id+"&like="+(flag?1:-1);
		}
		break;
		default:
		{
//			string baseURL = "http://www.oryor.com/newweb/webservice_oryor/service_year2v1.php?task=updateInfo";
			string baseURL = UserCommonData.GetURL()+"?task=updateInfo";
			api.LikeURL = baseURL+"&dataId="+id+"&like="+(flag?1:-1);
		}
		break;
		}

		api.SendLike();
	}
	static public void AddFavourite(ContentData cData,bool chk=false)
	{
		prevState = state;
		pGlobal.ShowDownloadChoice(cData);
		Debug.Log ("chk : "+chk);
		pGlobal.pchk = chk;
	}
	private ContentData FavData;
	
	public void ShowDownloadChoice(ContentData cData)
	{
		FavData = cData;
		PopupObject.ShowAlertPopup("Favourite","คุณต้องการ Download รูปแบบไหน?",
		                           "ยกเลิก",CancelCB,
		                           "บางส่วน",DownloadSomeCB,
		                           "ทั้งหมด",DownloadAllCB);
	}

	public void StartLoading()
	{
		PopupObject.ShowWaitingPopup("Download","ปิด",CreateFavourite,
		                             ProgressCB,CancelDownloadCB);
	}

	public float ProgressCB(out string text)
	{
		//calculate progress here
		float progress = ContentPageFile.GetProgress();
		int prText = (int)(progress*100);
		text = "Downloading ("+prText.ToString()+"%)";
		if (progress >= 1.0f)
		{
			//finish download
//			CreateFavourite ();
			state = prevState;
		}
		return progress;
	}

	public void DownloadSomeCB()
	{
		if (!pGlobal.file.AddContentToFile (FavData,false)) {
			PopupObject.ShowAlertPopup("Error","ข้อมูลนี้ได้ถูกบันทึกไปแล้ว","ปิด");
		} else {
			state = DetailState.DS_WAITING;
			pGlobal.StartLoading();
		}
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		if(pGlobal.pchk)
			contentPage.webviewshow ();
		#endif
	}

	public void DownloadAllCB()
	{
		if (!pGlobal.file.AddContentToFile (FavData,true)) {
			PopupObject.ShowAlertPopup("Error","ข้อมูลนี้ได้ถูกบันทึกไปแล้ว","ปิด");
		} else {
			state = DetailState.DS_WAITING;
			pGlobal.StartLoading();
		}
		Debug.Log ("pchk : "+pGlobal.pchk);
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		if(pGlobal.pchk)
			contentPage.webviewshow ();
		#endif
	}

	public void CancelCB()
	{
		state = prevState;
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		if(pGlobal.pchk)
			contentPage.webviewshow ();
		#endif
	}

	public void CancelDownloadCB()
	{
		state = prevState;
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
		if(pGlobal.pchk)
			contentPage.webviewshow ();
		#endif
	}

	static public void RemoveFavourite(ContentData cData)
	{
		cData.IsFav = false;
		pGlobal.file.RemoveContent (cData);
		foreach (ContentData data in pGlobal.xmlData.contentList)
		{
			data.IsFav = IsFavourite(data.dataid);
		}
		CreateFavourite ();
	}
	static public void CreateFavourite()
	{
		pGlobal.FavListDetail.ClearItemDetail();
		ContentPageFile.ClearLoadTexture ();
		pGlobal.FavListDetail.CreateFavItem (pGlobal.file.contentList.ToArray ());
	}
	static public void HideSubMenu()
	{
		pGlobal.subMenu.Trigger (false);
	}
	static public void HideFilter()
	{
		pGlobal.filter.Trigger (false);
	}
	// Use this for initialization
	static public GroupStructure GetGroupStruct()
	{
		foreach (GroupStructure str in pGlobal.GroupList) {
			for(int i = 0;i < str.TypeList.Length;i++)
			{
				if (str.TypeList[i] == type)
					return str;
			}
		}
		return null;
	}

	static public void PopFavouritePDFPage(ContentData data)
	{
		#if !UNITY_IOS
		state = DetailState.DS_CONTENT;
		#endif
		pGlobal.contentPage.PopPDFPage(data,true);
	}
	static public void PopPDFPage(ContentData data)
	{
		#if !UNITY_IOS
		state = DetailState.DS_CONTENT;
		#endif
		pGlobal.contentPage.PopPDFPage(data,false);
	}

	static public void PopFavouriteContentPage(ContentData data)
	{
		state = DetailState.DS_CONTENT;
		pGlobal.contentPage.PopContent (data,true);
	}

	static public void PopContentPage(ContentData data)
	{
		state = DetailState.DS_CONTENT;
		pGlobal.contentPage.PopContent (data,false);
	}

	static public void HideContentPage()
	{
		state = DetailState.DS_LIST;
		pGlobal.contentPage.HideContent ();
	}

	static public void HideList()
	{
		pGlobal.ShelfDetail.gameObject.SetActive (false);
		pGlobal.FullListDetail.gameObject.SetActive (false);
		pGlobal.PageDetail.SetActive (false);
	}

	static public void ShowList ()
	{
		switch (mode) {
		case DetailMode.DM_FULLLIST:
			ShowFullList();
			break;
		case DetailMode.DM_SHELF:
			ShowShelf();
			break;
		}
		pGlobal.PageDetail.SetActive (true);
	}

	static public void ShowShelf()
	{
		mode = DetailMode.DM_SHELF;
		pGlobal.HeadbarButton.SetActive (true);
		pGlobal.ShelfDetail.gameObject.SetActive (true);
		pGlobal.FullListDetail.gameObject.SetActive (false);
		pGlobal.FavListDetail.gameObject.SetActive (false);
		pGlobal.FooterBar.gameObject.SetActive (true);
	}

	static public void ShowFullList()
	{
		mode = DetailMode.DM_FULLLIST;
		pGlobal.HeadbarButton.SetActive (true);
		pGlobal.ShelfDetail.gameObject.SetActive (false);
		pGlobal.FullListDetail.gameObject.SetActive (true);
		pGlobal.FavListDetail.gameObject.SetActive (false);
		pGlobal.FooterBar.gameObject.SetActive (true);
	}

	static public void ShowFavList()
	{
		mode = DetailMode.DM_FAVOURITE;
		pGlobal.HeadbarButton.SetActive (false);
		pGlobal.ShelfDetail.gameObject.SetActive (false);
		pGlobal.FullListDetail.gameObject.SetActive (false);
		pGlobal.FavListDetail.gameObject.SetActive (true);
		pGlobal.FooterBar.gameObject.SetActive (false);
	}

	static public void ClearList()
	{
		pGlobal.ShelfDetail.ClearShelfItem ();
		pGlobal.FullListDetail.ClearItemDetail ();
	}

	static public void xmlDownload (int pageIdx)
	{
		ClearList ();
		pGlobal.DownloadJSON (pageIdx);
	}
	
	public static string GetLinkIndex(int idx)
	{
		if (idx < 0)
			return "";
		if (pGlobal.Link.Length < idx)
			return "";
		return pGlobal.Link [idx].Link;
	}

	public static string GetHeaderIndex(int idx)
	{
		if (idx < 0)
			return "";
		if (pGlobal.Link.Length < idx)
			return "";
		return pGlobal.Link [idx].Header;
	}

	public static void DownloadNewLink()
	{
		pGlobal.file.ResetFile ();
		//setup Favourite Bar
		ContentPageFile.ClearLoadTexture ();
		pGlobal.FavListDetail.CreateFavItem (pGlobal.file.contentList.ToArray ());
		pGlobal.FooterBar.ClearPaging ();
		pGlobal.CurrentPageSize = 0;
		if (pGlobal.HeaderText != null)
			pGlobal.HeaderText.text = GetHeaderIndex ((int)PageDetailGlobal.type - 1);
		pGlobal.filter.CurrentMarker = 0;
		pGlobal.filter.Trigger (false);
		pGlobal.xmlData.filter = "";
		pGlobal.xmlData.cat = 0;
		pGlobal.xmlData.SetLink (UserCommonData.GetURL()+ GetLinkIndex ((int)PageDetailGlobal.type-1));
		xmlDownload (0);
	}

	public static void DownloadNewSort(string sort)
	{
		pGlobal.FooterBar.ClearPaging ();
		pGlobal.CurrentPageSize = 0;
		pGlobal.xmlData.filter = "";
		if (pGlobal.HeaderText != null)
			pGlobal.HeaderText.text = GetHeaderIndex ((int)PageDetailGlobal.type - 1);
		pGlobal.xmlData.sort = sort;
		xmlDownload (0);
	}

	public static void DownloadNewFilter(string filter)
	{
		pGlobal.FooterBar.ClearPaging ();
		pGlobal.CurrentPageSize = 0;
		if (pGlobal.HeaderText != null)
			pGlobal.HeaderText.text = GetHeaderIndex ((int)PageDetailGlobal.type - 1);
		pGlobal.xmlData.filter = filter;
		xmlDownload (0);
	}

	public static void DownloadNewCat(int cat)
	{
		pGlobal.FooterBar.ClearPaging ();
		pGlobal.CurrentPageSize = 0;
		pGlobal.xmlData.cat = cat;
		xmlDownload (0);
	}

	void Awake()
	{
		pGlobal = this;
	}

	void Start () {
		mDownloadTexList = new List<DownloadedTexture> ();
		ContentPageFile.downloadTextureList.Clear();
		ContentPageFile.downloadingList.Clear();
		mode = DetailMode.DM_SHELF;
		xmlData.SetLink (UserCommonData.GetURL()+ GetLinkIndex ((int)PageDetailGlobal.type-1));
		xmlData.postDownloaded += FinishDownload;
		subMenu.CreateSubMenu (GetGroupStruct ());
		filter.CreateSubMenu ();
		DownloadNewLink ();
	}
	void DownloadJSON(int pageIdx)
	{
		ClearDownload ();
		xmlData.StopDownload ();
		//set page url
		xmlData.StartDownload (pageIdx*MaxContentPerPage);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void ClearDownload() 
	{
		for (int i = 0; i < mDownloadTexList.Count; i++) {
			Destroy(mDownloadTexList[i].gameObject);
		}
		mDownloadTexList.Clear ();
	}

	IEnumerator Waiting()
	{
		while (ContentPageFile.downloadingList.Count > 0) {
			yield return null;
		}
		CreateFavourite ();
		state = prevState;
		pGlobal.waitingDialogue.SetActive(false);
	}

	void FinishDownload() {
		if (xmlData.contentList.Length > 0) {
			for (int i = 0; i < xmlData.contentList.Length; i++) {
					GameObject dtx = new GameObject ("DownloadedTex" + i);
					DownloadedTexture dtxComp = dtx.AddComponent<DownloadedTexture> ();
					dtx.transform.parent = this.transform;
					dtxComp.StartDownload (xmlData.contentList [i].imgthumbnail);
					mDownloadTexList.Add (dtxComp);
			}
			int nextPageSize = Mathf.CeilToInt ((float)xmlData.maxPage / PageDetailGlobal.pGlobal.MaxContentPerPage);
			if (nextPageSize != CurrentPageSize) {
				CurrentPageSize = nextPageSize;
				FooterBar.ClearPaging ();
				FooterBar.CreatePaging (CurrentPageSize);
			}
			ShelfDetail.CreateShelfItem (xmlData.contentList, mDownloadTexList,FooterBar.IsFirstPage(),FooterBar.IsLastPage());
			FullListDetail.CreateItemDetail (xmlData.contentList, mDownloadTexList);
		} else {
			Indecator.SetActive (false);
			PopupObject.ShowAlertPopup("ไม่พบข้อมูลที่ค้นหา","กรุณาค้นหาใหม่อีกครั้ง","ตกลง");
		}
	}
}
