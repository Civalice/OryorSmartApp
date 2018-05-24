using UnityEngine;
using System.Collections;
//Iphone 5 : 640x1136
//Iphone 4 : 640x960
//IPad : 768x1024
//568-480 = 88
public class ItemDetail : MonoBehaviour {
	public GameObject indecator;
	public DownloadSprite texture;

	public ContentData cData;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DestroyTexture()
	{
		texture.DisposeSprite();
	}

	public void SetContentData(ContentData data)
	{
		cData = data;
	}
	
	public void SetSortingOrder(int order)
	{
		if (indecator != null)
			indecator.GetComponent<Renderer>().sortingOrder = order + 1;
		texture.GetComponent<Renderer>().sortingOrder = order;
		this.GetComponent<Renderer>().sortingOrder = order-1;
	}
}
