using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FilterButtonList : MonoBehaviour {
	
	public Collider2D ButtonCollider;
	public ContentMainSound mSound;
	
	public GameObject Marker;
	public GameObject SubMenuNormal;
	public GameObject SubMenuPress;

	public SubMenuButton TopSubMenu;
	public SubMenuButton MidSubMenu;
	public SubMenuButton BottomSubMenu;

	public int CurrentMarker = 0;

	public int TotalMenu = 8;
	private GameObject MenuButton;
	private List<GameObject> MenuList = new List<GameObject>();
	private bool IsCreate = false;
	public bool IsActive = false;
	
	public void CreateSubMenu()
	{
		IsCreate = true;
		//Instantiate SubMenuTop
		for (int i = 0; i < TotalMenu; i++) {
			if (i == 0)
			{
				GameObject button = (GameObject)GameObject.Instantiate(TopSubMenu.gameObject);
				string text = PageDetailGlobal.pGlobal.FilterList[i];
				button.GetComponent<SubMenuButton>().SetText(text);
				button.GetComponent<SubMenuButton>().ButtonIdx = i;
				button.GetComponent<SubMenuButton>().OnPressed += buttonCallback;

				button.transform.parent = this.transform;
				button.transform.localPosition = new Vector3(TopSubMenu.transform.localPosition.x,-0.78f,0f);
				MenuList.Add(button);
			}
			else if (i == TotalMenu-1)
			{
				GameObject button = (GameObject)GameObject.Instantiate(BottomSubMenu.gameObject);
				button.GetComponent<SubMenuButton>().SetText(PageDetailGlobal.pGlobal.FilterList[i]);
				button.GetComponent<SubMenuButton>().ButtonIdx = i;
				button.GetComponent<SubMenuButton>().OnPressed += buttonCallback;


				button.transform.parent = this.transform;
				button.transform.localPosition = new Vector3(BottomSubMenu.transform.localPosition.x,-0.78f+(TotalMenu-1)*-0.58f,0f);
				MenuList.Add(button);
			}
			else
			{
				GameObject button = (GameObject)GameObject.Instantiate(MidSubMenu.gameObject);
				button.GetComponent<SubMenuButton>().SetText(PageDetailGlobal.pGlobal.FilterList[i]);
				button.GetComponent<SubMenuButton>().ButtonIdx = i;
				button.GetComponent<SubMenuButton>().OnPressed += buttonCallback;


				button.transform.parent = this.transform;
				button.transform.localPosition = new Vector3(MidSubMenu.transform.localPosition.x,-0.78f+i*-0.58f,0f);
				MenuList.Add(button);
			}
		}
		Marker.transform.parent = MenuList [CurrentMarker].transform;
		Marker.transform.localPosition = new Vector3 (-0.2f, -0.3f, 0);
	}

	public void Trigger(bool active)
	{
		Marker.transform.parent = MenuList [CurrentMarker].transform;
		Marker.transform.localPosition = new Vector3 (-0.2f, -0.3f, 0);
		if (!IsCreate)
			return;
		IsActive = active;

		if (IsActive) {
			PageDetailGlobal.HideSubMenu ();
			PageDetailGlobal.state = DetailState.DS_FILTER;
				}
		else
			PageDetailGlobal.state = DetailState.DS_LIST;

		for (int i = 0;i < TotalMenu;i++)
		{
			MenuList[i].SetActive(IsActive);
		}
		SubMenuPress.SetActive(IsActive);
		SubMenuNormal.SetActive(!IsActive);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (ButtonCollider.OverlapPoint(touchPos))
			{
				//show button
				mSound.playContentSound("submenu");
				Trigger(!IsActive);
			}
		}
	}
	//Set Button Function
	void buttonCallback(int idx)
	{
		CurrentMarker = idx;
		Trigger (false);
		PageDetailGlobal.DownloadNewCat (idx);
	}

}
