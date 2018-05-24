using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodItem : MonoBehaviour {
	public static float movingSpeed = 0.0f;
	
	public GDATag gda;
	public PackageTag package;
	public bool IsTouchable = false;
	public bool MovingRight = false;
	public static bool IsMoving = true;
	public bool IsFree = false;
	public int extraToken = 0;
	public static List<FoodItem> foodList = new List<FoodItem> ();

	private Vector2 DragPos;
	private bool IsDrag = false;

	public static void ClearFoodList()
	{
		foreach(FoodItem fd in foodList)
		{
			if (fd != null)
			{
				Debug.Log("Destroy Food name "+fd.name);
				DestroyImmediate(fd.gameObject);
			}
		}
		foodList.Clear();
	}

	public static void RemoveFoodItem(FoodItem fd){
		foodList.Remove(fd);
	}

	public void CreatePackage()
	{
		IsFree = false;
		PigLvling currentLvl = Game2_LvlingStat.GetLvling();
		extraToken = Game2Global.GetExtraToken ();
		gda.SetTagFlag (Game2Global.GetRandomPackageFlag (extraToken+1));
		package.SetPackageIndex (Random.Range(0,5));
	}

	public static void ForceMoving(float x)
	{
		foreach (FoodItem fd in foodList) {
				float dx = x;
				if (fd.GetComponent<FoodItem>().MovingRight) {
						dx = -x;
				}
			Vector3 nextPos = new Vector3 (fd.transform.localPosition.x - dx, fd.transform.localPosition.y, fd.transform.localPosition.z);
			fd.transform.localPosition = nextPos;
		}
	}

	// Use this for initialization
	void Start () {
		foodList.Add (this);
	}

	public void ReachQueue()
	{
		IsTouchable = true;
	}

	// Update is called once per frame
	void Update () {
		if (Game2Global.GlobalPause)
		{
			GetComponent<Rigidbody2D>().Sleep();
			return;
		}
		else
		{
			GetComponent<Rigidbody2D>().WakeUp();
		}
		Animator anim = GetComponent<Animator> ();
		if (IsMoving) {
			anim.SetBool ("IsMoving", true);
			//movingspeed increase
			if (foodList [0] == this) {
				movingSpeed += 0.01f;
				if (movingSpeed > 1.0f)
					movingSpeed = 1.0f;
			}
			if (MovingRight) {
				anim.SetBool ("MovingRight", true);
				GetComponent<Rigidbody2D>().velocity = new Vector2 (movingSpeed * 60, 0);
			} else {
				anim.SetBool ("MovingRight", false);
				GetComponent<Rigidbody2D>().velocity = new Vector2 (-movingSpeed * 60, 0);
			}
		} else {
			anim.SetBool ("IsMoving", false);
			movingSpeed = 0;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);
		}

#if UNITY_EDITOR || UNITY_WEBPLAYER
		if (!IsDrag&&IsTouchable)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				//drop trash
				IsDrag = true;
				DropToTrash();
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				IsDrag = true;
				PigEating();
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			IsDrag = false;
		}
#endif
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		if (TouchDown) {
			if (IsTouchable && package.GetComponent<Collider2D>().OverlapPoint (pos)) {
				IsDrag = true;
				DragPos = pos;
			}
		} else if (TouchUp) {
			IsDrag = false;
		} else {
			if (IsDrag)
			{
				//check pos 
				if (((pos.x - DragPos.x) > 0.5f)&&!Game2Global.PigIsSick())
				{
					IsDrag = false;
					PigEating();
				}
				else if ((DragPos.y - pos.y) > 0.5f)
				{
					IsDrag = false;
					DropToTrash();
				}
				else if ((DragPos.x - pos.x > 0.5f))
				{
					IsDrag = false;
					DropToTrash();
				}
			}
		}
	}
	
	void PigEating()
	{
		IsTouchable = false;
		IsFree = true;
		Game2Global.Spawning();
		//release package
		package.transform.parent = null;
		package.GetComponent<Renderer>().sortingOrder = -5;
		//		foodList.Remove(this);
		Game2Global.PigEating (gda.GDAFlag);
		Game2Global.ReturnFlag (gda.GDAFlag);
		Game2Global.ReturnExtraToken (extraToken);
		if ((gda != null)&&(gda.gameObject!=null))
			Destroy(gda.gameObject);
		//play package move to Position
		package.Eat ();
		IsMoving = true;
	}

	void DropToTrash()
	{
		IsTouchable = false;
		IsFree = true;
		Game2Global.Spawning();
		//release package
		package.transform.parent = null;
		package.GetComponent<Renderer>().sortingOrder = -5;
		package.gameObject.AddComponent<Rigidbody2D>();
		package.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
//		foodList.Remove(this);
		Game2Global.ReturnFlag (gda.GDAFlag);
		Game2Global.ReturnExtraToken (extraToken);
		if ((gda != null)&&(gda.gameObject!=null))
			Destroy(gda.gameObject);
		IsMoving = true;
	}
}
