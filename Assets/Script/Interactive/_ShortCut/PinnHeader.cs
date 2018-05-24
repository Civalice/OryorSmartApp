using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PinnHeader : MonoBehaviour {
	public static PinnHeader pGlobal;

	public int count = 0;
	public GameObject ShiftList;
	public List<PinnItem> PinnList = new List<PinnItem>();
	private Vector3 ListPosition;

	public static void AddItem(PinnItem item)
	{
		pGlobal.mAddItem(item);
	}

	public static void ShiftUp(PinnItem item)
	{
		pGlobal.mShiftup(item);
	}

	public void mShiftup(PinnItem item)
	{
		bool found = false;
		for(int i = 0;i < PinnList.Count;i++)
		{
			if (found) PinnList[i].ShiftUp();
			if (PinnList[i] == item) found = true;
		}
		RemoveItem(item);
	}

	public void RemoveItem(PinnItem item)
	{
		PinnList.Remove(item);
		count--;
		//shift down position
		StopCoroutine("Shift");
		StartCoroutine("Shift");
	}

	public void mAddItem(PinnItem item)
	{
		PinnList.Add(item);
		item.Pinn(count);
		count++;
		//shift down position
		StopCoroutine("Shift");
		StartCoroutine("Shift");
		//set position before add in

	}

	IEnumerator Shift()
	{
		Vector3 currentPosition = ShiftList.transform.localPosition;
		Vector3 ShiftPos = new Vector3(ListPosition.x,ListPosition.y - count*0.6f,ListPosition.z);
		while (Vector3.Distance(currentPosition,ShiftPos) > 0.05f)
		{
			currentPosition = Vector3.Lerp(currentPosition,ShiftPos,Time.deltaTime*5);
			ShiftList.transform.localPosition = currentPosition;
			yield return null;
		}
		ShiftList.transform.localPosition = ShiftPos;
	}

	void Awake()
	{
		pGlobal = this;
		ListPosition = ShiftList.transform.localPosition;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
