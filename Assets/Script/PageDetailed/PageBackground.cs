using UnityEngine;
using System.Collections;

public class PageBackground : MonoBehaviour {
	public Color color;
	public GameObject[] ColorObject;
	// Use this for initialization
	void Start () {
		color = PageDetailGlobal.pageColor;
		foreach (GameObject obj in ColorObject) {
			if (obj != null)
			{
				SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
				renderer.color = color;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
