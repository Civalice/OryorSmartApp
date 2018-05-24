using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class RankingData
{
	public string userId;
	public string user_name;
	public string path;
	public int scoreAll;
	public int[] score = new int[6];
}
public class RankingList : MonoBehaviour {
	public RankingData[] rankingList;

	public ButtonObject AllBtn;
	public ButtonObject Game1Btn;
	public ButtonObject Game2Btn;
	public ButtonObject Game3Btn;
	public ButtonObject Game4Btn;
	public ButtonObject Game5Btn;
	public ButtonObject Game6Btn;

	public RankLoader gLoader;
	//list
	public GameObject RankPrefabs;
	public GameLandBG BG;

	public float RankHeight = 1.2f;

	public MyRankingItem myRanking;
	public ListSlider list;

	public int currentIndex = -1;//all

	public void LoadRank()
	{
		//open loading
		gLoader.LoadRank();
	}

	public void PostDownload()
	{
		AllGameRanking();
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
		myRanking.gameObject.SetActive(false);
	}
	
	public void CreateRankList(RankingData[] rawData,int GameId)
	{
		Debug.Log("Create RankList");
		ClearAllList();
		rankingList = rawData;

		//sorting rankingList
		switch(GameId)
		{
		case 0:
			//all
			for (int i = 0;i < rankingList.Length;i++)
			{
				for (int j = i+1;j < rankingList.Length;j++)
				{
					if (rankingList[j].scoreAll > rankingList[i].scoreAll)
					{
						RankingData tmp = rankingList[i];
						rankingList[i] = rankingList[j];
						rankingList[j] = tmp; 
					}
				}
			}
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
			for (int i = 0;i < rankingList.Length;i++)
			{
				for (int j = i+1;j < rankingList.Length;j++)
				{
					if (rankingList[j].score[GameId-1] > rankingList[i].score[GameId-1])
					{
						RankingData tmp = rankingList[i];
						rankingList[i] = rankingList[j];
						rankingList[j] = tmp; 
					}
				}
			}

			break;
		}

		//create gift
		myRanking.gameObject.SetActive(true);
		myRanking.Name.maxVisibleLines = 1;
		int q = 1;
		int rank = 1;
		foreach(RankingData item in rankingList)
		{
			if (item.userId == UserCommonData.pGlobal.user.user_id)
			{
				myRanking.SetRankItem(item,GameId,rank);
			}
			else
			{
				GameObject obj = (GameObject)GameObject.Instantiate(RankPrefabs);
				obj.transform.parent = this.transform;
				RankingItem rk = obj.GetComponent<RankingItem>();
				rk.SetRankItem(q,RankHeight,item,GameId,rank);
				rk.SendGiftButton.box = list.touchArea;
				//gf.sendDone += postSendGift;
				q++;
			}
			rank++;
		}
		BG.setHeight(RankHeight*q);
		if (q > 0) 		
			BG.gameObject.SetActive(true);
		list.ListHeight = RankHeight*q;
	}

	void Awake()
	{
		gLoader.postDownloaded += PostDownload;
		AllBtn.OnReleased += AllGameRanking;
		Game1Btn.OnReleased += Game1Ranking;
		Game2Btn.OnReleased += Game2Ranking;
		Game3Btn.OnReleased += Game3Ranking;
		Game4Btn.OnReleased += Game4Ranking;
		Game5Btn.OnReleased += Game5Ranking;
		Game6Btn.OnReleased += Game6Ranking;
	}

	void AllGameRanking()
	{
		if (currentIndex == 0) return;
		currentIndex = 0;
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
		CreateRankList(gLoader.rankingList,0);
	}

	void Game1Ranking()
	{
		if (currentIndex == 1) return;
		currentIndex = 1;
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

		CreateRankList(gLoader.rankingList,1);
	}

	void Game2Ranking()
	{
		if (currentIndex == 2) return;
		currentIndex = 2;
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

		CreateRankList(gLoader.rankingList,2);
	}
	
	void Game3Ranking()
	{
		if (currentIndex == 3) return;
		currentIndex = 3;
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

		CreateRankList(gLoader.rankingList,3);
	}
	
	void Game4Ranking()
	{
		if (currentIndex == 4) return;
		currentIndex = 4;
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

		CreateRankList(gLoader.rankingList,4);
	}
	
	void Game5Ranking()
	{
		if (currentIndex == 5) return;
		currentIndex = 5;
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
		
		CreateRankList(gLoader.rankingList,5);
	}
	
	void Game6Ranking()
	{
		if (currentIndex == 6) return;
		currentIndex = 6;
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
		
		CreateRankList(gLoader.rankingList,6);
	}
	
	// Use this for initialization
	void Start () {
		ClearAllList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
