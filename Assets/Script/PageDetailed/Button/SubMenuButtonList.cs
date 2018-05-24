using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubMenuButtonList : MonoBehaviour {

	public Collider2D ButtonCollider;
	public ContentMainSound mSound;

	public GameObject SubMenuNormal;
	public GameObject SubMenuPress;

	public SubMenuButton TopSubMenu;
	public SubMenuButton MidSubMenu;
	public SubMenuButton BottomSubMenu;

	public int TotalMenu = 6;
	private GameObject MenuButton;
	private List<GameObject> MenuList = new List<GameObject>();
	private GroupStructure groupStr;

	private bool IsCreate = false;
	public bool IsActive = false;

	public void CreateSubMenu(GroupStructure structure)
	{
		IsCreate = false;
		groupStr = structure;
		if (structure == null) {
			gameObject.SetActive(false);
			return;
				}
		IsCreate = true;
		TotalMenu = structure.TypeList.Length;
		//Instantiate SubMenuTop
		for (int i = 0; i < TotalMenu; i++) {
			if (i == 0)
			{
				GameObject button = (GameObject)GameObject.Instantiate(TopSubMenu.gameObject);
				button.GetComponent<SubMenuButton>().SetText(PageDetailGlobal.pGlobal.Link[(int)structure.TypeList[i]-1].Header);
				button.GetComponent<SubMenuButton>().ButtonIdx = i;
				button.GetComponent<SubMenuButton>().OnPressed += buttonCallback;
				button.transform.parent = this.transform;
				button.transform.localPosition = new Vector3(TopSubMenu.transform.localPosition.x,-0.78f,0f);
				MenuList.Add(button);
			}
			else if (i == TotalMenu-1)
			{
				GameObject button = (GameObject)GameObject.Instantiate(BottomSubMenu.gameObject);
				button.GetComponent<SubMenuButton>().SetText(PageDetailGlobal.pGlobal.Link[(int)structure.TypeList[i]-1].Header);
				button.GetComponent<SubMenuButton>().ButtonIdx = i;
				button.GetComponent<SubMenuButton>().OnPressed += buttonCallback;
			
				button.transform.parent = this.transform;
				button.transform.localPosition = new Vector3(BottomSubMenu.transform.localPosition.x,-0.78f+(TotalMenu-1)*-0.58f,0f);
				MenuList.Add(button);
			}
			else
			{
				GameObject button = (GameObject)GameObject.Instantiate(MidSubMenu.gameObject);
				button.GetComponent<SubMenuButton>().SetText(PageDetailGlobal.pGlobal.Link[(int)structure.TypeList[i]-1].Header);
				button.GetComponent<SubMenuButton>().ButtonIdx = i;
				button.GetComponent<SubMenuButton>().OnPressed += buttonCallback;

				button.transform.parent = this.transform;
				button.transform.localPosition = new Vector3(MidSubMenu.transform.localPosition.x,-0.78f+i*-0.58f,0f);
				MenuList.Add(button);
			}
		}
	}

	public void Trigger(bool active)
	{
		if (!IsCreate)
			return;
		IsActive = active;

		if (IsActive) {
			PageDetailGlobal.HideFilter();
			PageDetailGlobal.state = DetailState.DS_SUBMENU;
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

	void buttonCallback(int idx)
	{
		Trigger (false);
		if (PageDetailGlobal.type != groupStr.TypeList [idx]) {
						PageDetailGlobal.type = groupStr.TypeList [idx];
						PageDetailGlobal.DownloadNewLink ();
				}
	}
}
