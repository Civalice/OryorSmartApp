using UnityEngine;
using System.Collections;
using TMPro;

public class BTBMICal : MonoBehaviour {
	public GameObject ResultLayer;
	public GameObject Header;
	public GameObject Detail;
	public GameObject BMIDetailBT;
	public GameObject BMRDetailBT;
	public TextMeshPro AgeInput;
	public TextMeshPro HeightInput;
	public TextMeshPro WeightInput;
	public TextMeshPro Activities;
	public GameObject[] sexual;
	public static bool sex = true;
	public Collider2D mCollider;
	public Collider2D mArrowLeft;
	public Collider2D mArrowRight;
	public GameObject[] BMIBubble;
	public GameObject[] BMIPicture;

	public int age;
	public float height;
	public float weight;
	public float bmival;
	public float bmrval;
	public float burncal;

	public BMIResult BMIResult;
	public string[] activityName;
	public int activityChoose=0;

	public BMISoundController mSound;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if (mCollider.OverlapPoint(touchPos))
			{
				//show button
				mSound.playBMISound("click");
				if(AgeInput.text!="" && HeightInput.text!="" && WeightInput.text!=""){
					ResultLayer.SetActive (true);
					foreach(GameObject bubble in BMIBubble){
						bubble.SetActive(false);
					}
					foreach(GameObject picture in BMIPicture){
						picture.SetActive(false);
					}
					//Header.SetActive(false);
					Detail.SetActive(false);
					string txt = AgeInput.text.Replace(" ","");
					age = int.Parse(txt);
					txt = HeightInput.text.Replace(" ","");
					height = float.Parse(txt);
					txt = WeightInput.text.Replace(" ","");
					weight = float.Parse(txt);

					if(height>100){
						height = height / 100;
					}

					bmival = weight / (height*height);
					if(sex){
						bmrval = (float)(66+(13.7*weight)+(5*height*100)-(6.8*age));
					}
					else{
						bmrval = (float)(665+(9.6*weight)+(1.8*height*100)-(4.7*age));
					}

					if(activityChoose==0){
						burncal = (float)(bmrval * 1.2);
					}
					else if(activityChoose==1){
						burncal = (float)(bmrval * 1.375);
					}
					else if(activityChoose==2){
						burncal = (float)(bmrval * 1.55);
					}
					else if(activityChoose==3){
						burncal = (float)(bmrval * 1.725);
					}
					else if(activityChoose==4){
						burncal = (float)(bmrval * 1.9);
					}

					BMIDetailBT.SetActive(false);
					BMRDetailBT.SetActive(true);
					BMIResult.settingResult(bmival,bmrval,burncal);
				}
				else{

					PopupObject.ShowAlertPopup("ท่านกรอกข้อมูลไม่ครบถ้วน","กรุณากรอกอายุ ส่วนสูง และน้ำหนักให้ครบ","ตกลง",null);
				}

				//Debug.Log(sex+" "+age+" "+height+" "+weight+" "+bmival);
			}
			if(mArrowRight.OverlapPoint(touchPos)){
				mSound.playBMISound("click");
				if(activityChoose<(activityName.Length-1)){
					activityChoose++;
				}
				else{
					activityChoose=0;
				}
				Activities.text = activityName[activityChoose];
			}
			if(mArrowLeft.OverlapPoint(touchPos)){
				mSound.playBMISound("click");
				if(activityChoose>0){
					activityChoose--;
				}
				else{
					activityChoose=activityName.Length-1;
				}
				Activities.text = activityName[activityChoose];
			}
		}
	}
}
