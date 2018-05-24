using UnityEngine;
using System.Collections;

public class GameLandButtonObj : StateButtonObject<GameLandState> {
	void Awake() {
		OnClicked += PlaySound;
	}
	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	protected override void Update () {
		if (GameLandGlobal.state != WorkingState)
			return;
		base.Update();
	}

	void PlaySound()
	{
		MainSoundSrc.PlaySound("SButtonHome");
	}
}
