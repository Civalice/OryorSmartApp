using UnityEngine;
using System.Collections;

public class GameLandMenu : MonoBehaviour {
	public ButtonObject RankingButton;
	public ButtonObject AwardButton;
	public ButtonObject GiftButton;
	// Use this for initialization
	void Start () {
		RankingButton.OnReleased += ShowRanking;
		AwardButton.OnReleased += ShowAward;
		GiftButton.OnReleased += ShowGift;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void ShowGift()
	{
		if(GameLandGlobal.state==GameLandState.GS_MAPMODE){
			GameLandGlobal.ShowGift();
		}
	}

	void ShowAward()
	{
		if (GameLandGlobal.state == GameLandState.GS_MAPMODE) {
			GameLandGlobal.ShowAward ();
		}
	}

	void ShowRanking()
	{
		if (GameLandGlobal.state == GameLandState.GS_MAPMODE) {
			GameLandGlobal.ShowRanking ();
		}
	}
}
