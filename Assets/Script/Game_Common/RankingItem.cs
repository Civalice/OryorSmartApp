using UnityEngine;
using System.Collections;
using TMPro;

//[INDE] tag for Add Indicator Work

public class RankingItem : MonoBehaviour {
	public GameObject RankText;
	public ButtonObject SendGiftButton;
	public SpriteRenderer RankIcon;
	public Sprite Rank1;
	public Sprite Rank2;
	public Sprite Rank3;
	public TextMeshPro RankNumber;

	public TextMeshPro Name;
	public TextMeshPro Score;
	public TextMeshPro CountDown;

	private RankingData data;

	public GiftSend giftCD;
	public GameObject Loading;

	bool IsSending = false;
	public void SetRankItem(int idx,float Height,RankingData _data,int gameId,int rank)
	{
		data = _data;
		Name.text = _data.user_name;
		if (rank == 1)
		{
			RankText.SetActive(false);
			RankIcon.gameObject.SetActive(true);
			RankIcon.sprite = Rank1;
		}
		else if (rank == 2)
		{
			RankText.SetActive(false);
			RankIcon.gameObject.SetActive(true);
			RankIcon.sprite = Rank2;
		}
		else if (rank == 3)
		{
			RankText.SetActive(false);
			RankIcon.gameObject.SetActive(true);
			RankIcon.sprite = Rank3;
		}
		else
		{
			RankText.SetActive(true);
			RankIcon.gameObject.SetActive(false);
		}
		RankNumber.text = rank.ToString();
		switch(gameId)
		{
		case 0:
			//all
			Score.text = _data.scoreAll.ToString("#,##0");
			break;
		case 1:
			Score.text = _data.score[0].ToString("#,##0");
			break;
		case 2:
			Score.text = _data.score[1].ToString("#,##0");
			break;
		case 3:
			Score.text = _data.score[2].ToString("#,##0");
			break;
		case 4:
			Score.text = _data.score[3].ToString("#,##0");
			break;
		case 5:
			Score.text = _data.score[4].ToString("#,##0");
			break;
		case 6:
			Score.text = _data.score[5].ToString("#,##0");
			break;
		}
		//check if Gift is avaliable
		giftCD = GiftSentGlobal.getGift(data.userId);
		if ((giftCD != null)&&(GiftSentGlobal.CalculateCountDownTick(giftCD)>0))
		{
			//start countdown Routine
			StartCountDown();
		}
		else
		{
			//enable sent gift
			SendGiftButton.enabled = true;
			SendGiftButton.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,1.0f);
			CountDown.gameObject.SetActive(false);
		}
		transform.localPosition = new Vector3(0,idx*-Height,0);
	}

	void StartCountDown()
	{
		if (giftCD == null) return;
		Debug.Log("Start Countdown");
		//[INDE]
		//Hide Indicator
		Loading.SetActive(false);
		SendGiftButton.enabled = false;
		SendGiftButton.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
		StopCoroutine("mCountDown");

		StartCoroutine("mCountDown");
	}

	IEnumerator mCountDown()
	{
		Debug.Log("Start Coroutine CD");
		CountDown.gameObject.SetActive(true);
		long startTick = long.Parse(giftCD.tick);
		long countTick = System.DateTime.UtcNow.Ticks-startTick;
		long CountdownTick = GiftSentGlobal.GiftCountdown-countTick;
		while (CountdownTick > 0)
		{
			countTick = System.DateTime.UtcNow.Ticks-startTick;
			CountdownTick = GiftSentGlobal.GiftCountdown-countTick;
			System.TimeSpan ts = System.TimeSpan.FromTicks(CountdownTick);
			CountDown.text = ts.Hours.ToString("0") + ":" + ts.Minutes.ToString("00") + ":"+ts.Seconds.ToString("00");
			yield return null;
		}
		//enable gift
		Debug.Log("Done");
		SendGiftButton.enabled = true;
		SendGiftButton.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,1.0f);
		CountDown.gameObject.SetActive(false);
		yield return null;
	}

	void SendGiftCB()
	{
		if (IsSending) return;
		Debug.Log("Send Gift");
		//[INDE]
		//show Indecator
		IsSending = true;
		Loading.SetActive(true);
		StartCoroutine("SendingByURL");
	}

	IEnumerator SendingByURL()
	{
		WWWForm form = new WWWForm();
		form.AddField("user_sent_id",UserCommonData.pGlobal.user.user_id);

		form.AddField("user_receive_id",data.userId);
		form.AddField("gift_type_id",Random.Range(0,4));

		WWW loader = new WWW(GiftLoader.pGlobal.SentGiftURL,form);
		yield return loader;
		Debug.Log(loader.text);
		//check msg
		JSONObject json = new JSONObject(loader.text);
		string msg = json["msg"].str;
		if (msg == "OK")
		{
			Debug.Log("Set GiftCD");
			giftCD = new GiftSend();
			giftCD.user_id = data.userId;
			giftCD.tick = System.DateTime.UtcNow.Ticks.ToString();
			GiftSentGlobal.AddGift(giftCD);
			//start countdown here
			StartCountDown();
//			System.TimeSpan test = System.TimeSpan.FromTicks(long.Parse(gift.tick)-System.DateTime.UtcNow.Ticks);

		}
		IsSending = false;
//		yield return StartCoroutine(postAccept());
//		LoadingScript.HideLoading();
	}

	void Awake()
	{
		Loading.SetActive(false);
		SendGiftButton.OnReleased += SendGiftCB;
		Name.maxVisibleLines = 1;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
