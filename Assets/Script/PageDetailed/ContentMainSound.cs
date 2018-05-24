using UnityEngine;
using System.Collections;

public class ContentMainSound : MonoBehaviour {
	public AudioSource soundClick;
	public AudioSource soundSubmenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playContentSound(string txt){
		if(txt == "click"){
			soundClick.Play();
		}
		else if(txt == "submenu"){
			soundSubmenu.Play();
		}
	}
}
