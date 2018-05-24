using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class LBSPlugin {

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _OpenLBS();
	public static void OpenLBS() {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_OpenLBS();
		}
	}
#elif UNITY_ANDROID
	public static void OpenLBS() {
		AndroidJNIHelper.debug = true;
		//The comments describe what you would need to do if you were using raw JNI
		AndroidJavaObject jo = new AndroidJavaObject("oryor.smart.app.UnityTestActivity");

		}
#elif UNITY_WINRT
	public static void OpenLBS() {
		UnityPluginForWindowsPhone.BridgeWP.startPage ();
	}

#elif UNITY_EDITOR
	public static void OpenLBS() {
		Debug.Log("OpenLBS");
	}
#endif
}