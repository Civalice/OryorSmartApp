using UnityEngine;
using System.Collections;

public class ListSlider : MonoBehaviour {
	public Collider2D touchArea;
	public Collider2D touchAreaGameList;
	public float Sensitive = 5.0f;
	public float Smooth = 30.0f;
	public float ListHeight = 5.0f;
	public float ListHeightGap = 3.0f;
	
	private MorphObject SnappingMorph = new MorphObject();
	private float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	private bool IsTouch = false;
	private Vector2 lastTouch;
	private bool IsDrag = false;
	private Vector3 currentPos;
	
	public float lastHeight;
	public GameObject GameScroll;
	
	void Awake() 
	{
		lastHeight = transform.localPosition.y;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	public void ResetList()
	{
		ItemMoveMent = 0.0f;
		MoveSpeed = 0.0f;
		currentMoveSpeed = 0.0f;
		transform.localPosition = new Vector3(0,ItemMoveMent+lastHeight,0);
	}
	
	IEnumerator SnappingItem()
	{
		while (!SnappingMorph.IsFinish()) {
			SnappingMorph.Update ();
			ItemMoveMent = SnappingMorph.val;
			if (ListHeight-ListHeightGap < 0)
				yield break;
			if (ItemMoveMent < 0)
				ItemMoveMent = 0;
			if (ItemMoveMent > ListHeight-ListHeightGap)
				ItemMoveMent = ListHeight-ListHeightGap;
			
			transform.localPosition = new Vector3(0,ItemMoveMent+lastHeight,0);
			yield return 0;
		}
	}
	
	void ItemListUpdate () {
		if (IsTouch) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			ItemMoveMent += (touchPos.y - lastTouch.y) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.y - lastTouch.y)* (Sensitive/10.0f) / (Time.deltaTime);
			currentMoveSpeed = MoveSpeed;
			if (ListHeight-ListHeightGap < 0)
				return;
			if (ItemMoveMent < 0)
				ItemMoveMent = 0;
			if (ItemMoveMent > ListHeight-ListHeightGap)
				ItemMoveMent = ListHeight-ListHeightGap;
			lastTouch.y = touchPos.y;
			SnappingMorph.morphEasein (ItemMoveMent, ItemMoveMent+currentMoveSpeed, Mathf.Abs(MoveSpeed) / ListHeight + Smooth);
			//move all item
			transform.localPosition = new Vector3(0,ItemMoveMent+lastHeight,0);
		} 
	}
	
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		if (touchArea == null)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		
		if (touchedDown) {
			if (touchArea.OverlapPoint(touchPos)) {
				StopCoroutine("SnappingItem");
				IsTouch = true;
				lastTouch = touchPos;
			}
			if(touchAreaGameList!=null){
				if (touchAreaGameList.OverlapPoint(touchPos)) {
					Debug.Log ("IsDrag");
					//				StopCoroutine("SnappingItem");
					IsTouch = true;
					lastTouch = touchPos;
					IsDrag = true;
					
					currentPos = GameScroll.transform.localPosition;
				}
			}
		}
		else if (touchedUp) {
			if (IsTouch) {
				IsTouch = false;
				IsDrag = false;
				StopCoroutine("SnappingItem");
				StartCoroutine ("SnappingItem");
			}
		}
		
		if (IsDrag)
		{
			//calculate CurrentMapPos
			currentPos = GameScroll.transform.localPosition;
			Vector3 movedPosition = new Vector3(currentPos.x + touchPos.x - lastTouch.x,
			                                    GameScroll.transform.localPosition.y,
			                                    0);
			//			MapObject.rigidbody2D.transform.localPosition = movedPosition;
			if(movedPosition.x>=-2.49f && movedPosition.x<=0f)
				GameScroll.transform.localPosition = movedPosition;
			lastTouch = touchPos;
		}
		else
		{
			//			GameScroll.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		}
		ItemListUpdate ();
	}
}
