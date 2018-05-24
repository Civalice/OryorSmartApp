using UnityEngine;
using System.Collections;

public class ShortCutHeader : ShortCutItem {
	public ShortCutItem[] SubList;

	public Sprite UnPin_Green;
	public Sprite Pin_Green;
	public SpriteRenderer PinSR;

	public override void PinnItem()
	{
		IsPinn = true;
		PinSR.sprite = Pin_Green;
		if (SubList.Length > 0)
		{
			for (int i = 0;i < SubList.Length;i++)
			{
				SubList[i].PinnItem();
			}
			return;
		}
		if (SelfPinn == null) return;
		PinnHeader.AddItem(SelfPinn);
	}
	public override void PushBackItem()
	{
		IsPinn = false;
		PinSR.sprite = UnPin_Green;
	}
	public override void Shift(int ct)
	{
		ShiftCount += ct;
		StopCoroutine("RunShift");
		StartCoroutine("RunShift");
	}

	public void UnPinnItem()
	{
		IsPinn = false;
		PinSR.sprite = UnPin_Green;
		if (SubList.Length > 0)
		{
			IsPinn = false;
			PinSR.sprite = UnPin_Green;
			for (int i = 0;i < SubList.Length;i++)
			{
				SubList[i].UnPinnItem();
			}
			return;
		}
		if (SelfPinn == null) return;
		SelfPinn.UnPinn();
	}
	
	// Update is called once per frame
	void Update () {
		if (MainMenuGlobal.getCurrentState()!=MainMenuState.MS_SHORTCUT&&
		    MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_PIN_GREEN&&
		    MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_UNPIN_GREEN) return;
		if (SubList.Length > 0)
		{
			bool NoSubPinn = true;
			for (int i = 0;i < SubList.Length;i++)
			{
				if (!SubList[i].IsPinn)
				{
					NoSubPinn = false;
					break;
				}
			}
			if (NoSubPinn)
			{
				IsPinn = true;
				PinSR.sprite = Pin_Green;
			}
			else
			{
				IsPinn = false;
				PinSR.sprite = UnPin_Green;
			}
		}
		if (PinCollider == null)
			return;
		if (IsRunning) return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown)
		{
			if (PinCollider.OverlapPoint(touchPos))
			{
				//pinned all sub item or pin itself
				if (!IsPinn)
				{
					if (MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_UNPIN_GREEN)
					{
						PinnItem();
					}
					if (MainMenuGlobal.getCurrentState()==MainMenuState.MS_TUTORIAL_PIN_GREEN)
					{
						MainMenuGlobal.Tutorial.NextTutorial();
					}
				}
				else if (IsPinn)
				{
					if (MainMenuGlobal.getCurrentState()!=MainMenuState.MS_TUTORIAL_PIN_GREEN)
					{
						UnPinnItem();
					}
					if (MainMenuGlobal.getCurrentState()==MainMenuState.MS_TUTORIAL_UNPIN_GREEN)
					{
						MainMenuGlobal.Tutorial.NextTutorial();
					}
				}
			}
		}
	}
}
