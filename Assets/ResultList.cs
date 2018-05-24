using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class ResultList : MonoBehaviour {
	public TextMeshPro txt1;
	public TextMeshPro txt2;

	private CheckDownloader data;
	private int index = 0;
	private Action<CheckDownloader, string, int> onClick;
	private string productType;
	public static ResultLayer rLayer;
	
	public Collider2D box;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();

		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);

		if (touchedUp) {
			//if ((this.GetComponent<Collider2D>()==hit.collider) && rLayer.returnenableClick()) {
			if (box.OverlapPoint(touchPos) && ResultLayer.pGlobal.returnenableClick()) {
				Debug.Log (ResultLayer.pGlobal.returnenableClick());
				//mSound.playCheckSound("click");
				//if( data.checktype == "2.2")
				//{
					if(productType=="1"){ // Cosmetic
//						data.contentList[0] = data.contentList[index];
					}
					else if(productType=="2.1"){ // Danger Register
//						data.contentListDangerRegister[0] = data.contentListDangerRegister[index];
					}
					else if(productType=="2.2"){ // Danger License
//						data.contentListDangerLicense[0] = data.contentListDangerLicense[index];
					}
					else if(productType=="3"){ // Drug
						
					}
					else if(productType=="4"){ // Food
//						data.contentListFood[0] = data.contentListFood[index];
					}
					else if(productType=="5"){ // Tool
						
					}
					onClick( data, data.checktype, index );
				//}
			}
		}
	}

	public void setup(CheckDownloader data, int index, Action<CheckDownloader, string, int> onClick, string ProductType){
		this.data = data;
		this.index = index;
		this.onClick = onClick;
		this.productType = ProductType;


		txt2.maxVisibleLines = 2;
		txt1.maxVisibleLines = 2;

		//if( data.checktype == "2.2")
		//{
			txt1.text = data.seprdno;
//			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcetrdnm);
		//}

		if(ProductType=="1"){ // Cosmetic
			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentList[index].branth);
		}
		else if(ProductType=="2.1"){ // Danger Register
			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].ttxcnm);
		}
		else if(ProductType=="2.2"){ // Danger License
			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcetrdnm);
		}
		else if(ProductType=="3"){ // Drug
			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].thadrgnm);
		}
		else if(ProductType=="4"){ // Food
			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnprdnmt);
		}
		else if(ProductType=="5"){ // Tool
			txt2.text = StringUtil.ParseUnicodeEscapes(data.contentListTool[index].thamdnm);

		}
		//Debug.Log (index+" "+txt2.text);
	}
}
