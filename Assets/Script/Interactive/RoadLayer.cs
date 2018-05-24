using UnityEngine;
using System.Collections;
public class RoadLayer : MonoBehaviour {
	public GameObject RoadObject;
	float rotateVal = 0.0f;
	float changeDeg = 0.0f;
	GameObject[] road;
	[ExecuteInEditMode]
	void Awake () {
		if (road == null) {
			road = new GameObject[6];
			for (int i = 0;i < 6;i++) {
				road [i] = (GameObject)GameObject.Instantiate (RoadObject);
				road [i].name = "RoadSprite";
				road [i].transform.Rotate(0,0,9.05f*(i-3));
				road [i].transform.parent = this.transform;
			}
		}
	}
	// Use this for initialization
	void Start () {

	}

	public void SlideRoad(float deg) {
		changeDeg = deg;
	}

	// Update is called once per frame
	void Update () {
		rotateVal += changeDeg;
		for (int i = 0; i < 6; i++) {
			road [i].transform.Rotate(0,0,changeDeg);
		}
		while (rotateVal > 9.05f) {
			rotateVal -= 9.05f;
			for (int i = 0; i < 6; i++) {
				road [i].transform.Rotate(0,0,-9.05f);
			}
		}
		while (rotateVal < -9.05f) {
			rotateVal += 9.05f;
			for (int i = 0; i < 6; i++) {
				road [i].transform.Rotate(0,0,9.05f);
			}
		}
	}
}
