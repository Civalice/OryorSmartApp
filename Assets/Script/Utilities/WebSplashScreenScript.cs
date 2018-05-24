using UnityEngine;
using System.Collections;

public class WebSplashScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PopupObject.instance.Button1.OnReleased = null;
		RequestUserData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RequestUserData()
	{
#if UNITY_WEB_PLAYER
		//call request userdata here
		Application.ExternalCall("RequestUserDataString");
#endif
	}
	void RequestUserDataCB(string userData)
	{
		Debug.Log("userData String : "+userData);
	}
}
