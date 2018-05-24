using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FooterPageBar : MonoBehaviour {
	public Collider2D TouchCollider;
	public ContentMainSound mSound;
//	public XMLDownloader xmlData;
	
	public float PageLength = 2.0f;
	public int PageSize = 0;
	public float Sensitive = 1.0f;
	public float Smooth = 1.0f;

	public GameObject PagingObject;
	private List<TextMeshPro> PageList;
	private bool IsCreated = false;

	private MorphObject SnappingMorph;
	private int lastPageIndex = 0;
	private int currentPageIndex = 0;
	private float PageMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	private bool IsTouch = false;
	private Vector2 lastTouch;

	public bool IsFirstPage()
	{
		return currentPageIndex == 0;
	}

	public bool IsLastPage()
	{
		return currentPageIndex >= PageSize-1;
	}

	// Use this for initialization
	void Start () {
		SnappingMorph = new MorphObject ();
		PageList = new List<TextMeshPro> ();
		lastPageIndex = currentPageIndex;
	}
	public void CreatePaging(int maxPage)
	{
		PageSize = maxPage;
		Debug.Log ("Create page = "+PageSize);
		for (int i = 0; i < PageSize; i++) {
			GameObject tmp = (GameObject) GameObject.Instantiate(PagingObject);
			tmp.SetActive(true);
			tmp.transform.parent = this.transform;
			//			tmp.transform.localPosition = PagingObject.transform.localPosition;
			tmp.transform.localPosition = new Vector3(i*PageLength,0.12f,0);
			TextMeshPro tmpComp = tmp.GetComponent<TextMeshPro>();
			tmpComp.SetText("{0}",i+1);
			if (i == currentPageIndex)
			{
				tmpComp.color = Color.white;
			}
			else
			{
				tmpComp.color = new Color32(0,0,0,117);
			}
			PageList.Add(tmpComp);
		}
		IsCreated = true;
	}

	public void ClearPaging()
	{
		if (PageList == null)
			return;
		for (int i = 0;i < PageList.Count;i++)
		{
			Destroy(PageList[i].gameObject);
		}
		PageList.Clear ();
		PageSize = 0;
		IsCreated = false;
	}
	public void PrevPage()
	{
		if (currentPageIndex<=0) return;
		int index = currentPageIndex-1;
		if (index != currentPageIndex) {
			currentPageIndex = index;
			SnappingMorph.morphEasein (PageMoveMent, -currentPageIndex * PageLength, Mathf.Abs(currentMoveSpeed) / PageLength + Smooth);
			//start Coroutine
		}
		StopCoroutine("SnappingItem");
		StartCoroutine ("SnappingItem");
	}
	public void NextPage()
	{
		if (currentPageIndex>=PageSize-1) return;
		int index = currentPageIndex+1;
		if (index != currentPageIndex) {
			currentPageIndex = index;
			SnappingMorph.morphEasein (PageMoveMent, -currentPageIndex * PageLength, Mathf.Abs(currentMoveSpeed) / PageLength + Smooth);
			//start Coroutine
		}
		StopCoroutine("SnappingItem");
		StartCoroutine ("SnappingItem");

	}

	void PageItemUpdate () {
		if (IsTouch) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			PageMoveMent += (touchPos.x - lastTouch.x) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.x - lastTouch.x)* (Sensitive/10.0f) / (Time.deltaTime);
			currentMoveSpeed = MoveSpeed;
			if (PageMoveMent > 0)
				PageMoveMent = 0;
			if (PageMoveMent < -(PageLength * (PageSize - 1)))
				PageMoveMent = -PageLength * (PageSize - 1);
			lastTouch.x = touchPos.x;
			currentPageIndex = (int)(-(PageMoveMent - (PageLength / 2.0f)) / PageLength);
			SnappingMorph.morphEasein (PageMoveMent, -currentPageIndex * PageLength, Mathf.Abs(MoveSpeed) / PageLength + Smooth);
			//move all item
			for (int i = 0; i < PageSize; i++) {
				PageList [i].transform.localPosition = new Vector3 (PageMoveMent + i * PageLength, 0.12f, 0);
				if (i == currentPageIndex)
				{
					PageList [i].color = Color.white;
				}
				else
				{
					PageList [i].color = new Color32(0,0,0,117);
				}
			}
		} 
	}
	IEnumerator SnappingItem()
	{
		while (!SnappingMorph.IsFinish()) {
			SnappingMorph.Update ();
			PageMoveMent = SnappingMorph.val;
			
			if (PageMoveMent > 0)
				PageMoveMent = 0;
			if (PageMoveMent < -(PageLength * (PageSize - 1)))
				PageMoveMent = -PageLength * (PageSize - 1);
			
			for (int i = 0; i < PageSize; i++) {
				PageList [i].transform.localPosition = new Vector3 (PageMoveMent + i * PageLength, 0.12f, 0);
				int pageIdx = (int)(-(PageMoveMent - (PageLength / 2.0f)) / PageLength);
				currentPageIndex = pageIdx;
				if (i == pageIdx)
				{
					PageList [i].color = Color.white;
				}
				else
				{
					PageList [i].color = new Color32(0,0,0,117);
				}
			}
			yield return 0;
		}
		//load new XML
		if (lastPageIndex != currentPageIndex) {
			lastPageIndex = currentPageIndex;
			PageDetailGlobal.xmlDownload (currentPageIndex);
		}
	}
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		if (TouchCollider == null)
			return;
		if (PageList.Count <= 0) return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (TouchCollider.OverlapPoint(touchPos)) {
				StopCoroutine("SnappingItem");
				IsTouch = true;
				lastTouch = touchPos;
				mSound.playContentSound("click");
			}
		}
		else if (touchedUp) {
			if (IsTouch) {
				IsTouch = false;
				int index = (int)(-(PageMoveMent - (PageLength / 2.0f) + currentMoveSpeed * ((Smooth / 30.0f > 1) ? 1 : Smooth / 30.0f)) / PageLength);
				if (index != currentPageIndex) {
					currentPageIndex = index;
					SnappingMorph.morphEasein (PageMoveMent, -currentPageIndex * PageLength, Mathf.Abs(currentMoveSpeed) / PageLength + Smooth);
					//start Coroutine
				}
				StopCoroutine("SnappingItem");
				StartCoroutine ("SnappingItem");
			}
		}
		PageItemUpdate ();
	}
}
