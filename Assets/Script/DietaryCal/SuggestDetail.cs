using UnityEngine;
using System.Collections;
using TMPro;

public class SuggestDetail : MonoBehaviour {
	public TextMeshPro actText;
	public TextMeshPro calText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setText(string act, string cal){
		actText.text = act;
		calText.text = cal;
	}
}
