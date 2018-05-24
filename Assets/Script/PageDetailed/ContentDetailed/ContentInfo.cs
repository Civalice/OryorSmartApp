using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ContentInfo : MonoBehaviour {
	public TextMeshPro titleText;
	public Collider2D UnTouchArea;

	public List<ContentObject> ContentList = new List<ContentObject>();

	public GameObject ContentObjectPrefabs;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;

	public ContentData cData;
	
	private bool IsTouch = false;
	private MorphObject SnappingMorph = new MorphObject ();
	private Vector2 lastTouch;
	private float height = 0;
	public float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;

	private float detailHeight = 0.0f;

	public void SetContentFavourite (ContentData data)
	{
		ClearContent ();
		cData = data;
		float heightSummary = 0;
		
		titleText.text = "<b>"+StringUtil.ParseUnicodeEscapes(data.title)+"</b>";
		titleText.ForceMeshUpdate ();
		heightSummary -= (titleText.bounds.size.y + 0.5f - titleText.transform.localPosition.y);
		
		//max content
		int maxObj;
		if ((data.detail != null) && (data.img != null))
		{
			maxObj = (data.detail.Length > data.img.Length) ? data.detail.Length : data.img.Length;
		}
		else if (data.detail == null)
		{
			maxObj = data.img.Length;
		}
		else
		{
			maxObj = data.detail.Length;
		}
		for (int i = 0;i < maxObj ;i++)
		{
			GameObject cObj = (GameObject)GameObject.Instantiate(ContentObjectPrefabs);
			cObj.SetActive(true);
			cObj.name = "Paragraph"+(i+1);
			cObj.transform.parent = this.transform;
			ContentObject cComp = cObj.GetComponent<ContentObject>();
			//setup component
			if ((data.img!=null)&&(i < data.img.Length))
			{
				cComp.setupImg("file://"+data.img[i]);				
			}
			if ((data.detail != null)&&(i < data.detail.Length))
			{
				cComp.setupDetailText(data.detail[i]);
			}
			cObj.transform.localPosition = new Vector3(0,heightSummary,0);
			if(data.detail[i]==""){detailHeight=0.5f;}
			else{detailHeight = 0.0f;}
			heightSummary -= (cComp.height - detailHeight + 0.5f);
			ContentList.Add(cComp);
		}
		height = -heightSummary;
	}

	public void SetContent (ContentData data)
	{
		ClearContent ();
		cData = data;
		float heightSummary = 0;

		titleText.text = "<b>"+StringUtil.ParseUnicodeEscapes(data.title)+"</b>";
		titleText.ForceMeshUpdate ();
		heightSummary -= (titleText.bounds.size.y + 0.5f - titleText.transform.localPosition.y);

		//max content
		int maxObj = (data.detail.Length > data.img.Length) ? data.detail.Length : data.img.Length;
		for (int i = 0;i < maxObj ;i++)
		{
			GameObject cObj = (GameObject)GameObject.Instantiate(ContentObjectPrefabs);
			cObj.SetActive(true);
			cObj.name = "Paragraph"+(i+1);
			cObj.transform.parent = this.transform;
			ContentObject cComp = cObj.GetComponent<ContentObject>();
			//setup component
			if (i < data.img.Length)
			{
				cComp.setupImg(data.img[i]);				
			}
			if (i < data.detail.Length)
			{
				cComp.setupDetailText(data.detail[i]);
			}
			Debug.Log("Hight Summary : "+heightSummary);
			cObj.transform.localPosition = new Vector3(0,heightSummary,0);
			if(data.detail[i]==""){detailHeight=0.5f;}
			else{detailHeight = 0.0f;}
			heightSummary -= (cComp.height - detailHeight + 0.5f);
			ContentList.Add(cComp);
		}
		height = -heightSummary;
	}
	public void ClearContent()
	{
		foreach (ContentObject c in ContentList) {
			c.Dispose();
			Destroy(c.gameObject);
		}
		ContentList.Clear ();
		ItemMoveMent = 0.0f;
		transform.localPosition = new Vector3 (0, 0, 0);

	}
	// Use this for initialization
	void Start () {
		
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
				StopCoroutine("SnappingItem");
				lastTouch = touchPos;
			}
		}
		if (touchedUp && IsTouch) 
		{
			//touch up here
			if (IsTouch) {
				IsTouch = false;
				StopCoroutine("SnappingItem");
				StartCoroutine ("SnappingItem");
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

	IEnumerator SnappingItem()
	{
		while (!SnappingMorph.IsFinish()) {
			SnappingMorph.Update ();
			ItemMoveMent = SnappingMorph.val;
			
			if (ItemMoveMent < 0)
				ItemMoveMent = 0;
			if (ItemMoveMent > height-6)
				ItemMoveMent = height-6;
			transform.localPosition = new Vector3(0,ItemMoveMent,0);

//			for (int i = 0; i < itemDetailList.Count; i++) {
//				itemDetailList [i].transform.localPosition = new Vector3 (0,ItemMoveMent -i*DetailLength, 0);
//			}

			yield return 0;
		}
	}

}
