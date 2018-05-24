using UnityEngine;
using System.Collections;

public class RankingPage : PopScreen {
	public ButtonObject CloseButton;
	public RankingList rankList;
	// Use this for initialization
	void Start () {
		base.Start();
		CloseButton.OnReleased += ClosePage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ClosePage()
	{
		rankList.ClearAllList();
		rankList.currentIndex = -1;
		GameLandGlobal.HideRanking();
	}

	public void LoadRank()
	{
		rankList.LoadRank();
	}
}
