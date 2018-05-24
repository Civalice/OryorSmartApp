using UnityEngine;
using System.Collections;

public class LicenseDialog : MonoBehaviour {
	public ButtonObject CloseButton;
	public ButtonObject AcceptButton;
	public ButtonObject TickButton;

	public GameObject TickObject;

	public GameObject SwapInObj;
	public GameObject SwapOutObj;

	private Vector3 SwapInPos;
	private Vector3 SwapOutPos;
	private bool IsTickAccept = false;
	private LicenseDialog pGlobal;
	void Awake () {
		SwapInPos = SwapInObj.transform.position;
		SwapOutPos = SwapOutObj.transform.position;
//		CloseButton.OnReleased += CloseLicense;
		AcceptButton.OnReleased += AcceptLicense;
		TickButton.OnReleased += TickLicense;
		TickObject.SetActive(false);
		pGlobal = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Popout()
	{
		Debug.Log ("TTT");
		while(Vector3.Distance(this.transform.position,SwapOutPos) > 0.05f)
		{
			this.transform.position = Vector3.Lerp(this.transform.position,SwapOutPos,Time.deltaTime*10);
			yield return null;
		}
		this.transform.position = SwapOutPos;
//		this.gameObject.SetActive(false);
	}

	void TickLicense()
	{
		IsTickAccept = !IsTickAccept;
		if (IsTickAccept)
		{
//			UserCommonData.SetAG(true);
			TickObject.SetActive(true);
		}
		else
		{
			TickObject.SetActive(false);
		}
	}

	void CloseLicense()
	{
		StopCoroutine ("Popout");
		StartCoroutine ("Popout");
	}

	void AcceptLicense()
	{
		if (IsTickAccept) {
			UserCommonData.SetAG(true);
//			AgreementDrag.onAcceptLicense = false;
//			StopCoroutine ("Popout");
//			StartCoroutine ("Popout");this.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
		} else {
			PopupObject.ShowAlertPopup("ไม่สามารถไปต่อได้","กรุณากดยอมรับข้อตกลงก่อนใช้งาน","ตกลง");
		}
	}
}
