using UnityEngine;
using System.Collections;

public class TouchDebug : MonoBehaviour {
	public GUIText text;
	// Use this for initialization
	void Start () {
		text = GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 testing = TouchInterface.GetTouchPosition ();
		text.text = testing.ToString() + " " + TouchInterface.GetTouchDown().ToString() + " " + TouchInterface.GetTouchUp().ToString();
	}
}
