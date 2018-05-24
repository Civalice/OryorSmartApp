using UnityEngine;
using System.Collections;

public class DestroyBar : MonoBehaviour {

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
			FoodItem.RemoveFoodItem(item);
			Destroy(item.gameObject);
		}
	}
}
