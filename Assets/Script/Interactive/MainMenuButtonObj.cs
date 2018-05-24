using UnityEngine;
using System.Collections;

public class MainMenuButtonObj : ButtonObject {
	public MainMenuState WorkingState = MainMenuState.MS_NORMAL;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (MainMenuGlobal.getCurrentState() != WorkingState)
			return;
		base.Update();
	}
}
