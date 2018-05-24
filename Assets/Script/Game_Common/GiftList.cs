using UnityEngine;
using System.Collections;
[System.Serializable]
public class GiftItemData
{
	public string GiftKey;
	public string name;
	public int id;
	public string picPath;
	public string giftName;
}

public class GiftList : MonoBehaviour {
	public GiftItemData[] giftList;

	public GiftLoader gLoader;
	public GameObject NothingText;
	public GameObject giftPrefabs;
	public GameLandBG BG;
	
	public float GiftHeight = 1.2f;
	public ListSlider list;

	public void LoadGift()
	{
		gLoader.LoadGift();
		LoadingScript.ShowLoading();
	}
	
	public void PostDownload()
	{
		CreateGiftList(gLoader.giftList);
		LoadingScript.HideLoading();
	}

	public void ClearAllList()
	{
		for (int i = 0;i < transform.childCount;i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		list.ResetList();
		BG.setHeight(0);
		BG.gameObject.SetActive(false);
	}

	public void CreateGiftList(GiftItemData[] rawData)
	{
		Debug.Log("Create GiftList");
		ClearAllList();
		giftList = rawData;
		//create gift
		int i = 0;
		foreach(GiftItemData item in giftList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(giftPrefabs);
			obj.transform.parent = this.transform;
			GiftItem gf = obj.GetComponent<GiftItem>();
			gf.acceptGift.box = list.touchArea;
			gf.SetGiftItem(i,GiftHeight,item);
			gf.postAccept += postAcceptGift;
			i++;
		}
		BG.setHeight(GiftHeight*i);
		if (i > 0) 		
		{
			BG.gameObject.SetActive(true);
			NothingText.SetActive(false);
		}
		else
		{
			NothingText.SetActive(true);
		}
		list.ListHeight = GiftHeight*i;
	}

	IEnumerator postAcceptGift()
	{
		LoadGift();
		//reload GiftList
		while (gLoader.isFinish)
		{
			yield return null;
		}
	}

	void Awake () {
		gLoader.postDownloaded += PostDownload;
	}

	// Use this for initialization
	void Start () {
		ClearAllList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
