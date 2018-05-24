using UnityEngine;
using System.Collections;

public class WarpPoint : MonoBehaviour {
	public Collider2D WarpCollider;
	public GameObject WarpTarget;
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
			float dx = item.transform.position.x - this.transform.position.x;
			Vector3 nextPoint = new Vector3(WarpTarget.transform.position.x-dx,
			                                WarpTarget.transform.position.y,
			                                WarpTarget.transform.position.z);
			item.transform.localPosition = nextPoint;
			item.MovingRight = true;
		}
	}
}
