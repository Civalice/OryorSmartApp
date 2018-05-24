using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameAPIMsg
{

	public string msg;

	public string user_id{
		get{
			return mUsrID;
		}
		set{
			mUsrID = value;
		}
	}
	private string mUsrID;
	public int user_item_heart_no;
	public int user_item_coin_no;
	public int user_item_score_no;
	public string game_name;
	public string user_money;
	public string user_exp;
	public string user_exp_init;
	public string user_exp_next;
	public string user_level;

	public string user_unlock_item1;
	public string user_unlock_item2;
	public string user_unlock_item3;

	public int ini_game_rank;
	public int update_game_rank;
	public int ini_all_rank;
	public int update_all_rank;

	public string[] awardStringList;
};

public class GameUpdateAPI : MonoBehaviour {
	public GameAPIMsg msg;
	public delegate void APICallback(GameAPIMsg msg);
	public event APICallback apiCB;
	public string baseURL = "http://www.oryor.com/oryor_smart_app_year2/oryor_game_api/";
	public string page = "?task=gameUpdate";
	public bool IsDone = true;
	// Use this for initialization
	private WWWForm reform;
	void Start () {
	}
	
	public void wwwCallAPI(WWWForm form)
	{
		reform = form;
		CallForm();
	}

	void CallForm()
	{
		msg = new GameAPIMsg();
		IsDone = false;
		StartCoroutine("ICallAPI",reform);
	}

	IEnumerator ICallAPI(WWWForm form)
	{
		baseURL = UserCommonData.GetURL ();
		//create postform
		WWW api = new WWW(baseURL+"?task=gameUpdate",form);
		Debug.Log (baseURL+"?task=gameUpdate");
		while (!api.isDone)
		{
			//waiting
			yield return null;
		}
		//decrypt msg
		if (api.error == null)
		{
			Debug.Log(api.text);
			JSONObject json = new JSONObject(api.text);
			msg.msg = json["msg"].str;
			if (msg.msg == "OK")
			{
				JSONObject user = json["user"].list[0];

				msg.user_id = user["user_id"].str;
				//naming notice 
				//coin means item plus score
				//score means item plus exp
				//FUCK THE NAMING SERVER!!!
				msg.user_item_heart_no = (int)user["user_item_heart_no"].n;
				msg.user_item_coin_no = (int)user["user_item_coin_no"].n;
				msg.user_item_score_no = (int)user["user_item_score_no"].n;
				msg.user_money = user["user_money"].str;
				msg.user_exp = user["user_exp"].str;
				msg.user_exp_init = user["user_exp_init"].str;
				msg.user_exp_next = user["user_exp_next"].str;
				msg.user_level = user["user_level"].str;

				msg.user_unlock_item1 = ((int)user["user_unlock_item1"].n).ToString();
				msg.user_unlock_item2 = ((int)user["user_unlock_item2"].n).ToString();
				msg.user_unlock_item3 = ((int)user["user_unlock_item3"].n).ToString();

				msg.ini_game_rank = (int)user["init_game_rank"].n;
				msg.update_game_rank = (int)user["update_game_rank"].n;
				msg.ini_all_rank = (int)user["init_all_rank"].n;
				msg.update_all_rank = (int)user["update_all_rank"].n;
				JSONObject awardList = user["unlock_achieve"];
				msg.awardStringList = null;
				if ((awardList.list != null)&&(awardList.list.Count > 0))
				{
					msg.awardStringList = new string[awardList.list.Count];
					int i = 0;
					foreach(JSONObject award in awardList.list)
					{
						msg.awardStringList[i] = award.str;
						i++;
					}
				}
			}
			IsDone = true;

			if (apiCB != null)
				apiCB(msg);
		}
		else
		{
			//Error popup
			PopupObject.ShowAlertPopup("Error","มีปัญหากับการเชื่อมต่อ Server จะลองใหม่อีกครั้งหรือไม่?",
			                           "ปิด",Cancel,
			                           "ลองใหม่",CallForm);
		}
	}


	void Cancel()
	{
		IsDone = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
