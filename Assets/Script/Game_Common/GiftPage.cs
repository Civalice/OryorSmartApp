using UnityEngine;
using System.Collections;

public class GiftPage : PopScreen {
	public ButtonObject CloseButton;
	public GiftList gList;
	// Use this for initialization
	void Start () {
		base.Start();
		CloseButton.OnReleased += CloseGift;
	}

	void CloseGift()
	{
		gList.ClearAllList();
		GameLandGlobal.HideGift();
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadGift()
	{
		gList.LoadGift();
	}
}
