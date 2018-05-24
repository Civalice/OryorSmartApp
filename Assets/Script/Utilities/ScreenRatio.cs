using UnityEngine;
using System.Collections;

public class ScreenRatio : MonoBehaviour {



//	Screen Size = 320x480; ~ 1.5 we use size 4.8
//	Screen Size = 768x1024; ~ 1.333333 size 4.27

	// ratio/1.5 == x/4.8

	//size = 0.88958
	//size = 0.88888
	public static float ScrRatio = (float)Screen.height / Screen.width;
	public static float CameraSize = ScrRatio*4.8f/1.5f;
	public static void SetupCamera()
	{
		Camera.main.orthographicSize = CameraSize;
	}

	public static float SizeDiff()
	{
		return CameraSize-4.8f;
	}

	public static void ScrDebug()
	{
		Debug.Log ("Screen Ratio = " + Screen.width + "x" + Screen.height +"  "+ScrRatio);
//		ScrRatio = (float)Screen.height / Screen.width;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
