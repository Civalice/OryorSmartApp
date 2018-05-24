using UnityEngine;
using System.Collections;

public class LBSLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if !UNITY_WEBGL && !DISABLE_WEBVIEW
			LBSPlugin.OpenLBS();
		#endif
	}
	public void CallClose()
	{
		SceneMgr.ChangeScene("MainMenu");
	}
}
