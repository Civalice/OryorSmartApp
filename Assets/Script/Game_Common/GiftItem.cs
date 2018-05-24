using UnityEngine;
using System.Collections;
using TMPro;

public class GiftItem : MonoBehaviour {
	public delegate IEnumerator EventAction();
	public event EventAction postAccept;
	public TextMeshPro NameText;
	public ButtonObject acceptGift;

	private string GiftID;

	public void SetGiftItem(int idx,float Height,GiftItemData data)
	{
		NameText.text = data.name;
		GiftID = data.GiftKey;
		transform.localPosition = new Vector3(0,idx*-Height,0);
	}
	void Awake () {
		acceptGift.OnReleased += PressAccept;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PressAccept()
	{
		LoadingScript.ShowLoading();
		StartCoroutine("AcceptGift");
	}

	IEnumerator AcceptGift()
	{
		WWWForm form = new WWWForm();
		form.AddField("user_gift_id",GiftID);
		form.AddField("user_receive_id",UserCommonData.pGlobal.user.user_id);

		WWW loader = new WWW(GiftLoader.pGlobal.ReceiveGiftURL,form);
		yield return loader;
		Debug.Log(loader.text);
		yield return StartCoroutine(postAccept());
		LoadingScript.HideLoading();
	}
}
