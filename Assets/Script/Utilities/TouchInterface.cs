using UnityEngine;
using System.Collections;

public class TouchInterface {
	public static Vector2 lastTouch;
	public static Vector2 GetTouchPosition(){
#if UNITY_EDITOR || UNITY_WEBGL
		Vector3 TouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector2(TouchPos.x,TouchPos.y);
#else
		Vector3 TouchPos;
		if (Input.touchCount > 0)
		{
			TouchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			lastTouch = TouchPos;
		}
		return lastTouch;
#endif
	}
	public static bool GetTouchDown(){
#if UNITY_EDITOR || UNITY_WEBGL
		return Input.GetMouseButtonDown (0);
#else
		if (Input.touchCount > 0)
		{
			return (Input.GetTouch(0).phase == TouchPhase.Began);
		}
		else
			return false;
#endif
	}
	public static bool GetTouchUp(){
#if UNITY_EDITOR || UNITY_WEBGL
		return Input.GetMouseButtonUp (0);
#else
		if (Input.touchCount > 0)
		{
			return (Input.GetTouch(0).phase == TouchPhase.Ended);
		}
		else
			return false;
#endif
	}
}
