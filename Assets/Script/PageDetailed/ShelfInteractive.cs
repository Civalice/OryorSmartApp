using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShelfInteractive : MonoBehaviour {

	public BoxCollider2D TouchArea;
	public ContentMainSound mSound;

	public ShelfContent content;
	public GameObject ShelfList;

	public GameObject ShelfItem;
	public ButtonObject PrevPageArrow;
	public ButtonObject NextPageArrow;
	public GameObject Indecator = null;
	public float DetailLength = 2.0f;
	public float Sensitive = 1.0f;
	public float Smooth = 1.0f;
	public float ScaleItemRate = 1.0f;
	private List<GameObject> ShelfItemList = new List<GameObject> ();

	private MorphObject SnappingMorph;
	private int currentShelfIndex = 0;
	private float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	private bool IsTouch = false;
	private Vector2 lastTouch;

	void PrevPage()
	{
		PageDetailGlobal.PrevPage();
	}
	void NextPage()
	{
		PageDetailGlobal.NextPage();
	}

	void Awake() 
	{
		TouchArea.size = new Vector2(TouchArea.size.x,TouchArea.size.y + ScreenRatio.SizeDiff());
		TouchArea.offset = new Vector2(TouchArea.offset.x,TouchArea.offset.y + ScreenRatio.SizeDiff()/2);
		//new scale
		float scale = ScreenRatio.CameraSize/4.8f;
		ShelfList.transform.localScale = new Vector3(scale,scale,0);
		ShelfList.transform.localPosition = new Vector3(ShelfList.transform.localPosition.x,
		                                                ShelfList.transform.localPosition.y + ScreenRatio.SizeDiff()/2,0);
		PrevPageArrow.OnReleased += PrevPage;
		NextPageArrow.OnReleased += NextPage;
	}

	// Use this for initialization
	void Start () {
				SnappingMorph = new MorphObject ();
		}

	public void CreateShelfItem (ContentData[] dataList,List<DownloadedTexture> dtxList,bool IsFirstPage,bool IsLastPage)
	{
		if (Indecator != null)
			Indecator.SetActive (false);
		if (dataList.Length <= 0) return;
		//create item detail
		for (int i = 0; i < dataList.Length; i++) {
			dataList[i].IsFav = PageDetailGlobal.IsFavourite(dataList[i].dataid);
			dataList[i].IsLike = PageDetailGlobal.GetLikeAlready(dataList[i].dataid);

			GameObject itemDetail = (GameObject)GameObject.Instantiate (ShelfItem);
			itemDetail.transform.parent = ShelfList.transform;
			itemDetail.transform.localPosition = new Vector3 (0 + i * DetailLength, 0, 0);
			
			float size = ((ScaleItemRate+4)-Mathf.Abs(itemDetail.transform.localPosition.x))/(ScaleItemRate+4) ;
			itemDetail.transform.localScale = new Vector3 (size,size,1);
//			itemDetail.renderer.sortingOrder = 10-(int)Mathf.Abs(itemDetail.transform.localPosition.x*100);

			ItemDetail idt = itemDetail.GetComponent<ItemDetail>();
			idt.SetSortingOrder(10-(int)Mathf.Abs(itemDetail.transform.localPosition.x*100));
			idt.texture.AddDownloadTexture(dtxList[i]);
			idt.SetContentData(dataList[i]);
			ShelfItemList.Add (itemDetail);
		}
		//create arrow item
		//set position
		PrevPageArrow.transform.localPosition = new Vector3 (-1 + -DetailLength, 0, 0);
		NextPageArrow.transform.localPosition = new Vector3 (1 + dataList.Length*DetailLength, 0, 0);
		if (!IsFirstPage)
		{

			PrevPageArrow.gameObject.SetActive(true);
		}
		else
		{
			PrevPageArrow.gameObject.SetActive(false);
		}
		if (!IsLastPage)
		{

			NextPageArrow.gameObject.SetActive(true);
		}
		else
		{
			NextPageArrow.gameObject.SetActive(false);
		}
		currentShelfIndex = 0;
		content.SetContent (ShelfItemList [currentShelfIndex].GetComponent<ItemDetail> ().cData);
		content.ShowContent ();
	}

	public void ClearShelfItem()
	{
		StopCoroutine("SnappingItem");
		content.HideContent();
		if (ShelfItemList == null)
						return;
		for (int i = 0; i < ShelfItemList.Count; i++) {
			ItemDetail idt = ShelfItemList[i].GetComponent<ItemDetail>();
			idt.DestroyTexture();
			Destroy(ShelfItemList[i]);
			}
		ShelfItemList.Clear ();
		PrevPageArrow.gameObject.SetActive(false);
		NextPageArrow.gameObject.SetActive(false);
		ItemMoveMent = 0;
		if (Indecator != null)
			Indecator.SetActive (true);
	}

	void ShelfItemUpdate () {
		if (IsTouch) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
//calculate last touch
			ItemMoveMent += (touchPos.x - lastTouch.x) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.x - lastTouch.x)* (Sensitive/10.0f) / (Time.deltaTime);
			currentMoveSpeed = MoveSpeed;
			if (ItemMoveMent > 0)
					ItemMoveMent = 0;
			if (ItemMoveMent < -(DetailLength * (ShelfItemList.Count - 1)))
				ItemMoveMent = -DetailLength * (ShelfItemList.Count - 1);
			lastTouch.x = touchPos.x;
			currentShelfIndex = (int)(-(ItemMoveMent - (DetailLength / 2.0f)) / DetailLength);
			SnappingMorph.morphEasein (ItemMoveMent, -currentShelfIndex * DetailLength, Mathf.Abs(MoveSpeed) / DetailLength + Smooth);
//move all item
			for (int i = 0; i < ShelfItemList.Count; i++) {
					ShelfItemList [i].transform.localPosition = new Vector3 (ItemMoveMent + i * DetailLength, 0, 0);
					float size = ((ScaleItemRate+4)-Mathf.Abs(ShelfItemList [i].transform.localPosition.x))/(ScaleItemRate+4) ;
					ShelfItemList [i].transform.localScale = new Vector3 (size,size,1);
					ItemDetail idt = ShelfItemList [i].GetComponent<ItemDetail>();
					idt.SetSortingOrder(10-(int)Mathf.Abs(ShelfItemList [i].transform.localPosition.x*100));
				}
//move arrow item
			PrevPageArrow.transform.localPosition = new Vector3 (-1+ItemMoveMent + -DetailLength, 0, 0);
			NextPageArrow.transform.localPosition = new Vector3 (1+ItemMoveMent + ShelfItemList.Count*DetailLength, 0, 0);
			} 
		}
	IEnumerator SnappingItem()
	{
		if (ShelfItemList.Count <= 0) yield break;
		while (!SnappingMorph.IsFinish()) {
						SnappingMorph.Update ();
						ItemMoveMent = SnappingMorph.val;

						if (ItemMoveMent > 0)
								ItemMoveMent = 0;
			if (ItemMoveMent < -(DetailLength * (ShelfItemList.Count - 1)))
				ItemMoveMent = -DetailLength * (ShelfItemList.Count - 1);
				currentShelfIndex = (int)(-(ItemMoveMent - (DetailLength / 2.0f)) / DetailLength);
						for (int i = 0; i < ShelfItemList.Count; i++) {
								ShelfItemList [i].transform.localPosition = new Vector3 (ItemMoveMent + i * DetailLength, 0, 0);
								float size = ((ScaleItemRate+4)-Mathf.Abs(ShelfItemList [i].transform.localPosition.x))/(ScaleItemRate+4) ;
								ShelfItemList [i].transform.localScale = new Vector3 (size,size,1);
								ItemDetail idt = ShelfItemList [i].GetComponent<ItemDetail>();
								idt.SetSortingOrder(10-(int)Mathf.Abs(ShelfItemList [i].transform.localPosition.x*100));
							}
			PrevPageArrow.transform.localPosition = new Vector3 (-1+ItemMoveMent + -DetailLength, 0, 0);
			NextPageArrow.transform.localPosition = new Vector3 (1+ItemMoveMent + ShelfItemList.Count*DetailLength, 0, 0);
			yield return 0;
		}
		content.SetContent (ShelfItemList [currentShelfIndex].GetComponent<ItemDetail> ().cData);
		content.ShowContent ();
		//show IndexItem
	}


	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		if (!PageDetailGlobal.IsFinishLoading())
			return;
		if (PopupObject.IsPopup) return;
		if (TouchArea == null)
			return;
		if (ShelfItemList.Count <= 0) return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (TouchArea.OverlapPoint(touchPos)) {
				StopCoroutine("SnappingItem");
				IsTouch = true;
				mSound.playContentSound("click");
				lastTouch = touchPos;
				content.HideContent();
			}
		}
		else if (touchedUp) {
			if (IsTouch) {
				IsTouch = false;
				int index = (int)(-(ItemMoveMent - (DetailLength / 2.0f) + currentMoveSpeed * ((Smooth / 30.0f > 1) ? 1 : Smooth / 30.0f)) / DetailLength);
				if (index != currentShelfIndex) {
					currentShelfIndex = index;
				}
				SnappingMorph.morphEasein (ItemMoveMent, -currentShelfIndex * DetailLength, Mathf.Abs(currentMoveSpeed) / DetailLength + Smooth);
				StopCoroutine("SnappingItem");
				StartCoroutine ("SnappingItem");
			}
		}
		ShelfItemUpdate ();
	}
}