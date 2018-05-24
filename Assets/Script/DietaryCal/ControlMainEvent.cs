using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ControlMainEvent : MonoBehaviour {
	public List<Food> DataFoodList = new List<Food>();
	public List<Food> updateFoodList = new List<Food>();
	public List<Food> tempFoodList = new List<Food>();
	public DietarySaveAndLoadData snl;
	public GameObject RecordTable;
	private List<GameObject> FoodList = new List<GameObject>();
	
	private int TotalCal=0;
	public TextMeshPro resultCal;

	public GameObject[] Mascot;
	private bool IsTouch = false;
	public Collider2D mBox;
	public Collider2D pBox;
	
	private MorphObject SnappingMorph = new MorphObject ();
	private Vector2 lastTouch;
	public float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;
	private float height = 0;
	private bool AllowScroll=true;
	
	public DateTime saveNow = DateTime.Now;

	long period = 10L * 60L * 1000L * 10000L;

	void Awake(){
		Debug.Log("ControlMainEvent Awake : "+saveNow+" "+System.DateTime.Now.Ticks);
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		//snl.loadData(DataFoodList);
	}
	// Use this for initialization
	void Start () {
		createRecordData();
		resultCal.text = TotalCal.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if(mBox.OverlapPoint(touchPos) && !(pBox.OverlapPoint(touchPos))){
				IsTouch = true;
				lastTouch = touchPos;
			}
		}
		if (touchedUp && IsTouch) 
		{
			//touch up here
			if (IsTouch) {
				IsTouch = false;
			}
		}
		ContentUpdate ();
	}
	void ContentUpdate () {
		if (IsTouch && AllowScroll) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			ItemMoveMent += (touchPos.y - lastTouch.y) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.y - lastTouch.y)* (Sensitive/10.0f) / (Time.deltaTime);
			//Debug.Log(ItemMoveMent.ToString()+" "+height.ToString());
			currentMoveSpeed = MoveSpeed;
			if (ItemMoveMent < 0)
				ItemMoveMent = 0;
			if (ItemMoveMent > height)
				ItemMoveMent = height;
			lastTouch = touchPos;
			SnappingMorph.morphEasein (ItemMoveMent, ItemMoveMent+currentMoveSpeed, Mathf.Abs(MoveSpeed)/10.0f + Smooth*30.0f);

			//move all item
			transform.localPosition = new Vector3(0,ItemMoveMent,0);
			//			}
		} 
	}

	public void AddToListData(){
		//DataFoodList.Add (SelectedFood);
		snl.saveData(DataFoodList);
		ClearAndCreateContentData();
	}

	private void createRecordData(){
		if(DataFoodList!=null)DataFoodList.Clear ();
		DataFoodList = snl.loadData();
		transform.localPosition = new Vector3(0,0,0);
		int j=0;
		for(int i=0;i<DataFoodList.Count;i++){
			if(DataFoodList[i].saveDate>=(DateTime.Now.Ticks - period)){
				TotalCal += DataFoodList[i].reciveCal;
				GameObject RecordDetail = (GameObject)GameObject.Instantiate(RecordTable);
				RecordDetail.SetActive(true);
				RecordDetail.transform.parent = this.transform;
				RecordDetail.transform.localPosition = new Vector3 (0, 0.4f - i * 0.8f, 0);
				
				ListTable idt = RecordDetail.GetComponent<ListTable>();
				idt.setContent(DataFoodList[i],j);
				updateFoodList.Add(DataFoodList[i]);
				FoodList.Add (RecordDetail);

				height = -(0.4f - j * 0.8f) - 2.0f;
				j++;
			}
		}
		DataFoodList = updateFoodList;
		Debug.Log(height);
		if(height<=0.6f){
			height = 0.6f;
			AllowScroll = false;
		}
		else{
			AllowScroll = true;
		}

		if(TotalCal==0){
			Mascot[0].SetActive(false);
		}
		else if(TotalCal<1500){
			Mascot[0].SetActive(true);
		}
		else if(TotalCal>2500){
			Mascot[2].SetActive(true);
		}
		else{
			Mascot[1].SetActive(true);
		}
		//Debug.Log(FoodList.Count);
	}
	private void ClearAndCreateContentData(){
		Debug.Log(FoodList.Count);
		Mascot[0].SetActive(false);
		Mascot[0].SetActive(false);
		Mascot[2].SetActive(false);
		Mascot[1].SetActive(false);
		TotalCal = 0;
		if (FoodList == null)
			return;
		for (int i = 0; i < FoodList.Count; i++) {
			Destroy(FoodList[i]);
		}
		FoodList.Clear ();
		createRecordData();
		resultCal.text = TotalCal.ToString();
	}

	public void deleteFoodList(Food delFood,int Pos){
		if(tempFoodList!=null) tempFoodList.Clear ();

		for(int i=0;i<DataFoodList.Count;i++){
			if(!(i==Pos && DataFoodList[i].FoodName == delFood.FoodName)){

				tempFoodList.Add (DataFoodList[i]);
			}
		}
		DataFoodList.Clear ();
		DataFoodList = tempFoodList;
		snl.saveData(DataFoodList);

		ClearAndCreateContentData();
	}
	public void editToListData(Food editFood,int Pos){
		if(tempFoodList!=null) tempFoodList.Clear ();
		
		for(int i=0;i<DataFoodList.Count;i++){

			if(i!=Pos){
				//Debug.Log("Original : "+i+" "+DataFoodList[i].FoodName);
				tempFoodList.Add(DataFoodList[i]);
			}
			else{
				//Debug.Log("Edit : "+Pos+" "+editFood.FoodName);
				tempFoodList.Add(editFood);
			}
			//Debug.Log("Edit : "+i+" "+DataFoodList[i].FoodName);
		}
		DataFoodList.Clear ();
		DataFoodList = tempFoodList;
		snl.saveData(DataFoodList);
		
		ClearAndCreateContentData();
	}

	private bool compareDate(DateTime saveDate){
		return true;
	}
}
