using UnityEngine;
using System.Collections;

public class TextWrap : MonoBehaviour {
	TextMesh tMesh;
	// Use this for initialization
	void Start () {
		tMesh = GetComponent<TextMesh> ();
		Debug.Log (tMesh.characterSize);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
