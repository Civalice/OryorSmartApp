using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReportTypeBar : MonoBehaviour {
	public List<Sprite> texList = new List<Sprite>();
	public SpriteRenderer typeRenderer;
	public ButtonObject NextButton;
	public ButtonObject PrevButton;
//	public Collider2D NextButton;
//	public Collider2D PrevButton;
	public int currentIdx = 0;
	public ReportMainSound mSound;
	// Use this for initialization
	void Awake () {
		NextButton.OnReleased += NextButtonPress;
		PrevButton.OnReleased += PrevButtonPress;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void PrevButtonPress()
	{
		mSound.playReportSound("click");
		currentIdx = (currentIdx+texList.Count-1)%texList.Count;
		
		SetType(currentIdx);
	}

	void NextButtonPress()
	{
		mSound.playReportSound("click");
		currentIdx = (currentIdx+1)%texList.Count;

		SetType(currentIdx);

	}

	public void SetType(int idx)
	{
		if (typeRenderer == null)
						return;
		typeRenderer.sprite = texList [idx];
	}
}
