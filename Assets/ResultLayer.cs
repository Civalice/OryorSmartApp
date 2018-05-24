using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ResultLayer : MonoBehaviour {
	public GameObject rSearch;
	public GameObject rSearchLayer;
	public GameObject rList;
	public GameObject objListLayer;
	public CreateResultList rListLayer;
	public GameObject notfound;
	public GameObject TextAll;
	public GameObject TextCosmetic;
	public GameObject TextDanger;
	public GameObject TextDrug;
	public GameObject TextFood;
	public GameObject TextTool;
	public GameObject returnButton;
	public GameObject header;
	static public ResultLayer pGlobal;
	
	public TextMeshPro inputBox;
	
	private List<string> searchName = new List<string> ();
	private List<string> searchData = new List<string> ();
	static private List<GameObject> searchList = new List<GameObject> ();
	public List<GameObject> resultList = new List<GameObject>();
	
	private Vector2 lastTouchPos;
	private Vector3 currentMapPos;
	public float ItemMoveMent = 0.0f;
	public float ItemSearchMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;
	private MorphObject SnappingMorph = new MorphObject ();
	
	public bool IsDrag = false;
	private bool resultState = false;
	public bool enableClick = true;
	private float scrollAreaHeight;
	private float scrollSearchAreaHeight;
	
	// Use this for initialization
	void Start () {
		pGlobal = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(!resultState){return;}
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		
		if (TouchDown)
		{
			lastTouchPos = touchPos;
			currentMapPos = objListLayer.transform.localPosition;
			IsDrag = true;
			
			enableClick = true;
		}
		if (TouchUp)
		{
			if(IsDrag){
				IsDrag = false;
			}
		}
		
		if(IsDrag && (Mathf.Abs(lastTouchPos.y-touchPos.y)>=0.09f)){
			enableClick = false;
			//			Debug.Log ("enable False : "+IsDrag+" "+enableClick+" "+lastTouchPos.y+" "+touchPos.y+" "+(Mathf.Abs(lastTouchPos.y-touchPos.y)<=0.09f));
		}
		
		if (IsDrag)
		{
			//calculate last touch
			ItemMoveMent += (touchPos.y - lastTouchPos.y) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.y - lastTouchPos.y)* (Sensitive/10.0f) / (Time.deltaTime);
			currentMoveSpeed = MoveSpeed;
			ItemSearchMoveMent = ItemMoveMent;
			if (ItemMoveMent < 3.65f)
				ItemMoveMent = 3.65f;
			if (ItemMoveMent > scrollAreaHeight)
				ItemMoveMent = scrollAreaHeight;
			if (ItemSearchMoveMent < 3.65f)
				ItemSearchMoveMent = 3.65f;
			if (ItemSearchMoveMent > scrollSearchAreaHeight)
				ItemSearchMoveMent = scrollSearchAreaHeight;
			lastTouchPos = touchPos;
			Debug.Log (((Screen.height/20))+" "+ItemMoveMent+" "+scrollAreaHeight+" "+ItemSearchMoveMent+" "+scrollSearchAreaHeight);
			if(currentMoveSpeed!=0){
				//IsDrag = true;
			}
			//move all item
			if(scrollAreaHeight>((Screen.height/20))){
				objListLayer.transform.localPosition = new Vector3(-1,ItemMoveMent,0);
			}
			if(scrollSearchAreaHeight>((Screen.height/20))){
				rSearchLayer.transform.localPosition = new Vector3(-1,ItemSearchMoveMent,0);
			}
		}
	}
	
	public void checkContentData(CheckDownloader data, string ProductType){
		checkSearchData(data, ProductType);
		header.SetActive (false);
	}
	
	public void selectListCallback(CheckDownloader data, string ProductType,int index){
		ClearList();
		returnButton.SetActive (true);
		setContentData( data, ProductType, index );
	}
	
	public void setContentData(CheckDownloader data, string ProductType, int index){
		createSearchData(data, ProductType, index);
		
		rSearchLayer.transform.localPosition = new Vector3(-1f,3.65f,0);
		for(int i=0;i<searchName.Count;i++){
			GameObject itemDetail = (GameObject)GameObject.Instantiate(rSearch);
			itemDetail.SetActive(true);
			itemDetail.transform.parent = rSearchLayer.transform;
			itemDetail.transform.localPosition = new Vector3 (1f, 0f - i * 0.81f, 0);
			
			ResultSearch idt = itemDetail.GetComponent<ResultSearch>();
			idt.setSearchContent(searchName[i],searchData[i]);
			searchList.Add (itemDetail);
			
			scrollSearchAreaHeight = i * 0.81f;
			//Debug.Log(searchList.Count);
		}
	}
	public void setNotFound(int ProductType){
		notfound.SetActive(true);
		TextAll.SetActive(false);
		TextCosmetic.SetActive(false);
		TextDanger.SetActive(false);
		TextDrug.SetActive(false);
		TextFood.SetActive(false);
		TextTool.SetActive(false);
		if(ProductType==0){
			TextAll.SetActive(true);
		}
		else if(ProductType==1){
			TextCosmetic.SetActive(true);
		}
		else if(ProductType==2){
			TextDanger.SetActive(true);
		}
		else if(ProductType==3){
			TextDrug.SetActive(true);
		}
		else if(ProductType==4){
			TextFood.SetActive(true);
		}
		else if(ProductType==5){
			TextTool.SetActive(true);
		}
		else{
			TextAll.SetActive(true);
		}
	}
	void checkSearchData(CheckDownloader data, string ProductType){
		if(ProductType=="1"){ // Cosmetic
			if(data.contentList.Length>1){//create List
				rListLayer.createResultList(data,ProductType);
			}
			else{setContentData(data,ProductType,0);}
		}
		else if(ProductType=="2.1"){ // Danger Register
			if(data.contentListDangerRegister.Length>1){//create List
				rListLayer.createResultList(data,ProductType);
			}
			else{setContentData(data,ProductType,0);}
		}
		else if(ProductType=="2.2"){ // Danger License
			Debug.Log ("Data Search Length : "+data.contentListDangerLicense.Length);
			if(data.contentListDangerLicense.Length>1){//create List
				//				for(int i=0;i<data.contentListDangerLicense.Length;i++){
				//					GameObject itemDetail = (GameObject)GameObject.Instantiate(rList);
				//					itemDetail.SetActive(true);
				//					itemDetail.transform.parent = this.transform;
				//					itemDetail.transform.localPosition = new Vector3 (0, -6.88f - i * 0.68f, 0);
				//					
				//					ResultList idt = itemDetail.GetComponent<ResultList>();
				//					idt.setup( data, i, selectListCallback );
				//					resultList.Add(itemDetail);
				//				}
				rListLayer.createResultList(data,ProductType);
			}
			else{setContentData(data,ProductType,0);}
		}
		else if(ProductType=="3"){ // Drug
			if(data.contentListDrug.Length>1){//create List
				rListLayer.createResultList(data,ProductType);
			}
			else{setContentData(data,ProductType,0);}
		}
		else if(ProductType=="4"){ // Food
			if(data.contentListFood.Length>1){//create List
				rListLayer.createResultList(data,ProductType);
			}
			else{setContentData(data,ProductType,0);}
		}
		else if(ProductType=="5"){ // Tool
			if(data.contentListTool.Length>1){//create List
				rListLayer.createResultList(data,ProductType);
			}
			else{setContentData(data,ProductType,0);}
		}
		resultState = true;
	}
	void createSearchData(CheckDownloader data, string ProductType, int index){
		if(ProductType=="1"){ // Cosmetic
			searchName.Add("ประเภทการแจ้ง");
			searchName.Add("เลขที่แจ้ง");
			searchName.Add("cattype");
			searchName.Add("ชื่อการค้า (TH)");
			searchName.Add("ชื่อการค้า (EN)");
			searchName.Add("ชื่อเครื่องสำอาง (TH)");
			searchName.Add("ชื่อเครื่องสำอาง (EN)");
			searchName.Add("ชื่อผู้ประกอบการ");
			searchName.Add("ชื่อประเทศ");
			searchName.Add("ประเภท เครื่องสำอาง");
			searchName.Add("ที่ตั้ง");
			Debug.Log(data.contentList[index].cmttype);
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].cmttype));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].prdno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].cattype));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].branth));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].branen));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].prdth));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].prden));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].addr));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].frg));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].ctm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentList[index].cnt));
		}
		else if(ProductType=="2.1"){ // Danger Register
			searchName.Add("เลขที่ใบสำคัญ");
			searchName.Add("ชื่อการค้า (TH)");
			searchName.Add("ชื่อการค้า (EN)");
			searchName.Add("ประเภท ทะเบียน");
			searchName.Add("ประเภท ผลิตภัณฑ์");	
			searchName.Add("รูปแบบ ผลิตภัณฑ์");
			searchName.Add("ผู้รับอนุญาต");
			searchName.Add("ที่อยู่ ผู้รับอนุญาต");
			searchName.Add("ชื่อผู้ผลิต");
			searchName.Add("ที่อยู่ผู้ผลิต");
			searchName.Add("วันที่ได้ รับอนุญาต");
			searchName.Add("วันหมดอายุ การอนุญาต");
			Debug.Log(data.contentListDangerRegister[index].txcrgttpnm);
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcrgtno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].ttxcnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].etxcn));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcrgttpnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcusednm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcfrmtnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txclctnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcaddr));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcpdlctnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcpdaddr));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcappvdate));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerRegister[index].txcexpdate));
		}
		else if(ProductType=="2.2"){ // Danger License
			searchName.Add("เลขทะเบียน");
			searchName.Add("เลขที่ใบอนุญาต");
			searchName.Add("ประเภท");
			searchName.Add("ชื่อการค้า (TH)");
			searchName.Add("ชื่อการค้า (EN)");
			searchName.Add("ประเภทผลิตภัณฑ์");
			searchName.Add("รูปแบบผลิตภัณฑ์");
			searchName.Add("ชื่อผู้ผลิต");
			searchName.Add("ที่ตั้งผู้ผลิต");
			searchName.Add("ชื่อผู้รับอนุญาต");
			searchName.Add("ที่ตั้งผู้รับอนุญาต");
			//searchName.Add("txclctcode");
			//searchName.Add("txclcnsnm");
			Debug.Log(data.contentListDangerLicense[index].txclcntpnm);
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcrgno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txclcnno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txclcntpnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcttrdnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcetrdnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcusednm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcfrmtnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txclctnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txcaddr));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txckplctnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txckpaddr));
			//searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txclctcode));
			//searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDangerLicense[index].txclcnsnm));
		}
		else if(ProductType=="3"){ // Drug
			searchName.Add("pvncd");
			searchName.Add("lcnno");
			searchName.Add("ชื่อการค้า (TH)");
			searchName.Add("ชื่อการค้า (EN)");
			searchName.Add("ชื่อผู้ประกอบการ");
			searchName.Add("lcnsid");
			searchName.Add("lcntpcd");
			searchName.Add("rgttpcd");
			searchName.Add("drgtpcd");
			searchName.Add("appdate");
			searchName.Add("Newcode");
			searchName.Add("สถานที่");
			Debug.Log(data.contentListDrug[index].thadrgnm);
			
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].pvncd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].lcnno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].thadrgnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].engdrgnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].engfrgnnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].lcnsid));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].lcntpcd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].rgttpcd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].drgtpcd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].appdate));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].Newcode));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListDrug[index].fulladdr));
		}
		else if(ProductType=="4"){ // Food
			searchName.Add("ประเภทจดแจ้ง");
			searchName.Add("เลขสารบบ");
			searchName.Add("ประเภทอาหาร");
			searchName.Add("ชื่อการค้า (TH)");
			searchName.Add("ชื่อการค้า (EN)");
			searchName.Add("ประเภท ใบอนุญาต");
			searchName.Add("เลขสถานที่ / เลขใบอนุญาต");
			searchName.Add("ผู้รับอนุญาต");
			searchName.Add("ผู้นำเข้า");
			searchName.Add("สถานที่");
			Debug.Log(data.contentListFood[index].fnregntftpnm);
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnregntftpcd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnfdpdtnotxt));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnregntftpnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnprdnmt));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnprdnme));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnfdtypenm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnfdlcnno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnfdlcnsnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnfdlctnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListFood[index].fnlctaddr));
		}
		else if(ProductType=="5"){ // Tool
			//searchName.Add("ประเภทจดแจ้ง");
			searchName.Add("เลขสารบบ");
			searchName.Add("ประเภท");
			searchName.Add("ชื่อผลิตภัณฑ์ (TH)");
			searchName.Add("ชื่อผลิตภัณฑ์ (EN)");
			searchName.Add("ประเภท ใบอนุญาต");
			searchName.Add("เลขสถานที่ / เลขใบอนุญาต");
			searchName.Add("ผู้รับอนุญาต");
			//searchName.Add("ผู้นำเข้า");
			searchName.Add("สถานที่");
			Debug.Log(data.contentListTool[index].rgttpcd);
			//searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].rgttpcd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].lcnno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].rgttpcd));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].thamdnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].engmdnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].lcnsid));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].lcnno));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].engfrgnnm));
			//searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].fnfdlctnm));
			searchData.Add(StringUtil.ParseUnicodeEscapes(data.contentListTool[index].fulladdr));
		}
	}
	
	public void ClearContentData(){
		Debug.Log(searchList.Count);
		//if (searchList == null)
		//return;
		for (int i = 0; i < searchList.Count; i++) {
			Debug.Log("Destroy Search List");
			Destroy(searchList[i]);
		}
		//if (resultList == null)
		//return;
		for (int i = 0; i < resultList.Count; i++) {
			Destroy(resultList[i]);
		}
		
		inputBox.text = "";
		
		searchName.Clear ();
		searchData.Clear ();
		searchList.Clear ();
		resultList.Clear ();
		resultState = false;
		IsDrag = false;
	}
	
	private void ClearList()
	{
		if (resultList == null)
			return;
		for (int i = 0; i < resultList.Count; i++) {
			Destroy(resultList[i]);
		}
	}
	public bool returnenableClick(){
		//		IsDrag = false;
		return enableClick;
	}
	public void setScrollable(float height){
		scrollAreaHeight = height;
		Debug.Log ("scrollAreaHeight :"+scrollAreaHeight);
	}
}
