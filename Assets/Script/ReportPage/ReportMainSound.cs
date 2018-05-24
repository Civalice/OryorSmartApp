using UnityEngine;
using System.Collections;

public class ReportMainSound : MonoBehaviour {
	public AudioSource soundClick;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playReportSound(string txt){
		if(txt == "click"){
			soundClick.Play();
		}
		else if(txt == "check"){
			
		}
	}
}
