using UnityEngine;
using System.Collections;

public class PackageTag : MonoBehaviour {
	public Sprite[] PackageList;
	public void SetPackageIndex(int idx)
	{
		SpriteRenderer rd = GetComponent<SpriteRenderer> ();
		if (idx < PackageList.Length)
						rd.sprite = PackageList [idx];
				else
						Debug.LogWarning ("PackageList out of Range");
	}

	public void Eat()
	{
		StartCoroutine ("EatPackage");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator EatPackage()
	{
		Vector3 eatPosition = Game2Global.GetEatingPoint ();
		while (Vector3.Distance(transform.position,eatPosition) > 0.4f) {
			transform.position = Vector3.Lerp(transform.position,eatPosition,Time.deltaTime * 5);
			yield return null;
		}
		Destroy (this.gameObject);
		yield return null;
	}
}
