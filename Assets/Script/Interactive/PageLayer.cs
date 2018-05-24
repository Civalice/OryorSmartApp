using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PageLayer : MonoBehaviour {
	public static int maxPage;
	public static float pageLength;
	public List<BubbleObject> BubbleList;
	public List<BGObject> ObjectList;
	public float baseRotate;
	public float rotVal;

	private float MaxRotVal;
	// Use this for initialization
	void Awake () {
		MaxRotVal = (maxPage/2)*pageLength;
		foreach (BubbleObject obj in BubbleList) {
			obj.hide ();
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
	public void setRotatePage(float _deg) {
		baseRotate = _deg;
		rotVal = 0;
		if (baseRotate+rotVal > MaxRotVal) {
						rotVal -= maxPage*pageLength;
		} else if (baseRotate+rotVal < -MaxRotVal) {
						rotVal += maxPage*pageLength;
				} 
		{
			this.transform.rotation = Quaternion.Euler (0, 0, baseRotate+rotVal);
				}
	}
	public void RotatePageDiff(float _deg) {
		rotVal += _deg;
		if (baseRotate+rotVal > MaxRotVal) {
			rotVal -= maxPage*pageLength;
		} else if (baseRotate+rotVal < -MaxRotVal) {
			rotVal += maxPage*pageLength;
		} 
		{
			this.transform.rotation = Quaternion.Euler (0, 0, baseRotate+rotVal);
		}
	}

	public void DebugRotate() {
		Debug.Log ("Rotate = " + rotVal + " Base = " + baseRotate);
	}
	public void PopObject() {
		foreach (BGObject obj in ObjectList) {
			obj.popup();
				}
		}

	public void ResetBubble() {
		foreach (BubbleObject obj in BubbleList) {
			obj.hide ();
		}
	}
	public void PopBubble() {
		foreach (BubbleObject obj in BubbleList) {
			obj.popup ();
		}
	}

	public bool PageIsReady() {
		foreach (BubbleObject obj in BubbleList) {
			if (!obj.IsReady()) return false;
				}
		return true;
	}
}

