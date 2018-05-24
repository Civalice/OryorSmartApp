using UnityEngine;
using System.Collections;
using TMPro;

public class MyRankingItem : MonoBehaviour {
	public GameObject RankText;
	public SpriteRenderer RankIcon;
	public Sprite Rank1;
	public Sprite Rank2;
	public Sprite Rank3;
	public TextMeshPro RankNumber;
	
	public TextMeshPro Name;
	public TextMeshPro Score;

	private RankingData data;
	private int GameID;

	public ButtonObject sharebt;

	public void SetRankItem(RankingData _data,int gameId,int rank)
	{
		data = _data;
		GameID = gameId;
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
		}
	}
	
	void ShareAction()
	{
		//show share popup
		string path = "";
		if (GameID == 4)
		{
			path = "http://www.oryor.com/oryor_smart_app_year2/icongame/Icon_mole_lesson.png";
		}
		else if (GameID == 3)
		{
			path = "http://www.oryor.com/oryor_smart_app_year2/icongame/Icon_dropdrag.png";
		}
		else if (GameID == 2)
		{
			path = "http://www.oryor.com/oryor_smart_app_year2/icongame/Icon_oryor_collector.png";
		}
		else if (GameID == 1)
		{
			path = "http://www.oryor.com/oryor_smart_app_year2/icongame/Icon_gda_rush.png";
		}
		//เราได้คะแนน x,xxx,xxx เกมสนุกมากๆ มาเล่นกับเราหน่อยน้า สนุก ได้ความรู้ และอาจได้ของรางวัลจาก อย. ด้วยนะ คลิ๊กเลย!
//		string Desc = "เราได้คะแนน "+ Score.ToString("#,##0") + " เกมสนุกมากๆ มาเล่นกับเราหน่อยน้า สนุก ได้ความรู้ และอาจได้ของรางวัลจาก อย. ด้วยนะ คลิ๊กเลย!";
		string Desc = "เราได้คะแนน "+ Score.ToString() + " เกมสนุกมากๆ มาเล่นกับเราหน่อยน้า สนุก ได้ความรู้ และอาจได้ของรางวัลจาก อย. ด้วยนะ คลิ๊กเลย!";
		FacebookLogin.pGlobal.PostFacebook(null,"เพื่อนๆ มาเล่นกัน!","Game",Desc,path);
	}
	
	// Use this for initialization
	void awake () {
		sharebt.OnClicked += ShareAction;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
