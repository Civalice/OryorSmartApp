using UnityEngine;
using System.Collections;

public class SelectedBT : MonoBehaviour {
	public Collider2D BTAll;
	public Collider2D BTCosmetic;
	public Collider2D BTDrug;
	public Collider2D BTDanger;
	public Collider2D BTTool;
	public Collider2D BTFood;
	public GameObject[] IconAll;
	public GameObject[] IconCosmetic;
	public GameObject[] IconDrug;
	public GameObject[] IconDanger;
	public GameObject[] IconTool;
	public GameObject[] IconFood;
	public GameObject[] BoxDetail;
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
			if (BTAll.OverlapPoint(touchPos))
			{
				mSound.playCheckSound("click");
				enableIcon(0);
				CheckPageGlobal.pGlobal.ProductType = 0;
			}
			if (BTCosmetic.OverlapPoint(touchPos))
			{
				mSound.playCheckSound("click");
				enableIcon(1);
				CheckPageGlobal.pGlobal.ProductType = 1;
			}
			if (BTDanger.OverlapPoint(touchPos))
			{
				mSound.playCheckSound("click");
				enableIcon(2);
				CheckPageGlobal.pGlobal.ProductType = 2;
			}
			if (BTDrug.OverlapPoint(touchPos))
			{
				mSound.playCheckSound("click");
				enableIcon(3);
				CheckPageGlobal.pGlobal.ProductType = 3;
			}
			if (BTFood.OverlapPoint(touchPos))
			{
				mSound.playCheckSound("click");
				enableIcon(4);
				CheckPageGlobal.pGlobal.ProductType = 4;
			}
			if (BTTool.OverlapPoint(touchPos))
			{
				mSound.playCheckSound("click");
				enableIcon(5);
				CheckPageGlobal.pGlobal.ProductType = 5;
			}
		}
	}

	void enableIcon(int type){
		IconAll[0].SetActive(true);
		IconAll[1].SetActive(false);
		IconCosmetic[0].SetActive(true);
		IconCosmetic[1].SetActive(false);
		IconDanger[0].SetActive(true);
		IconDanger[1].SetActive(false);
		IconDrug[0].SetActive(true);
		IconDrug[1].SetActive(false);
		IconFood[0].SetActive(true);
		IconFood[1].SetActive(false);
		IconTool[0].SetActive(true);
		IconTool[1].SetActive(false);
		for(int i=0;i<BoxDetail.Length;i++){
			BoxDetail[i].SetActive(false);
		}

		switch(type){
		case 0:
			IconAll[0].SetActive(false);
			IconAll[1].SetActive(true);
			BoxDetail[0].SetActive(true);
			break;
		case 1:
			IconCosmetic[0].SetActive(false);
			IconCosmetic[1].SetActive(true);
			BoxDetail[1].SetActive(true);
			break;
		case 2:
			IconDanger[0].SetActive(false);
			IconDanger[1].SetActive(true);
			BoxDetail[2].SetActive(true);
			break;
		case 3:
			IconDrug[0].SetActive(false);
			IconDrug[1].SetActive(true);
			BoxDetail[3].SetActive(true);
			break;
		case 4:
			IconFood[0].SetActive(false);
			IconFood[1].SetActive(true);
			BoxDetail[4].SetActive(true);
			break;
		case 5:
			IconTool[0].SetActive(false);
			IconTool[1].SetActive(true);
			BoxDetail[5].SetActive(true);
			break;
		}
	}
}
