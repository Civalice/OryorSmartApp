using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartTank : MonoBehaviour {
	public GameObject HeartPrefab;
	public List<GameObject> HeartTankList = new List<GameObject>();
	private int lifeCount;
	// Use this for initialization
	void Start () {
	}

	public void SetupHeart(int life)
	{
		StartCoroutine ("CreateHeart",life);
	}

	IEnumerator CreateHeart(int life)
	{
		yield return new WaitForSeconds(1.0f);
//		int life = GlobalController.GetLifeCount ();
		lifeCount = life;
		for (int i = 0;i < life;i++)
		{
			yield return new WaitForSeconds(0.2f);
			GameObject heart = (GameObject)GameObject.Instantiate (HeartPrefab);
			heart.transform.parent = this.transform;
			heart.transform.localPosition = new Vector3(i*0.6f,0,0);
			HeartTankList.Add(heart);
		}
	}

	public void DecreaseLife()
	{
		if (lifeCount <= 0)
			return;
		lifeCount--;
		HeartTankList [lifeCount].GetComponent<Animator> ().Play ("HeartBreak");
	}

	public void ClearTank()
	{
		foreach (GameObject heart in HeartTankList) {
			DestroyObject(heart);
		}
		HeartTankList.Clear ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
