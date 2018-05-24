using UnityEngine;
using System.Collections;
using TMPro;

public class OnlineResult : MonoBehaviour {
	public GameObject IconPrefabs;
	public GameObject Event1;//Money And Level
	public GameObject Event2;//Ranking Text
	public GameObject Event3;//Ranking Icon
	public GameObject AwardIcon; //award

	public MoneyGuage moneyGuage;
	public EXPGuage EXPguage;

	public AudioClip congratSound;

	private bool IsHaveAward;
	private bool IsGameRankUp;
	private bool IsAllRankUp;
	private int nextMoney;
	private int nextProgress;
	private int nextLevel;

	private int rank_g_init;
	private int rank_g_next;
	private int rank_a_init;
	private int rank_a_next;

	private GameObject AllRank = null;
	private GameObject GameRank = null;
	private int GameID;
	private Sprite GameIcon;

	public bool IsDone = false;

	const int smooth = 5;

	public void SetGameLogo(Sprite icon)
	{
		GameIcon = icon;
	}

	public void SetupGameID(int id)
	{
		GameID = id;
	}

	public void ShowResult()
	{
		IsDone = false;
		StartCoroutine ("RunResult");
	}

	public void SetStartResult(int money,int progress,int level)
	{
		Vector3 scale = new Vector3 (0, 0, 1);
		Event1.transform.localScale = scale;
		Event2.transform.localScale = scale;
		AwardIcon.transform.localScale = scale;
		AwardIcon.SetActive(false);
		if (AllRank != null)
			Destroy (AllRank);
		if (GameRank != null)
			Destroy (GameRank);
		AllRank = null;
		GameRank = null;

		moneyGuage.SetMoney(money);
		EXPguage.SetCurrentProgress (progress, level);
	}

	public void SetResult(int money,int progress,int level,int r_g_start,int r_g_next,int r_a_start,int r_a_next,bool HaveAward = false)
	{
		IsHaveAward = HaveAward;
		AwardIcon.SetActive(HaveAward);
		//moneyGuage
		nextMoney = money;
		nextProgress = progress;
		nextLevel = level;
		//Ranking
		rank_g_init = r_g_start;
		rank_g_next = r_g_next;
		rank_a_init = r_a_start;
		rank_a_next = r_a_next;

		if (rank_g_init > rank_g_next)
			IsGameRankUp = true;
		else
			IsGameRankUp = false;
		if (rank_a_init > rank_a_next)
			IsAllRankUp = true;
		else
			IsAllRankUp = false;

		//ranking number
		//all Rank Item
		if (IsAllRankUp) {
			AllRank = (GameObject)GameObject.Instantiate(IconPrefabs);
			AllRank.transform.parent = Event3.transform;
			AllRank.transform.localPosition = new Vector3(0,0,0);
			RankingIcon ricon = AllRank.GetComponent<RankingIcon>();
			ricon.Reset();
		}
		if (IsGameRankUp) {
			GameRank = (GameObject)GameObject.Instantiate (IconPrefabs);
			GameRank.transform.parent = Event3.transform;
			GameRank.transform.localPosition = new Vector3(0,0,0);
			RankingIcon ricon = AllRank.GetComponent<RankingIcon>();
			ricon.Reset();
			ricon.SetIcon(GameIcon);
		}

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator RunEvent1()
	{
		Vector3 nScale = new Vector3 (1, 1, 1);
		GameObject icon = Event1;
		while (Vector3.Distance(icon.transform.localScale,nScale)>0.01f) {
			Vector3 cScale = Vector3.Lerp(icon.transform.localScale,nScale,Time.deltaTime*smooth);
			AwardIcon.transform.localScale = cScale;
			icon.transform.localScale = cScale;
			yield return null;
		}
		icon.transform.localScale = nScale;

		EXPguage.SetNextProgress (nextProgress, nextLevel);
		EXPguage.RunProgress ();
		moneyGuage.SetMoney (nextMoney);
	}

	IEnumerator RunEvent2()
	{
		Vector3 nScale = new Vector3 (1, 1, 1);
		GameObject icon = Event2;
		while (Vector3.Distance(icon.transform.localScale,nScale)>0.01f) {
			Vector3 cScale = Vector3.Lerp(icon.transform.localScale,nScale,Time.deltaTime*smooth);
			icon.transform.localScale = cScale;
			yield return null;
		}
		icon.transform.localScale = nScale;
	}
	
	IEnumerator RunEvent3()
	{

		if (GameRank != null)
		{
			RankingIcon gameRankComp = GameRank.GetComponent<RankingIcon> ();
			gameRankComp.ShowRanking (rank_g_init,rank_g_next);
		}
		if (AllRank != null) {
			RankingIcon allRankComp = AllRank.GetComponent<RankingIcon> ();
			allRankComp.ShowRanking (rank_a_init,rank_a_next);
		}
		if (IsAllRankUp && IsGameRankUp) {
			AllRank.transform.localPosition = new Vector3(-1.2f,0,0);
			GameRank.transform.localPosition = new Vector3(1.2f,0,0);
		}
		yield return null;
	}
	
	IEnumerator RunResult()
	{
		if (IsHaveAward)
			GetComponent<AudioSource>().PlayOneShot(congratSound);
		StartCoroutine ("RunEvent1");
		if (IsGameRankUp || IsAllRankUp) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine ("RunEvent2");
			yield return new WaitForSeconds (0.5f);
			StartCoroutine ("RunEvent3");
		}
		IsDone = true;
		yield return null;
	}
}
