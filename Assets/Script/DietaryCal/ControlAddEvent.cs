using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("foodlist")]
public class FoodCalList{
	[XmlArray("list"),XmlArrayItem("food")]
	public foodcal[] foodcals;
	
	public static FoodCalList Load (TextAsset file)
	{
//		if (File.Exists(path))
		if (file != null)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(FoodCalList));
//			using (XmlReader reader = XmlReader.Create(path))
			StringReader strReader = new StringReader(file.ToString());
			using (	XmlReader reader = XmlReader.Create(strReader))
			{
				Debug.Log("File Food Found.");
				return serializer.Deserialize(reader) as FoodCalList;
			}
		}
		else
		{
			Debug.Log("File Not Found.");
			return new FoodCalList();
		}
	}
}
public class ControlAddEvent : MonoBehaviour {
	static public ControlAddEvent pGlobal;
	public DietarySaveAndLoadData snl;
	public ControlMainEvent cme;
	public GameObject AddPage;
	public GameObject MainPage;
	public GameObject AddBT;
	public GameObject EditBT;
	public TextMeshPro AddFoodName;
	public TextMeshPro AddFoodCal;
	public Food[] testFood;
	public Food SelectedFood;
	public FoodCalList fileXml;
	private List<Food> testFoodList = new List<Food>();
	private List<Food> filterFoodList = new List<Food>();
	private List<GameObject> FoodList = new List<GameObject>();
	private static bool chkSel=false;
	private bool IsEditMode=false;
	private Food editFood = new Food();
	private int editPos;
	public DietaryMainSound mSound;

	public GameObject ListTable;
	public DateTime saveDate;

	static public bool[] selCat = new bool[]{false,false,false,false,false,false,false,false,false,false};
	public bool IsSearchInput=false;
	
	public Collider2D mBox;
	public Collider2D mBoxFooter;
	private bool IsTouch = false;
	public bool IsDrag = false;
	private MorphObject SnappingMorph = new MorphObject ();
	private Vector2 lastTouch;
	public float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;
	private float height = 0;

	void Awake(){
		Debug.Log("ControlAddEvent Awake");
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}
	// Use this for initialization
	void Start () {
		pGlobal = this;
		TextAsset m_XmlAsset = (TextAsset)Resources.Load<TextAsset>("food");
//		fileXml = FoodCalList.Load(Application.streamingAssetsPath+"/food.xml");
		fileXml = FoodCalList.Load(m_XmlAsset);
		//init Food for testing
		if (fileXml.foodcals != null){
			Debug.Log("File Count : "+fileXml.foodcals.Length.ToString());
			testFood = new Food[fileXml.foodcals.Length];
			for(int i=0;i<fileXml.foodcals.Length;i++){
				testFood[i] = new Food();
				testFood[i].FoodName = fileXml.foodcals[i].foodName;
				testFood[i].reciveCal = int.Parse(fileXml.foodcals[i].foodCal);
				testFood[i].FoodID = int.Parse(fileXml.foodcals[i].foodcalid);
				if(fileXml.foodcals[i].cat1=="1"){testFood[i].selCat[0] = true;}
				else{testFood[i].selCat[0] = false;}
				if(fileXml.foodcals[i].cat2=="1"){testFood[i].selCat[1] = true;}
				else{testFood[i].selCat[1] = false;}
				if(fileXml.foodcals[i].cat3=="1"){testFood[i].selCat[2] = true;}
				else{testFood[i].selCat[2] = false;}
				if(fileXml.foodcals[i].cat4=="1"){testFood[i].selCat[3] = true;}
				else{testFood[i].selCat[3] = false;}
				if(fileXml.foodcals[i].cat5=="1"){testFood[i].selCat[4] = true;}
				else{testFood[i].selCat[4] = false;}
				if(fileXml.foodcals[i].cat6=="1"){testFood[i].selCat[5] = true;}
				else{testFood[i].selCat[5] = false;}
				if(fileXml.foodcals[i].cat7=="1"){testFood[i].selCat[6] = true;}
				else{testFood[i].selCat[6] = false;}
				if(fileXml.foodcals[i].cat8=="1"){testFood[i].selCat[7] = true;}
				else{testFood[i].selCat[7] = false;}
				if(fileXml.foodcals[i].cat9=="1"){testFood[i].selCat[8] = true;}
				else{testFood[i].selCat[8] = false;}
				if(fileXml.foodcals[i].cat10=="1"){testFood[i].selCat[9] = true;}
				else{testFood[i].selCat[9] = false;}

				testFoodList.Add(testFood[i]);
			}
		}

		FfilterFoodList();
		//createFoodList();
		transform.localPosition = new Vector3(0,2.15f,0);
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if(mBox.OverlapPoint(touchPos) && !(mBoxFooter.OverlapPoint(touchPos))){
				IsTouch = true;
//				if(lastTouch!=touchPos){
//					IsDrag = true;
//				}
				lastTouch = touchPos;
			}
		}
		if (touchedUp) 
		{
			//touch up here
			if (IsTouch) {
				IsTouch = false;
				IsDrag = false;
				//Debug.Log("Touch Up "+IsDrag);
			}
		}
		if(IsSearchInput){

		}
		ContentUpdate ();
	}
	void ContentUpdate () {
		if (IsTouch) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			ItemMoveMent += (touchPos.y - lastTouch.y) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.y - lastTouch.y)* (Sensitive/10.0f) / (Time.deltaTime);
			//Debug.Log(ItemMoveMent.ToString()+" "+height.ToString()+" "+IsDrag);
			currentMoveSpeed = MoveSpeed;
			if (ItemMoveMent < 2.15f)
				ItemMoveMent = 2.15f;
			if (ItemMoveMent > height)
				ItemMoveMent = height;
			lastTouch = touchPos;
			SnappingMorph.morphEasein (ItemMoveMent, ItemMoveMent+currentMoveSpeed, Mathf.Abs(MoveSpeed)/10.0f + Smooth*30.0f);
			if(currentMoveSpeed!=0){
				IsDrag = true;
			}
			//move all item
			transform.localPosition = new Vector3(0,ItemMoveMent,0);
			//			}
		} 
	}

	public void selectingCat(int selingCat){
		if(selingCat!=0){
			Debug.Log("Selected Category");
			mSound.playDietarySound("click");
			selCat[selingCat-1] = !selCat[selingCat-1];
			chkSel = true;

			if(!selCat[0] && !selCat[1] && !selCat[2] && !selCat[3] && !selCat[4] && !selCat[5] && !selCat[6] && !selCat[7] && !selCat[8] && !selCat[9])
				chkSel = false;
		}
		
		clearFoodList();
		FfilterFoodList();
	}
	private void clearFoodList(){
		if (FoodList == null)
			return;
		for (int i = 0; i < FoodList.Count; i++) {
			Destroy(FoodList[i]);
		}
		FoodList.Clear ();
	}
	private void FfilterFoodList(){
		if(filterFoodList!=null){
			filterFoodList.Clear ();
		}
		Debug.Log("testFoodList : "+testFoodList.Count+" "+chkSel);
		for(int i=0;i<testFoodList.Count;i++){
			bool chkmatch = true;
				if(!chkSel){
					if(testFoodList[i].FoodName!="" && IsSearchInput){
						if((testFoodList[i].FoodName.IndexOf(AddFoodName.text))!=-1){
							filterFoodList.Add(testFoodList[i]);
						}
					}
					else{
						filterFoodList.Add(testFoodList[i]);
					}
					
				}
				else{
					for(int j=0;j<selCat.Length;j++){
						if(selCat[j]){
							if(selCat[j]!=testFoodList[i].selCat[j]){
								chkmatch = false;
								break;
							}
						}
					}
					if(chkmatch){
//						filterFoodList.Add(testFoodList[i]);
						if(testFoodList[i].FoodName!="" && IsSearchInput){
							if((testFoodList[i].FoodName.IndexOf(AddFoodName.text))!=-1){
								filterFoodList.Add(testFoodList[i]);
							}
						}
						else{
							filterFoodList.Add(testFoodList[i]);
						}
					}
				}
		}
		createFoodList();
	}
	private void createFoodList(){
		Debug.Log("filterFoodList : "+filterFoodList.Count);
		transform.localPosition = new Vector3(0,2.15f,0);
		for(int i=0;i<filterFoodList.Count;i++){
			GameObject ListDetail = (GameObject)GameObject.Instantiate(ListTable);
			ListDetail.SetActive(true);
			ListDetail.transform.parent = this.transform;
			ListDetail.transform.localPosition = new Vector3 (0, 0.0f - i * 0.5f, 0);

			ListAddTable idt = ListDetail.GetComponent<ListAddTable>();
			idt.setContent(filterFoodList[i]);
			FoodList.Add (ListDetail);

			height = (2.15f + i * 0.5f);
			if(height <= 2.5f){
				height = 2.5f;
			}
		}
	}
	public void setText(Food sFood){
//		if(!IsDrag){
			AddFoodName.text = sFood.FoodName;
			AddFoodCal.text = sFood.reciveCal.ToString();
			SelectedFood = sFood;
			editFood = sFood;
			IsSearchInput = false;
//		}
	}

	public void AddData(){
		//Debug.Log("Add Data");
		if (AddFoodName.text != "" && AddFoodCal.text != "") {
				AddPage.SetActive (false);
				MainPage.SetActive (true);
				SelectedFood.FoodName = AddFoodName.text;
				SelectedFood.reciveCal = int.Parse (AddFoodCal.text);
//			SelectedFood.saveDate = DateTime.Now;
				if (!IsEditMode) {
						AddBT.SetActive (true);
						EditBT.SetActive (false);
						IsEditMode = false;
						cme.DataFoodList.Add (SelectedFood);
						cme.AddToListData ();
				} else {
		
						AddBT.SetActive (true);
						EditBT.SetActive (false);

						IsEditMode = false;
						Debug.Log (editFood.FoodName + " " + editPos);
						cme.editToListData (editFood, editPos);
				}
		} else {
			PopupObject.ShowAlertPopup("กรุณากรอกรายการอาหารและพลังงานที่ได้รับ","","ตกลง");
		}
	}
	public void CancelData(){
		IsEditMode = false;
		AddPage.SetActive(false);
		MainPage.SetActive(true);
	}
	
	public void setAddState(){
		AddBT.SetActive(true);
		EditBT.SetActive(false);

		clearFoodList();
		FfilterFoodList();
	}
	public void EditMode(Food selFood,int Pos){
		IsEditMode = true;
		editFood = selFood;
		editPos = Pos;

		AddBT.SetActive(false);
		EditBT.SetActive(true);

		AddFoodName.text = selFood.FoodName;
		AddFoodCal.text = selFood.reciveCal.ToString();

		clearFoodList();
		FfilterFoodList();
	}
}
