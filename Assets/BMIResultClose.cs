using UnityEngine;
using System.Collections;

public class BMIResultClose : MonoBehaviour {
	public GameObject ResultLayer;
	public Collider2D mCollider;

	private Vector3 bottomposition;

	// Use this for initialization
	void Start () {
		bottomposition = new Vector3(0,-5.25f,0);
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if (mCollider.OverlapPoint(touchPos))
			{
				//show button
				ResultLayer.transform.position = bottomposition;
				ResultLayer.SetActive (false);
				
			}
		}
	}
}
