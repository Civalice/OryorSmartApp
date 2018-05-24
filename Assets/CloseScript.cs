using UnityEngine;
using System.Collections;

public class CloseScript : MonoBehaviour {
	public Collider2D box;
	public GameObject r;
	public GameObject main;
	public GameObject ResultNotFound;
	public GameObject TextAll;
	public GameObject TextCosmetic;
	public GameObject TextDanger;
	public GameObject TextDrug;
	public GameObject TextFood;
	public GameObject TextTool;
	public GameObject Loading;
	public GameObject returnButton;
	public GameObject header;
	public ResultLayer rLayer;
	public CheckMainSound mSound;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if (box == hit.collider) {
				mSound.playCheckSound("click");
				TextAll.SetActive(false);
				TextCosmetic.SetActive(false);
				TextDanger.SetActive(false);
				TextDrug.SetActive(false);
				TextFood.SetActive(false);
				TextTool.SetActive(false);
				ResultNotFound.SetActive(false);
				Loading.SetActive(true);
				r.SetActive(false);
				main.SetActive(true);
				header.SetActive (true);
				returnButton.SetActive(false);
				rLayer.ClearContentData();
			}
		}
	}
}
