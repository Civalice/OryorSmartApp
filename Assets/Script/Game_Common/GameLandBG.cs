using UnityEngine;
using System.Collections;

public class GameLandBG : MonoBehaviour {
	public GameObject topBG;
	public GameObject midBG;
	public GameObject botBG;

	public void setHeight(float height)
	{
		midBG.transform.localScale = new Vector3(1,height/0.64f,1);
		botBG.transform.localPosition = new Vector3(0,-height,0);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
