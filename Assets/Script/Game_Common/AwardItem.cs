using UnityEngine;
using System.Collections;
using TMPro;

public class AwardItem : MonoBehaviour {
	public TextMeshPro TitleText;
	public GameObject LineObject;
	public AwardProgress progressBar;

	public SpriteRenderer awardSprite;
	public Sprite awardNone;
	public Sprite awardGold;

	public ButtonObject awardPopup;

	string popupTxt;

	public void SetupAward(int idx,float AwardHeight,string name,int progress,int max,string popupText)
	{
		popupTxt = StringUtil.ParseUnicodeEscapes(popupText);
		if (idx == 0)
			LineObject.SetActive(false);
		transform.localPosition = new Vector3(0,idx*-AwardHeight,0);
		bool IsGold = progressBar.SetUpAward(progress,max);
		if (IsGold)
			awardSprite.sprite = awardGold;
		else
			awardSprite.sprite = awardNone;
		TitleText.text = StringUtil.ParseUnicodeEscapes(name);
	}

	void Awake() {
		awardPopup.OnReleased += popupAction;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void popupAction()
	{
		PopupObject.ShowAlertPopup("Award",popupTxt,"ปิด");
	}
}
