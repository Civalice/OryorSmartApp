using UnityEngine;
using System.Collections;
using TMPro;

public class ReportCancel : MonoBehaviour {
	public ButtonObject buttonCancel;
	public TextMeshPro name;
	public TextMeshPro email;
	public TextMeshPro tel;
	public TextMeshPro detail;
	void Awake()
	{
		buttonCancel.OnReleased += Clear;
		name.text = UserCommonData.pGlobal.user.user_firstname + "  " + UserCommonData.pGlobal.user.user_surname;
		email.text = UserCommonData.pGlobal.user.user_email;
		tel.text = UserCommonData.pGlobal.user.user_tel;
	}

	void Clear()
	{
		name.text = "";
		email.text = "";
		tel.text = "";
		detail.text = "";
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
