using UnityEngine;
using System.Collections;

[System.Serializable]
public class AwardItemData
{
	public string title;
	public string AwardDetail;
	public int progress;
	public int max;
}

public class AwardList : MonoBehaviour {
	public AwardItemData[] awardList;

	public ButtonObject AllBtn;
	public ButtonObject Game1Btn;
	public ButtonObject Game2Btn;
	public ButtonObject Game3Btn;
	public ButtonObject Game4Btn;
	public ButtonObject Game5Btn;
	public ButtonObject Game6Btn;

	public AwardLoader gLoader;

	public GameObject awardPrefabs;
	public GameLandBG BG;

	public float AwardHeight = 1.2f;
	public ListSlider list;

	public int currentIndex = -1;//all
	private AwardLoadData dbs;

	public void LoadAward()
	{
		//open loading
		gLoader.LoadAward();
	}
	
	public void PostDownload()
	{
		dbs = gLoader.awardList;
		if (gLoader.awardList.allGameData[0] == null) {
			PopupObject.ShowAlertPopup("พบปัญหาในการเชื่อมต่อ",
			                           "ไม่สามารถตรวจสอบข้อมูลของท่านได้ กรุณาตรวจสอบอินเทอร์เน็ตของท่าน และลองใหม่อีกครั้ง",
			                           "ยกเลิก",null,
			                           "เชื่อมต่อใหม่",LoadAward);
				} else {
						AllGameAward ();
				}
		LoadingScript.HideLoading();
	}

	// Use this for initialization
	void Awake ()
	{
		gLoader.postDownloaded += PostDownload;
		AllBtn.OnReleased += AllGameAward;
		Game1Btn.OnReleased += Game1Award;
		Game2Btn.OnReleased += Game2Award;
		Game3Btn.OnReleased += Game3Award;
		Game4Btn.OnReleased += Game4Award;
		Game5Btn.OnReleased += Game5Award;
		Game6Btn.OnReleased += Game6Award;
	}
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClearAllAward()
	{
		for (int i = 0;i < transform.childCount;i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		list.ResetList();
	}

	void AllGameAward()
	{
		if (currentIndex == 0) return;
		currentIndex = 0;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}

		//create allGameAward
		//get AllAwardData
		awardList = dbs.allGameData;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}

	void Game1Award()
	{
		if (currentIndex == 1) return;
		currentIndex = 1;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}

		//create allGameAward
		//get AllAwardData
		awardList = dbs.Game1Data;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}

	void Game2Award()
	{
		if (currentIndex == 2) return;
		currentIndex = 2;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}

		//create allGameAward
		//get AllAwardData
		awardList = dbs.Game2Data;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}

	void Game3Award()
	{
		if (currentIndex == 3) return;
		currentIndex = 3;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}

		//create allGameAward
		//get AllAwardData
		awardList = dbs.Game3Data;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}

	void Game4Award()
	{
		if (currentIndex == 4) return;
		currentIndex = 4;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}

		//create allGameAward
		//get AllAwardData
		awardList = dbs.Game4Data;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}
	
	void Game5Award()
	{
		if (currentIndex == 5) return;
		currentIndex = 5;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		
		//create allGameAward
		//get AllAwardData
		awardList = dbs.Game5Data;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}
	
	void Game6Award()
	{
		if (currentIndex == 6) return;
		currentIndex = 6;
		ClearAllAward();
		foreach(SpriteRenderer sr in AllBtn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game1Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game2Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game3Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game4Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game5Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,0.3f);
		}
		foreach(SpriteRenderer sr in Game6Btn.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1.0f,1.0f,1.0f,1.0f);
		}
		
		//create allGameAward
		//get AllAwardData
		awardList = dbs.Game6Data;
		int i = 0;
		foreach(AwardItemData item in awardList)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(awardPrefabs);
			obj.transform.parent = this.transform;
			obj.GetComponent<AwardItem>().SetupAward(i,AwardHeight,item.title,item.progress,item.max,item.AwardDetail);
			i++;
		}
		BG.setHeight(AwardHeight*i);
		list.ListHeight = AwardHeight*i;
	}
}
