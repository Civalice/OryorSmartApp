using UnityEngine;
using System.Collections;
using TMPro;

public class TextLineLimit : MonoBehaviour {
	public int LineShow = 2;
	// Use this for initialization
	void Start () {
		TextMeshPro tmpro = GetComponent<TextMeshPro> ();
		tmpro.maxVisibleLines = LineShow;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
