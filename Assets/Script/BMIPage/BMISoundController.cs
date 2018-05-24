using UnityEngine;
using System.Collections;

public class BMISoundController : MonoBehaviour {
	public AudioSource soundClick;
	public AudioSource soundCalculator;
	public AudioSource soundBoyVeryFat;
	public AudioSource soundBoyFat;
	public AudioSource soundBoyNormal;
	public AudioSource soundBoySlim;
	public AudioSource soundBoyVerySlim;
	public AudioSource soundGirlVeryFat;
	public AudioSource soundGirlFat;
	public AudioSource soundGirlNormal;
	public AudioSource soundGirlSlim;
	public AudioSource soundGirlVerySlim;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playBMISound(string txt){
		if(txt == "click"){
			soundClick.Play();
		}
		else if(txt == "cal"){
			Debug.Log("Calculator");
			soundCalculator.Play();
		}
		else if(txt == "boyveryfat"){
			soundBoyVeryFat.Play();
		}
		else if(txt == "boyfat"){
			soundBoyFat.Play();
		}
		else if(txt == "boynormal"){
			soundBoyNormal.Play();
		}
		else if(txt == "boyslim"){
			soundBoySlim.Play();
		}
		else if(txt == "boyveryslim"){
			soundBoyVerySlim.Play();
		}
		else if(txt == "girlveryfat"){
			soundGirlVeryFat.Play();
		}
		else if(txt == "girlfat"){
			soundGirlFat.Play();
		}
		else if(txt == "girlnormal"){
			soundGirlNormal.Play();
		}
		else if(txt == "girlslim"){
			soundGirlSlim.Play();
		}
		else if(txt == "girlveryslim"){
			soundGirlVerySlim.Play();
		}
	}
}
