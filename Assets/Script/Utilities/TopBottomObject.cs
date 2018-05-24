using UnityEngine;
using System.Collections;

public enum ObjectPosition
{
	Pos_Top = 0,
	Pos_Bottom,
	Pos_Free
};

public class TopBottomObject : MonoBehaviour {
	public ObjectPosition position;
	public float Ratio = 1.0f;
	// Use this for initialization
	void Start () {
		float change = (ScreenRatio.CameraSize - 4.8f)*Ratio;
		switch (position) {
		case ObjectPosition.Pos_Top:
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y+change,0);
			break;
		case ObjectPosition.Pos_Bottom:
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y-change,0);
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
