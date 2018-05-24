using UnityEngine;
using System.Collections;

public class CheckMainSound : MonoBehaviour {
	public AudioSource soundClick;
	public AudioSource soundCheck;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playCheckSound(string txt){
		if(txt == "click"){
			soundClick.Play();
		}
		else if(txt == "check"){
			Debug.Log("Calculator");
			soundCheck.Play();
		}
	}
}
