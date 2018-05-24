using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class InfoSign : MonoBehaviour {
	
	public enum SignType
	{
		Food,
		Drug,
		Cosmatic,
		Medical,
		Dangerous
	}

	[SerializeField]
	private InfoSignSprite [] infoSignSprites;

	[SerializeField]
	private GameObject text, sign;
	[SerializeField]
	private Image gauge;

	private static InfoSign inst;

	public static InfoSign GetInstance()
	{
		if(inst == null)
		{
			Debug.LogError("No Instance of InfoSign");
			Debug.Break();
		}
		return inst;
	}

	void Awake()
	{
		inst = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Close()
	{
		text.SetActive(false);
		sign.SetActive(false);
		gauge.fillAmount = 0f;
	}

	public void SwitchSign(SignType signType)
	{
		text.SetActive(true);
		sign.SetActive(true);

		var infoSignSprite = infoSignSprites[ (int)signType ];
		text.GetComponent<SpriteRenderer>().sprite = infoSignSprite.Text;
		sign.GetComponent<SpriteRenderer>().sprite = infoSignSprite.Sign;
	}

	[Serializable]
	internal class InfoSignSprite
	{
		public Sprite Text;
		public Sprite Sign;
	}
}