using UnityEngine;
using System.Collections;
using TMPro;

public class BMIResult : MonoBehaviour {
	public static BMIResult pGlobal;
	public BMISoundController mSound;
	public Animator Female;
	public Animator Male;
	public Animator BGAnimation;
	public GameObject BMIPictureVeryFat;
	public GameObject BMIPictureFat;
	public GameObject BMIPictureNormal;
	public GameObject BMIPictureSlim;
	public GameObject BMIPictureVerySlim;
	public TextMeshPro BMIValue;
	public TextMeshPro BMILevel;
	public TextMeshPro BMRVal;
	public TextMeshPro BURNVal;
	public GameObject BMIBubbleMaleVeryFat;
	public GameObject BMIBubbleMaleFat;
	public GameObject BMIBubbleMaleNormal;
	public GameObject BMIBubbleMaleSlim;
	public GameObject BMIBubbleMaleVerySlim;
	public GameObject BMIBubbleFemaleVeryFat;
	public GameObject BMIBubbleFemaleFat;
	public GameObject BMIBubbleFemaleNormal;
	public GameObject BMIBubbleFemaleSlim;
	public GameObject BMIBubbleFemaleVerySlim;

	private float rbmival;
	private float rbmrval;
	private float rburncal;

	void Awake(){
		pGlobal = this;
	}

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void settingResult(float bmival,float bmrval,float burncal)
	{
		rbmival = bmival;
		rbmrval = bmrval;
		rburncal = burncal;
		Debug.Log(BTBMICal.sex+" "+bmival);
		pGlobal.BMIValue.text = (Mathf.Round(bmival * 100f) / 100f).ToString();
		pGlobal.BMRVal.text = "<color=red>"+(Mathf.Round(bmrval)).ToString()+"</color>  kcal";
		pGlobal.BURNVal.text = "<color=red>"+(Mathf.Round(burncal)).ToString()+"</color>  kcal";
		pGlobal.BGAnimation.enabled = true;
		mSound.playBMISound("cal");
		if(BTBMICal.sex){
			pGlobal.Male.enabled = false;
			pGlobal.Female.enabled = true;
			pGlobal.Male.gameObject.SetActive(false);
			pGlobal.Female.gameObject.SetActive(true);
		}
		else{
			pGlobal.Male.enabled = true;
			pGlobal.Female.enabled = false;
			pGlobal.Male.gameObject.SetActive(true);
			pGlobal.Female.gameObject.SetActive(false);
		}
		if(bmival<=15){ //very slim
			pGlobal.BMIPictureVerySlim.SetActive(true);
			pGlobal.BMILevel.text = "ผอมมาก";
			pGlobal.BGAnimation.Play("BGVerySlim");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleVerySlim");
				mSound.playBMISound("girlveryslim");
				pGlobal.BMIBubbleFemaleVerySlim.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleVerySlim");
				mSound.playBMISound("boyveryslim");
				pGlobal.BMIBubbleMaleVerySlim.SetActive(true);
			}
		}
		else if(15<bmival && bmival<18.5){ //slim
			pGlobal.BMIPictureSlim.SetActive(true);
			pGlobal.BMILevel.text = "ผอม";
			pGlobal.BGAnimation.Play("BGSlim");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleSlim");
				mSound.playBMISound("girlslim");
				pGlobal.BMIBubbleFemaleSlim.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleSlim");
				mSound.playBMISound("boyslim");
				pGlobal.BMIBubbleMaleSlim.SetActive(true);
			}
		}
		else if(18.5<=bmival && bmival<23.4){ //normal
			pGlobal.BMIPictureNormal.SetActive(true);
			pGlobal.BMILevel.text = "กำลังดี";
			pGlobal.BGAnimation.Play("BGNormal");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleNormal");
				mSound.playBMISound("girlnormal");
				pGlobal.BMIBubbleFemaleNormal.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleNormal");
				mSound.playBMISound("boynormal");
				pGlobal.BMIBubbleMaleNormal.SetActive(true);
			}
		}
		else if(23.4<=bmival && bmival<28.4){ //fat
			pGlobal.BMIPictureFat.SetActive(true);
			pGlobal.BMILevel.text = "อ้วน";
			pGlobal.BGAnimation.Play("BGFat");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleFat");
				mSound.playBMISound("girlfat");
				pGlobal.BMIBubbleFemaleFat.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleFat");
				mSound.playBMISound("boyfat");
				pGlobal.BMIBubbleMaleFat.SetActive(true);
			}
		}
		else if(bmival>=28.4){ //very fat
			pGlobal.BMIPictureVeryFat.SetActive(true);
			pGlobal.BMILevel.text = "อ้วนมาก";
			pGlobal.BGAnimation.Play("BGVeryFat");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleVeryFat");
				mSound.playBMISound("girlveryfat");
				pGlobal.BMIBubbleFemaleVeryFat.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleVeryFat");
				mSound.playBMISound("boyveryfat");
				pGlobal.BMIBubbleMaleVeryFat.SetActive(true);
			}
		}
		else{}
	}
	
	public void reSettingResult()
	{
		Debug.Log(BTBMICal.sex+" "+rbmival);
		pGlobal.BMIValue.text = (Mathf.Round(rbmival * 100f) / 100f).ToString();
		pGlobal.BMRVal.text = "<color=red>"+(Mathf.Round(rbmrval)).ToString()+"</color>  kcal";
		pGlobal.BURNVal.text = "<color=red>"+(Mathf.Round(rburncal)).ToString()+"</color>  kcal";
		pGlobal.BGAnimation.enabled = true;
		mSound.playBMISound("cal");
		if(BTBMICal.sex){
			pGlobal.Male.enabled = false;
			pGlobal.Female.enabled = true;
			pGlobal.Male.gameObject.SetActive(false);
			pGlobal.Female.gameObject.SetActive(true);
		}
		else{
			pGlobal.Male.enabled = true;
			pGlobal.Female.enabled = false;
			pGlobal.Male.gameObject.SetActive(true);
			pGlobal.Female.gameObject.SetActive(false);
		}
		if(rbmival<=15){ //very slim
			pGlobal.BMIPictureVerySlim.SetActive(true);
			pGlobal.BMILevel.text = "ผอมมาก";
			pGlobal.BGAnimation.Play("BGVerySlim");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleVerySlim");
				mSound.playBMISound("girlveryslim");
				pGlobal.BMIBubbleFemaleVerySlim.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleVerySlim");
				mSound.playBMISound("boyveryslim");
				pGlobal.BMIBubbleMaleVerySlim.SetActive(true);
			}
		}
		else if(15<rbmival && rbmival<18.5){ //slim
			pGlobal.BMIPictureSlim.SetActive(true);
			pGlobal.BMILevel.text = "ผอม";
			pGlobal.BGAnimation.Play("BGSlim");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleSlim");
				mSound.playBMISound("girlslim");
				pGlobal.BMIBubbleFemaleSlim.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleSlim");
				mSound.playBMISound("boyslim");
				pGlobal.BMIBubbleMaleSlim.SetActive(true);
			}
		}
		else if(18.5<=rbmival && rbmival<23.4){ //normal
			pGlobal.BMIPictureNormal.SetActive(true);
			pGlobal.BMILevel.text = "กำลังดี";
			pGlobal.BGAnimation.Play("BGNormal");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleNormal");
				mSound.playBMISound("girlnormal");
				pGlobal.BMIBubbleFemaleNormal.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleNormal");
				mSound.playBMISound("boynormal");
				pGlobal.BMIBubbleMaleNormal.SetActive(true);
			}
		}
		else if(23.4<=rbmival && rbmival<28.4){ //fat
			pGlobal.BMIPictureFat.SetActive(true);
			pGlobal.BMILevel.text = "อ้วน";
			pGlobal.BGAnimation.Play("BGFat");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleFat");
				mSound.playBMISound("girlfat");
				pGlobal.BMIBubbleFemaleFat.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleFat");
				mSound.playBMISound("boyfat");
				pGlobal.BMIBubbleMaleFat.SetActive(true);
			}
		}
		else if(rbmival>=28.4){ //very fat
			pGlobal.BMIPictureVeryFat.SetActive(true);
			pGlobal.BMILevel.text = "อ้วนมาก";
			pGlobal.BGAnimation.Play("BGVeryFat");
			if(BTBMICal.sex){
				pGlobal.Female.Play("FemaleVeryFat");
				mSound.playBMISound("girlveryfat");
				pGlobal.BMIBubbleFemaleVeryFat.SetActive(true);
			}
			else{
				pGlobal.Male.Play("MaleVeryFat");
				mSound.playBMISound("boyveryfat");
				pGlobal.BMIBubbleMaleVeryFat.SetActive(true);
			}
		}
		else{}
	}
}
