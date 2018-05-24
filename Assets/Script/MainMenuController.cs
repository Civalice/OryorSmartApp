using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour {
	public ButtonObject btMoveLeft;
	public ButtonObject btMoveRight;

	public float layerLength = 30;
	public float DragSensitive = 2.5f;
	public float DragTreshold = 0.5f;
	public RoadLayer roadLayer;
	public List<PageLayer> pagePrefabs;

	public MainMenuSound mSound;
	
	private MorphObject rotateMorphVal;
	private int currentLayerNo;
	private List<GameObject> pageLayers;

	private float rotateDiff = 0;
	private int countCor = 0;
	private int onRotate = 0;

	// Input Position
	Vector2 lastTouchPos;
	bool IsMouseDown;
	bool IsDrag;
	bool IsDragArrow;
	// LayerValue
	float moveVal;
	// Use this for initialization
	void Awake () {
	}

	void Start () {
		btMoveLeft.OnReleased += moveLeftFunc;
		btMoveRight.OnReleased += moveRightFunc;
		rotateMorphVal = new MorphObject ();
		IsDrag = false;
		IsMouseDown = false;
//		moveVal = 0.0f;
		currentLayerNo = MainMenuGlobal.lastIndex;
		moveVal = 0.0f;
//		moveVal = currentLayerNo * layerLength;
		lastTouchPos = TouchInterface.GetTouchPosition();
		//create Layer
		pageLayers = new List<GameObject> ();
		int i = 0;
		PageLayer.maxPage = pagePrefabs.Count;
		PageLayer.pageLength = layerLength;
		foreach (PageLayer gObj in pagePrefabs) {
			if (gObj != null) {
				GameObject obj = (GameObject)GameObject.Instantiate (gObj.gameObject);
				pageLayers.Add (obj);
				PageLayer pObj = obj.GetComponent<PageLayer> ();
				pObj.setRotatePage (-layerLength * i++);
			}
		}
		foreach (GameObject gobj in pageLayers) {
			PageLayer obj = gobj.GetComponent<PageLayer> ();
			obj.RotatePageDiff (currentLayerNo * layerLength);
		}

		setCurrentLayerNo ();
	}
	// Update is called once per frame
	void Update () {
		if ((MainMenuGlobal.getCurrentState() != MainMenuState.MS_NORMAL)&& 
		    (MainMenuGlobal.getCurrentState() != MainMenuState.MS_DRAG)&&
		    (MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_DRAG)&&
		    (MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_SHORTCUT)
		    )
		{
			IsDrag = false;
			IsMouseDown = false;
			return;
		}
		updateTouch ();
		updateRotate ();
	}
	public void moveLeftFunc(){
		Debug.Log ("moveLeftFunc");
		rotateDiff = 0f;
		countCor = 0;
		onRotate = 1;
	}
	public void moveRightFunc(){
		Debug.Log ("moveRightFunc");
		rotateDiff = 0f;
		countCor = 0;
		onRotate = 2;
	}
	private void updateRotate(){
		if(onRotate==1 || onRotate==2){
			if(onRotate==1){rotateDiff = (-0.734f)*DragSensitive;}
			else if(onRotate==2){rotateDiff = (0.736f)*DragSensitive;}
			
			if (onRotate==1 || onRotate==2) {
				if ((MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_DRAG)&&
				    (MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_SHORTCUT))
				{
					MainMenuGlobal.SetState(MainMenuState.MS_DRAG);
				}
				IsDragArrow = true;
			}
			if (IsDragArrow) {
				
				moveVal += rotateDiff*8;
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.ResetBubble ();
				}
				roadLayer.SlideRoad (rotateDiff);
				//move layer here
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.RotatePageDiff (rotateDiff);
				}
				//get current Layer you're in
				//check current RotateVal length
			}
			if(countCor>=6){
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.PopBubble ();
					obj.PopObject ();
				}
				setCurrentLayerNo ();
//				rotateMorphVal.morphEasein (0, -moveVal, 10);
				IsDragArrow = false;
				onRotate = 0;
				moveVal = 0f;
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.RotatePageDiff (rotateMorphVal.dVal);
				}
				roadLayer.SlideRoad (rotateMorphVal.dVal);
				rotateMorphVal.Update ();
				if (rotateMorphVal.IsFinish())
				{
					if ((MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_DRAG)&&
					    (MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_SHORTCUT))
						
					{
						MainMenuGlobal.SetState(MainMenuState.MS_NORMAL);
					}
				}
			}
			countCor++;
		}
	}

	private void updateTouch(){
		float diff = 0;
		if (TouchInterface.GetTouchDown ()) {
			if (rotateMorphVal.IsFinish ()) {
				IsMouseDown = true;
				Vector2 mousePos = TouchInterface.GetTouchPosition ();
				lastTouchPos = mousePos;
			}
		}  
		if (TouchInterface.GetTouchUp ()) {
			if (IsMouseDown) {
				mSound.playSound("fw");
				if (IsDrag) {
					foreach (GameObject gobj in pageLayers) {
						PageLayer obj = gobj.GetComponent<PageLayer> ();
						obj.PopBubble ();
						obj.PopObject ();
					}
					if (MainMenuGlobal.getCurrentState() == MainMenuState.MS_TUTORIAL_DRAG)
					{
						MainMenuGlobal.Tutorial.NextTutorial();
					}
				}
				setCurrentLayerNo ();
				rotateMorphVal.morphEasein (0, -moveVal, 10);
				IsDrag = false;
				IsMouseDown = false;
			}
		}
		if (IsMouseDown) {
			Vector2 mousePos = TouchInterface.GetTouchPosition ();
			diff = (lastTouchPos.x - mousePos.x)*DragSensitive;
			if (Mathf.Abs (diff) > DragTreshold) {
				if ((MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_DRAG)&&
				    (MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_SHORTCUT))
				{
					MainMenuGlobal.SetState(MainMenuState.MS_DRAG);
				}
				IsDrag = true;
			}
			if (IsDrag) {
				lastTouchPos = mousePos;
				moveVal += diff;
				
				Debug.Log("IsDrag Cal Page No = "+currentLayerNo+" "+moveVal);
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.ResetBubble ();
				}
				roadLayer.SlideRoad (diff);
				//move layer here
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.RotatePageDiff (diff);
				}
				//get current Layer you're in
				//check current RotateVal length
			}
		} else {
			moveVal = 0;
			{
				//keep snap layer
				//snap layer by using rotateVal
				//get delta angle here
				foreach (GameObject gobj in pageLayers) {
					PageLayer obj = gobj.GetComponent<PageLayer> ();
					obj.RotatePageDiff (rotateMorphVal.dVal);
				}
				roadLayer.SlideRoad (rotateMorphVal.dVal);
				rotateMorphVal.Update ();
				if (rotateMorphVal.IsFinish())
				{
					if ((MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_DRAG)&&
						(MainMenuGlobal.getCurrentState() != MainMenuState.MS_TUTORIAL_SHORTCUT))

					{
						MainMenuGlobal.SetState(MainMenuState.MS_NORMAL);
					}
				}
			}
		}
	}

	private void UpdateState(){

	}

	private void setCurrentLayerNo(){
		while (moveVal > layerLength / 2) {
			currentLayerNo = (currentLayerNo + 1) % pageLayers.Count;
			moveVal = moveVal - layerLength;
				}
		while (moveVal < -layerLength / 2) {
			currentLayerNo = (currentLayerNo + pageLayers.Count - 1) % pageLayers.Count;
			moveVal = moveVal + layerLength;
				}
		MainMenuGlobal.lastIndex = currentLayerNo;
		Debug.Log("CurrentLayer No = "+currentLayerNo+" "+moveVal);
	}
}

