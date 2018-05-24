using UnityEngine;
using System.Collections;

public class PlayPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		FoodItem item = coll.gameObject.GetComponent<FoodItem> ();
		if (item != null) {
			if (!item.IsFree)
			{
				FoodItem.IsMoving = false;
				item.IsTouchable = true;
				//lerp item to right position;
				FoodItem.ForceMoving(this.transform.localPosition.x-coll.transform.localPosition.x);
			}
		}
	}
}
