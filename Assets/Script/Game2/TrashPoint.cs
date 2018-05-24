using UnityEngine;
using System.Collections;

public class TrashPoint : MonoBehaviour {
	public AudioClip trashSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		PackageTag item = coll.gameObject.GetComponent<PackageTag> ();
		if (item != null) {
			GetComponent<AudioSource>().PlayOneShot(trashSound);
			Destroy(item.gameObject);
		}
	}
}
