using UnityEngine;
using System.Collections;

public class MainMenuSound : MonoBehaviour {
	public AudioSource soundClick;
	public AudioSource soundFw;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playSound(string txt){
		if(txt == "click"){
			soundClick.Play();
		}
		else if(txt == "fw"){
			soundFw.Play();
		}
	}
}
