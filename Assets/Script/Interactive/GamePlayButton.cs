using UnityEngine;
using System.Collections;

public class GamePlayButton : StateButtonObject<GameState> {
	void Awake() {
		OnClicked += PlaySound;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (GlobalController.getState() != WorkingState)
			return;
		base.Update();
	}
	
	void PlaySound()
	{
		MainSoundSrc.PlaySound("SButtonHome");
	}
}
