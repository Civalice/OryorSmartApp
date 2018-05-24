using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {
	public GameObject FoodPrefabs;
	public float FoodItemDistance = 0.8f;
	private FoodItem lastestFood;
	public AudioClip movingSound;
	// Use this for initialization
	void Start() {}

	public void FirstSpawn() {
		//add 8 foodItem and set position
		FoodItem.IsMoving = true;
		for (int i = 0; i < 10; i++) {
			GameObject foodItem = (GameObject)GameObject.Instantiate (FoodPrefabs);
			Vector3 position = new Vector3(this.transform.position.x + i*FoodItemDistance,
			                               this.transform.position.y,
			                               this.transform.position.z);
			foodItem.transform.localPosition = position;
			FoodItem fd = foodItem.GetComponent<FoodItem> ();
			fd.CreatePackage();
			lastestFood = fd;
		}
		GetComponent<AudioSource>().PlayOneShot(movingSound);
	}

	public void Spawn()
	{
		GameObject foodItem = (GameObject)GameObject.Instantiate (FoodPrefabs);
		Vector3 position = new Vector3(lastestFood.transform.position.x + FoodItemDistance,
		                               lastestFood.transform.position.y,
		                               lastestFood.transform.position.z);
		foodItem.transform.localPosition = position;
		FoodItem fd = foodItem.GetComponent<FoodItem> ();
		fd.CreatePackage();
		lastestFood = fd;
		GetComponent<AudioSource>().PlayOneShot(movingSound);
	}

	// Update is called once per frame
	void Update () {

	}
}
