using UnityEngine;
using System.Collections;

public class ButtonSetting : MonoBehaviour {
	public ButtonObject btHelp;
	public ButtonObject btHelpBack;
	public ButtonObject btLogin;
	public ButtonObject btSetting;
	public ButtonObject btSettingBack;

	// Use this for initialization
	void Start () {
		btHelp.OnReleased += HelpButtonPress;
		btHelpBack.OnReleased += HelpClose;
		btLogin.OnReleased += ProfileButtonPress;
//		btLoginBack.OnReleased += ProfileClose;
		btSetting.OnReleased += SettingButtonPress;
		btSettingBack.OnReleased += SettingClose;
	}

	void HelpButtonPress()
	{
		MainMenuGlobal.SetHelpObject(true);
	}

	void ProfileButtonPress()
	{
		MainMenuGlobal.SetLoginObject(true);
	}

	void SettingButtonPress()
	{
		MainMenuGlobal.SetSettingObject(true);
	}
	void HelpClose()
	{
		MainMenuGlobal.SetHelpObject(false);
	}
	
	void ProfileClose()
	{
		MainMenuGlobal.SetLoginObject(false);
	}

	void SettingClose()
	{
		MainMenuGlobal.SetSettingObject(false);
	}
	// Update is called once per frame
	void Update () {
	}
}
