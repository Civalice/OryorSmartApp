using UnityEngine;
using System.Collections;
using TMPro;

public class ResultSearch : MonoBehaviour {
	static public ResultSearch pGlobal;
	public TextMeshPro txt1;
	public TextMeshPro txt2;


	// Use this for initialization
	void Start () {
		pGlobal = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setSearchContent(string text1, string text2){
		//Debug.Log("Create Search Check Text");
		txt1.text = text1;
		txt1.maxVisibleLines = 2;
		txt2.text = text2;
		txt2.maxVisibleLines = 2;
	}
}
