using UnityEngine;
using System.Collections;

public class BMIDetailBMR : MonoBehaviour {
	public Collider2D box;
	public GameObject BMIDetail;
	public GameObject Head;
	public GameObject Detail;
	public BMISoundController mSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		//				Debug.Log (Camera.main.ScreenToWorldPoint (touchPos));
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if (box == hit.collider) {
				mSound.playBMISound("click");
				BMIDetail.SetActive(true);
				Head.SetActive(false);
				Detail.SetActive(false);
			}
		}
	}
}
