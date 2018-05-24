using UnityEngine;
using System.Collections;
[RequireComponent (typeof(ButtonObject))]
public class TestEvent : MonoBehaviour {

	private ButtonObject btObj;

	void OnEnable() {
		if (btObj != null) {
			btObj.OnReleased+=MyEvent;
				}
	}

	void OnDisable() {
		if (btObj != null) {
						btObj.OnReleased -= MyEvent;
				}
		}
	void Awake() {
		btObj = GetComponent<ButtonObject> ();
		}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MyEvent() {
		Debug.Log ("Test Event");
		}
}
