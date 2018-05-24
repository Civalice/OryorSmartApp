using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CheckPageGlobal : MonoBehaviour {
	static public Color pageColor = new Color32(46,48,146,255);
	public GameObject[] ColorObject;
	public int ProductType=0;  //0=all,1=cosmetic,3=drug,2=danger,4=food,5=tool
	public GameObject MainLayer;
	public GameObject ResultLayer;
	public GameObject ResultLoading;
	public GameObject Agreement;
	public ResultLayer rLayer;
	static public CheckPageGlobal pGlobal;
	public TextMeshPro productno;
	public string prdno="";
	
	public CheckMainSound mSound;
	
	public CheckDownloader xmlData;
	
	// Use this for initialization
	void Awake () {
		#if !UNITY_EDITOR
		if(UserCommonData.IsAGOn()){
			Agreement.SetActive(false);
		}
		#endif
	}
	void Start () {
		pGlobal = this;
		foreach (GameObject obj in ColorObject) {
			if (obj != null)
			{
				SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
				renderer.color = pageColor;
			}
		}
		xmlData.postDownloaded += FinishDownload;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Check Product : "+pGlobal.ProductType.ToString());
	}
	public static void CheckProduct(){
		pGlobal.prdno = pGlobal.productno.text;
		if(pGlobal.prdno!=""){
			pGlobal.mSound.playCheckSound("Check");
			pGlobal.MainLayer.SetActive(false);
			pGlobal.ResultLayer.SetActive(true);
			Debug.Log("Start Check : "+pGlobal.productno.text);
			//xmlData.SetLink (proDuctType);
			pGlobal.xmlData.clearxmldata();
			pGlobal.xmlData.StopDownload();
			pGlobal.xmlData.StartDownload(pGlobal.prdno);
		}
	}
	public void FinishDownload(){
		Debug.Log("Finish Download");
		Debug.Log("contentList : "+ (xmlData.contentList!=null));
		Debug.Log("contentListDangerRegister : "+ (xmlData.contentListDangerRegister!=null));
		Debug.Log("contentListDangerLicense : "+ (xmlData.contentListDangerLicense!=null));
		Debug.Log("contentListFood : "+ (xmlData.contentListFood!=null));
		Debug.Log("contentListDrug : "+ (xmlData.contentListDrug!=null));
		Debug.Log("contentListTool : "+ (xmlData.contentListTool!=null));
		
		ResultLoading.SetActive(false);
		
		rLayer.ClearContentData();
		if(xmlData.contentList!=null || xmlData.contentListDangerRegister!=null || xmlData.contentListDangerLicense!=null || xmlData.contentListFood!=null || xmlData.contentListDrug!=null || xmlData.contentListTool!=null){
			//rLayer.setContentData(xmlData,xmlData.checktype);
			rLayer.checkContentData(xmlData,xmlData.checktype);
		}
		else{
			rLayer.setNotFound(ProductType);
		}
		
	}
}
