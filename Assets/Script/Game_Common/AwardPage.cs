using UnityEngine;
using System.Collections;

public class AwardPage : PopScreen {
	public ButtonObject CloseButton;
	public AwardList list;
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
		list.ClearAllAward();
		list.currentIndex = -1;

		GameLandGlobal.HideAward();
	}

	public void LoadAward()
	{
		list.LoadAward();
	}

}
