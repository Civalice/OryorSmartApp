using UnityEngine;
using System.Collections;

public class HelpControl : MonoBehaviour {
	public Collider2D BTRootMenu;
	public Collider2D BTFW;
	public Collider2D BTBACK;

	public Collider2D BTMenu1;
	public Collider2D BTMenu2;
	public Collider2D BTMenu3;
	public Collider2D BTMenu4;
	public Collider2D BTMenu5;

	public GameObject RootMenu;
	public GameObject Menu1;
	public GameObject[] SubMenu1;
	public GameObject[] MascotMenu1;
	public GameObject Menu2;
	public GameObject[] SubMenu2;
	public GameObject[] MascotMenu2;
	public GameObject Menu3;
	public GameObject[] SubMenu3;
	public GameObject[] MascotMenu3;
	public GameObject Menu4;
	public GameObject[] SubMenu4;
	public GameObject[] MascotMenu4;
	public GameObject Menu5;
	public GameObject[] SubMenu5;
	public GameObject[] MascotMenu5;

	public GameObject BTOBjRootMenu;
	public GameObject BTOBjFW;
	public GameObject BTOBjBACK;
	
	public static int PrevMenuPage=0;
	public static int MenuPage=0;
	public static int SubMenuPage=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown && MainMenuState.MS_HELP == MainMenuGlobal.getCurrentState())
		{
			if(BTMenu1.OverlapPoint(touchPos)){
				PrevMenuPage = MenuPage;
				MenuPage=1;
				SubMenuPage=0;
				ChangeMenu();
			}
			else if(BTMenu2.OverlapPoint(touchPos)){
				PrevMenuPage = MenuPage;
				MenuPage=2;
				SubMenuPage=0;
				ChangeMenu();
			}
			else if(BTMenu3.OverlapPoint(touchPos)){
				PrevMenuPage = MenuPage;
				MenuPage=3;
				SubMenuPage=0;
				ChangeMenu();
			}
			else if(BTMenu4.OverlapPoint(touchPos)){
				PrevMenuPage = MenuPage;
				MenuPage=4;
				SubMenuPage=0;
				ChangeMenu();
			}
			else if(BTMenu5.OverlapPoint(touchPos)){
				PrevMenuPage = MenuPage;
				MenuPage=5;
				SubMenuPage=0;
				ChangeMenu();
			}
			else if(BTRootMenu.OverlapPoint(touchPos)){
				PrevMenuPage = MenuPage;
				MenuPage=0;
				SubMenuPage=0;
				BTOBjRootMenu.SetActive(false);
				ChangeMenu();
			}
			else if(BTFW.OverlapPoint(touchPos)){
				SubMenuPage++;
				ChangeSubMenu(true);
			}
			else if(BTBACK.OverlapPoint(touchPos)){
				if(SubMenuPage>0){
					SubMenuPage--;
					ChangeSubMenu(false);
				}
				else{
					changeToRoot ();
				}

			}
		}
	}
	
	private void ChangeMenu(){

		if(MenuPage!=0){
			BTOBjRootMenu.SetActive(false);
		}
		if(PrevMenuPage==0){
			RootMenu.SetActive(false);
		}
		else if(PrevMenuPage==1){
			for(int i=0;i<SubMenu1.Length;i++){
				if(i==0){
					SubMenu1[i].SetActive(true);
				}
				else{
					SubMenu1[i].SetActive(false);
				}
			}
			Menu1.SetActive(false);
		}
		else if(PrevMenuPage==2){
			for(int i=0;i<SubMenu2.Length;i++){
				if(i==0){
					SubMenu2[i].SetActive(true);
				}
				else{
					SubMenu2[i].SetActive(false);
				}
			}
			Menu2.SetActive(false);
		}
		else if(PrevMenuPage==3){
			for(int i=0;i<SubMenu3.Length;i++){
				if(i==0){
					SubMenu3[i].SetActive(true);
				}
				else{
					SubMenu3[i].SetActive(false);
				}
			}
			Menu3.SetActive(false);
		}
		else if(PrevMenuPage==4){
			for(int i=0;i<SubMenu4.Length;i++){
				if(i==0){
					SubMenu4[i].SetActive(true);
				}
				else{
					SubMenu4[i].SetActive(false);
				}
			}
			Menu4.SetActive(false);
		}
		else if(PrevMenuPage==5){
			for(int i=0;i<SubMenu5.Length;i++){
				if(i==0){
					SubMenu5[i].SetActive(true);
				}
				else{
					SubMenu5[i].SetActive(false);
				}
			}
			Menu5.SetActive(false);
		}
		if(MenuPage==0){
			RootMenu.SetActive(true);
			BTOBjBACK.SetActive(false);
			BTOBjFW.SetActive(false);
		}
		else if(MenuPage==1){
			Menu1.SetActive(true);
			for(int i=0;i<SubMenu1.Length;i++){
				if(i==0){
					SubMenu1[i].SetActive(true);
				}
				else{
					SubMenu1[i].SetActive(false);
				}
			}
			BTOBjBACK.SetActive(true);
			BTOBjFW.SetActive(true);
		}
		else if(MenuPage==2){
			Menu2.SetActive(true);
			for(int i=0;i<SubMenu2.Length;i++){
				if(i==0){
					SubMenu2[i].SetActive(true);
				}
				else{
					SubMenu2[i].SetActive(false);
				}
			}
			BTOBjBACK.SetActive(true);
			BTOBjFW.SetActive(true);
		}
		else if(MenuPage==3){
			Menu3.SetActive(true);
			for(int i=0;i<SubMenu3.Length;i++){
				if(i==0){
					SubMenu3[i].SetActive(true);
				}
				else{
					SubMenu3[i].SetActive(false);
				}
			}
			BTOBjBACK.SetActive(true);
			BTOBjFW.SetActive(false);
		}
		else if(MenuPage==4){
			Menu4.SetActive(true);
			for(int i=0;i<SubMenu4.Length;i++){
				if(i==0){
					SubMenu4[i].SetActive(true);
				}
				else{
					SubMenu4[i].SetActive(false);
				}
			}
			BTOBjBACK.SetActive(true);
			BTOBjFW.SetActive(false);
		}
		else if(MenuPage==5){
			Menu5.SetActive(true);
			for(int i=0;i<SubMenu5.Length;i++){
				if(i==0){
					SubMenu5[i].SetActive(true);
				}
				else{
					SubMenu5[i].SetActive(false);
				}
			}
			BTOBjBACK.SetActive(true);
			BTOBjFW.SetActive(false);
		}
	}
	private void ChangeSubMenu(bool up){
		if(MenuPage==1){
			if(up){
				SubMenu1[SubMenuPage-1].SetActive(false);
				SubMenu1[SubMenuPage].SetActive(true);
			}
			else{
				SubMenu1[SubMenuPage+1].SetActive(false);
				SubMenu1[SubMenuPage].SetActive(true);
			}

			if(SubMenuPage==0){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
			else if((SubMenuPage+1)==SubMenu1.Length){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(false);
			}
			else{
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
		}
		else if(MenuPage==2){
			if(up){
				SubMenu2[SubMenuPage-1].SetActive(false);
				SubMenu2[SubMenuPage].SetActive(true);
			}
			else{
				SubMenu2[SubMenuPage+1].SetActive(false);
				SubMenu2[SubMenuPage].SetActive(true);
			}
			
			if(SubMenuPage==0){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
			else if((SubMenuPage+1)==SubMenu2.Length){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(false);
			}
			else{
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
		}
		else if(MenuPage==3){
			if(up){
				SubMenu3[SubMenuPage-1].SetActive(false);
				SubMenu3[SubMenuPage].SetActive(true);
			}
			else{
				SubMenu3[SubMenuPage+1].SetActive(false);
				SubMenu3[SubMenuPage].SetActive(true);
			}
			
			if(SubMenuPage==0){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
			else if((SubMenuPage+1)==SubMenu3.Length){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(false);
			}
			else{
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
		}
		else if(MenuPage==4){
			if(up){
				SubMenu4[SubMenuPage-1].SetActive(false);
				SubMenu4[SubMenuPage].SetActive(true);
			}
			else{
				SubMenu4[SubMenuPage+1].SetActive(false);
				SubMenu4[SubMenuPage].SetActive(true);
			}
			
			if(SubMenuPage==0){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
			else if((SubMenuPage+1)==SubMenu4.Length){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(false);
			}
			else{
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
		}
		else if(MenuPage==5){
			if(up){
				SubMenu5[SubMenuPage-1].SetActive(false);
				SubMenu5[SubMenuPage].SetActive(true);
			}
			else{
				SubMenu5[SubMenuPage+1].SetActive(false);
				SubMenu5[SubMenuPage].SetActive(true);
			}
			
			if(SubMenuPage==0){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
			else if((SubMenuPage+1)==SubMenu5.Length){
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(false);
			}
			else{
				BTOBjBACK.SetActive(true);
				BTOBjFW.SetActive(true);
			}
		}
	}

	private void changeToRoot(){
		PrevMenuPage = MenuPage;
		MenuPage=0;
		SubMenuPage=0;
		BTOBjRootMenu.SetActive(false);
		ChangeMenu();
	}
}
