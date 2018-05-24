using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NewsboardInfo : MonoBehaviour {
	public static NewsboardInfo pGlobal;
	public TextMeshPro titleText;
	public Collider2D UnTouchArea;
	
	public List<NewsboardContentObject> ContentList = new List<NewsboardContentObject>();
	public List<GameObject> ObjectList = new List<GameObject>();
	
	public GameObject ContentObjectPrefabs;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;
	
	public NewsboardDetail cData;
	
	private bool IsTouch = false;
	private MorphObject SnappingMorph = new MorphObject ();
	private Vector2 lastTouch;
	private float height = 0;
	public float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	
	private float detailHeight = 0.0f;

	
	public void SetContent (NewsboardDetail data)
	{
		ClearContent ();
		cData = data;
		float heightSummary = 0;
		Debug.Log ("Title : "+StringUtil.ParseUnicodeEscapes(data.title));
		titleText.text = "<b>"+StringUtil.ParseUnicodeEscapes(data.title)+"</b>";
		titleText.ForceMeshUpdate ();
		heightSummary -= (titleText.bounds.size.y + 0.5f - titleText.transform.localPosition.y);
		
		//max content
		int maxObj = (data.desc.Length > data.img.Length) ? data.desc.Length : data.img.Length;
		for (int i = 0;i < maxObj ;i++)
		{
			GameObject cObj = (GameObject)GameObject.Instantiate(ContentObjectPrefabs);
			cObj.SetActive(true);
			cObj.name = "Paragraph"+(i+1);
			cObj.transform.parent = this.transform;
			NewsboardContentObject cComp = cObj.GetComponent<NewsboardContentObject>();
			//setup component
			if (i < data.img.Length)
			{
				cComp.setupImg(data.img[i],i);				
			}
			if (i == 0)
			{
				cComp.setupDetailText(data.desc[i]);
			}
			Debug.Log("Hight Summary : "+heightSummary);
			cObj.transform.localPosition = new Vector3(0,heightSummary,0);
			if(data.desc[i]==""){detailHeight=0.5f;}
			else{detailHeight = 0.0f;}
			heightSummary -= (cComp.height - detailHeight + 0.5f);
			ObjectList.Add (cObj);
			ContentList.Add(cComp);
		}
		height = -heightSummary;
	}
	public void setContentPosition(float posHeight, int i){
		Debug.Log ("posHeight : "+posHeight+" , i : "+i);
		float heightSummary = 0;
		for(int a =i;a<ObjectList.Count;a++){
			if(ObjectList[a]!=null)
				ObjectList[a].transform.localPosition =  new Vector3(0,heightSummary,0);
			heightSummary -= (ContentList[a].height + 0.5f);
		}
	}
	public void ClearContent()
	{
		foreach (NewsboardContentObject c in ContentList) {
			c.Dispose();
			Destroy(c.gameObject);
		}
		titleText.text = "";
		ContentList.Clear ();
		ItemMoveMent = 0.0f;
		transform.localPosition = new Vector3 (0, 0, 0);
		
	}
	// Use this for initialization
	void Start () {
		pGlobal = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (UnTouchArea == null)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (!UnTouchArea.OverlapPoint(touchPos))
			{
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
		if (IsTouch) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			ItemMoveMent += (touchPos.y - lastTouch.y) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.y - lastTouch.y)* (Sensitive/10.0f) / (Time.deltaTime);
			currentMoveSpeed = MoveSpeed;
			if (ItemMoveMent < 0)
				ItemMoveMent = 0;
			if (ItemMoveMent > height-6)
				ItemMoveMent = height-6;
			lastTouch = touchPos;
			SnappingMorph.morphEasein (ItemMoveMent, ItemMoveMent+currentMoveSpeed, Mathf.Abs(MoveSpeed)/10.0f + Smooth*30.0f);
			//move all item
			transform.localPosition = new Vector3(0,ItemMoveMent,0);
			//			}
		} 
	}
}
